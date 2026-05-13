using Microsoft.EntityFrameworkCore;
using smartPharmaAPI.Data;
using smartPharmaAPI.Dtos;
using smartPharmaAPI.Models;

namespace smartPharmaAPI.Services;

public sealed class OrderService
{
    private readonly SmartPharmaDbContext _db;

    public OrderService(SmartPharmaDbContext db)
    {
        _db = db;
    }

    public async Task<OperationResult<Order>> PlaceOrderAsync(OrderRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerName))
        {
            return OperationResult<Order>.Failure("Customer name is required.");
        }

        if (request.Items.Count == 0)
        {
            return OperationResult<Order>.Failure("At least one order item is required.");
        }

        if (request.Items.Any(item => item.Quantity <= 0))
        {
            return OperationResult<Order>.Failure("Order item quantities must be greater than zero.");
        }

        var requestedMedicineIds = request.Items.Select(item => item.MedicineId).Distinct().ToList();
        var medicines = await _db.Medicines
            .Include(medicine => medicine.InventoryItems)
            .Where(medicine => requestedMedicineIds.Contains(medicine.Id))
            .ToListAsync(cancellationToken);

        if (medicines.Count != requestedMedicineIds.Count)
        {
            return OperationResult<Order>.Failure("One or more medicines do not exist.");
        }

        var prescription = await GetPrescriptionAsync(request.PrescriptionId, cancellationToken);
        if (request.PrescriptionId.HasValue && prescription is null)
        {
            return OperationResult<Order>.Failure("Prescription was not found.");
        }

        foreach (var item in request.Items)
        {
            var medicine = medicines.Single(medicine => medicine.Id == item.MedicineId);
            if (!medicine.IsActive)
            {
                return OperationResult<Order>.Failure($"{medicine.Name} is not active.");
            }

            if (medicine.RequiresPrescription && !PrescriptionCoversMedicine(prescription, item))
            {
                return OperationResult<Order>.Failure($"{medicine.Name} requires an approved prescription.");
            }

            var availableQuantity = medicine.InventoryItems.Sum(inventoryItem => inventoryItem.Quantity);
            if (availableQuantity < item.Quantity)
            {
                return OperationResult<Order>.Failure($"{medicine.Name} has only {availableQuantity} units available.");
            }
        }

        var order = new Order
        {
            CustomerUserId = request.CustomerUserId,
            CustomerName = request.CustomerName.Trim(),
            PrescriptionId = request.PrescriptionId,
            Status = OrderStatus.Confirmed,
            Items = new List<OrderItem>()
        };

        foreach (var item in request.Items)
        {
            var medicine = medicines.Single(medicine => medicine.Id == item.MedicineId);

            order.Items.Add(new OrderItem
            {
                MedicineId = medicine.Id,
                Quantity = item.Quantity,
                UnitPrice = medicine.Price
            });

            order.TotalAmount += medicine.Price * item.Quantity;
            ReduceInventory(medicine, item.Quantity);
        }

        if (prescription is not null && prescription.Status == PrescriptionStatus.Approved)
        {
            prescription.Status = PrescriptionStatus.Fulfilled;
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync(cancellationToken);

        return OperationResult<Order>.Success(order);
    }

    private async Task<Prescription?> GetPrescriptionAsync(Guid? prescriptionId, CancellationToken cancellationToken)
    {
        if (!prescriptionId.HasValue)
        {
            return null;
        }

        return await _db.Prescriptions
            .Include(prescription => prescription.Items)
            .FirstOrDefaultAsync(prescription => prescription.Id == prescriptionId.Value, cancellationToken);
    }

    private static bool PrescriptionCoversMedicine(Prescription? prescription, OrderItemRequest item)
    {
        if (prescription is null || prescription.Status != PrescriptionStatus.Approved)
        {
            return false;
        }

        return prescription.Items.Any(prescriptionItem =>
            prescriptionItem.MedicineId == item.MedicineId &&
            prescriptionItem.Quantity >= item.Quantity);
    }

    private static void ReduceInventory(Medicine medicine, int quantity)
    {
        var remaining = quantity;
        foreach (var batch in medicine.InventoryItems.OrderBy(item => item.ExpirationDate))
        {
            if (remaining == 0)
            {
                break;
            }

            var consumed = Math.Min(batch.Quantity, remaining);
            batch.Quantity -= consumed;
            batch.LastUpdatedAtUtc = DateTime.UtcNow;
            remaining -= consumed;
        }
    }
}
