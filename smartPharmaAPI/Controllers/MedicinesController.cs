using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smartPharmaAPI.Data;
using smartPharmaAPI.Dtos;
using smartPharmaAPI.Models;

namespace smartPharmaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class MedicinesController : ControllerBase
{
    private readonly SmartPharmaDbContext _db;

    public MedicinesController(SmartPharmaDbContext db)
    {
        _db = db;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<MedicineResponse>>> GetMedicines(
        [FromQuery] string? search,
        [FromQuery] string? category,
        CancellationToken cancellationToken)
    {
        var query = _db.Medicines
            .Include(medicine => medicine.InventoryItems)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim();
            query = query.Where(medicine =>
                medicine.Name.Contains(normalizedSearch) ||
                (medicine.GenericName != null && medicine.GenericName.Contains(normalizedSearch)));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            var normalizedCategory = category.Trim();
            query = query.Where(medicine => medicine.Category == normalizedCategory);
        }

        var medicines = await query
            .OrderBy(medicine => medicine.Name)
            .ToListAsync(cancellationToken);

        return Ok(medicines.Select(ToResponse).ToList());
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MedicineResponse>> GetMedicine(Guid id, CancellationToken cancellationToken)
    {
        var medicine = await _db.Medicines
            .Include(medicine => medicine.InventoryItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(medicine => medicine.Id == id, cancellationToken);

        return medicine is null ? NotFound() : Ok(ToResponse(medicine));
    }

    [Authorize(Roles = "Admin,Pharmacist")]
    [HttpPost]
    public async Task<ActionResult<MedicineResponse>> CreateMedicine(MedicineRequest request, CancellationToken cancellationToken)
    {
        var validationError = Validate(request);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        var medicine = new Medicine
        {
            Name = request.Name.Trim(),
            GenericName = request.GenericName?.Trim(),
            Description = request.Description?.Trim(),
            Manufacturer = request.Manufacturer.Trim(),
            DosageForm = request.DosageForm.Trim(),
            Strength = request.Strength.Trim(),
            Category = request.Category.Trim(),
            Price = request.Price,
            RequiresPrescription = request.RequiresPrescription,
            IsActive = request.IsActive
        };

        _db.Medicines.Add(medicine);
        await _db.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetMedicine), new { id = medicine.Id }, ToResponse(medicine));
    }

    [Authorize(Roles = "Admin,Pharmacist")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MedicineResponse>> UpdateMedicine(Guid id, MedicineRequest request, CancellationToken cancellationToken)
    {
        var validationError = Validate(request);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        var medicine = await _db.Medicines
            .Include(medicine => medicine.InventoryItems)
            .FirstOrDefaultAsync(medicine => medicine.Id == id, cancellationToken);

        if (medicine is null)
        {
            return NotFound();
        }

        medicine.Name = request.Name.Trim();
        medicine.GenericName = request.GenericName?.Trim();
        medicine.Description = request.Description?.Trim();
        medicine.Manufacturer = request.Manufacturer.Trim();
        medicine.DosageForm = request.DosageForm.Trim();
        medicine.Strength = request.Strength.Trim();
        medicine.Category = request.Category.Trim();
        medicine.Price = request.Price;
        medicine.RequiresPrescription = request.RequiresPrescription;
        medicine.IsActive = request.IsActive;

        await _db.SaveChangesAsync(cancellationToken);

        return Ok(ToResponse(medicine));
    }

    [Authorize(Roles = "Admin,Pharmacist")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeactivateMedicine(Guid id, CancellationToken cancellationToken)
    {
        var medicine = await _db.Medicines.FindAsync([id], cancellationToken);
        if (medicine is null)
        {
            return NotFound();
        }

        medicine.IsActive = false;
        await _db.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private static string? Validate(MedicineRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Manufacturer) ||
            string.IsNullOrWhiteSpace(request.DosageForm) ||
            string.IsNullOrWhiteSpace(request.Strength) ||
            string.IsNullOrWhiteSpace(request.Category))
        {
            return "Name, manufacturer, dosage form, strength, and category are required.";
        }

        return request.Price < 0 ? "Price cannot be negative." : null;
    }

    private static MedicineResponse ToResponse(Medicine medicine)
    {
        var stockQuantity = medicine.InventoryItems.Sum(item => item.Quantity);
        var lowStock = medicine.InventoryItems.Any(item => item.Quantity <= item.ReorderLevel);

        return new MedicineResponse(
            medicine.Id,
            medicine.Name,
            medicine.GenericName,
            medicine.Description,
            medicine.Manufacturer,
            medicine.DosageForm,
            medicine.Strength,
            medicine.Category,
            medicine.Price,
            medicine.RequiresPrescription,
            medicine.IsActive,
            stockQuantity,
            lowStock);
    }
}
