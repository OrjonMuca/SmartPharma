using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smartPharmaAPI.Data;
using smartPharmaAPI.Dtos;
using smartPharmaAPI.Models;

namespace smartPharmaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Pharmacist")]
public sealed class InventoryController : ControllerBase
{
    private readonly SmartPharmaDbContext _db;

    public InventoryController(SmartPharmaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<InventoryItemResponse>>> GetInventory(CancellationToken cancellationToken)
    {
        var items = await InventoryQuery()
            .OrderBy(item => item.Medicine!.Name)
            .ThenBy(item => item.ExpirationDate)
            .ToListAsync(cancellationToken);

        return Ok(items.Select(ToResponse).ToList());
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<List<InventoryItemResponse>>> GetLowStock(CancellationToken cancellationToken)
    {
        var items = await InventoryQuery()
            .Where(item => item.Quantity <= item.ReorderLevel)
            .OrderBy(item => item.Quantity)
            .ToListAsync(cancellationToken);

        return Ok(items.Select(ToResponse).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<InventoryItemResponse>> GetInventoryItem(Guid id, CancellationToken cancellationToken)
    {
        var item = await InventoryQuery()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);

        return item is null ? NotFound() : Ok(ToResponse(item));
    }

    [HttpPost]
    public async Task<ActionResult<InventoryItemResponse>> CreateInventoryItem(
        InventoryItemRequest request,
        CancellationToken cancellationToken)
    {
        var validationError = await ValidateRequestAsync(request, null, cancellationToken);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        var item = new InventoryItem
        {
            MedicineId = request.MedicineId,
            SupplierId = request.SupplierId,
            BatchNumber = request.BatchNumber.Trim(),
            Quantity = request.Quantity,
            ReorderLevel = request.ReorderLevel,
            ExpirationDate = request.ExpirationDate,
            LastUpdatedAtUtc = DateTime.UtcNow
        };

        _db.InventoryItems.Add(item);
        await _db.SaveChangesAsync(cancellationToken);

        item = await InventoryQuery().SingleAsync(inventoryItem => inventoryItem.Id == item.Id, cancellationToken);
        return CreatedAtAction(nameof(GetInventoryItem), new { id = item.Id }, ToResponse(item));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<InventoryItemResponse>> UpdateInventoryItem(
        Guid id,
        InventoryItemRequest request,
        CancellationToken cancellationToken)
    {
        var validationError = await ValidateRequestAsync(request, id, cancellationToken);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        var item = await _db.InventoryItems.FindAsync([id], cancellationToken);
        if (item is null)
        {
            return NotFound();
        }

        item.MedicineId = request.MedicineId;
        item.SupplierId = request.SupplierId;
        item.BatchNumber = request.BatchNumber.Trim();
        item.Quantity = request.Quantity;
        item.ReorderLevel = request.ReorderLevel;
        item.ExpirationDate = request.ExpirationDate;
        item.LastUpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        item = await InventoryQuery().SingleAsync(inventoryItem => inventoryItem.Id == id, cancellationToken);
        return Ok(ToResponse(item));
    }

    [HttpPatch("{id:guid}/quantity")]
    public async Task<ActionResult<InventoryItemResponse>> UpdateQuantity(
        Guid id,
        UpdateInventoryQuantityRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Quantity < 0)
        {
            return BadRequest("Quantity cannot be negative.");
        }

        var item = await _db.InventoryItems.FindAsync([id], cancellationToken);
        if (item is null)
        {
            return NotFound();
        }

        item.Quantity = request.Quantity;
        item.LastUpdatedAtUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);

        item = await InventoryQuery().SingleAsync(inventoryItem => inventoryItem.Id == id, cancellationToken);
        return Ok(ToResponse(item));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteInventoryItem(Guid id, CancellationToken cancellationToken)
    {
        var item = await _db.InventoryItems.FindAsync([id], cancellationToken);
        if (item is null)
        {
            return NotFound();
        }

        _db.InventoryItems.Remove(item);
        await _db.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private IQueryable<InventoryItem> InventoryQuery()
    {
        return _db.InventoryItems
            .Include(item => item.Medicine)
            .Include(item => item.Supplier)
            .AsNoTracking();
    }

    private async Task<string?> ValidateRequestAsync(
        InventoryItemRequest request,
        Guid? existingInventoryItemId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.BatchNumber))
        {
            return "Batch number is required.";
        }

        if (request.Quantity < 0)
        {
            return "Quantity cannot be negative.";
        }

        if (request.ReorderLevel < 0)
        {
            return "Reorder level cannot be negative.";
        }

        if (!await _db.Medicines.AnyAsync(medicine => medicine.Id == request.MedicineId, cancellationToken))
        {
            return "Medicine does not exist.";
        }

        if (request.SupplierId.HasValue &&
            !await _db.Suppliers.AnyAsync(supplier => supplier.Id == request.SupplierId.Value, cancellationToken))
        {
            return "Supplier does not exist.";
        }

        var batchNumber = request.BatchNumber.Trim();
        var duplicateBatchExists = await _db.InventoryItems.AnyAsync(
            item => item.MedicineId == request.MedicineId &&
                    item.BatchNumber == batchNumber &&
                    item.Id != existingInventoryItemId,
            cancellationToken);

        return duplicateBatchExists ? "This medicine already has an inventory item with the same batch number." : null;
    }

    private static InventoryItemResponse ToResponse(InventoryItem item)
    {
        return new InventoryItemResponse(
            item.Id,
            item.MedicineId,
            item.Medicine?.Name ?? "Unknown medicine",
            item.BatchNumber,
            item.Quantity,
            item.ReorderLevel,
            item.Quantity <= item.ReorderLevel,
            item.ExpirationDate,
            item.SupplierId,
            item.Supplier?.Name,
            item.LastUpdatedAtUtc);
    }
}
