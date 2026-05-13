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
public sealed class SuppliersController : ControllerBase
{
    private readonly SmartPharmaDbContext _db;

    public SuppliersController(SmartPharmaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<SupplierResponse>>> GetSuppliers(CancellationToken cancellationToken)
    {
        var suppliers = await _db.Suppliers
            .AsNoTracking()
            .OrderBy(supplier => supplier.Name)
            .Select(supplier => ToResponse(supplier))
            .ToListAsync(cancellationToken);

        return Ok(suppliers);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SupplierResponse>> GetSupplier(Guid id, CancellationToken cancellationToken)
    {
        var supplier = await _db.Suppliers
            .AsNoTracking()
            .FirstOrDefaultAsync(supplier => supplier.Id == id, cancellationToken);

        return supplier is null ? NotFound() : Ok(ToResponse(supplier));
    }

    [HttpPost]
    public async Task<ActionResult<SupplierResponse>> CreateSupplier(SupplierRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Supplier name is required.");
        }

        var name = request.Name.Trim();
        if (await _db.Suppliers.AnyAsync(supplier => supplier.Name == name, cancellationToken))
        {
            return Conflict("A supplier with this name already exists.");
        }

        var supplier = new Supplier
        {
            Name = name,
            ContactPerson = request.ContactPerson?.Trim(),
            Email = request.Email?.Trim(),
            Phone = request.Phone?.Trim(),
            Address = request.Address?.Trim()
        };

        _db.Suppliers.Add(supplier);
        await _db.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetSupplier), new { id = supplier.Id }, ToResponse(supplier));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<SupplierResponse>> UpdateSupplier(Guid id, SupplierRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Supplier name is required.");
        }

        var supplier = await _db.Suppliers.FindAsync([id], cancellationToken);
        if (supplier is null)
        {
            return NotFound();
        }

        var name = request.Name.Trim();
        var duplicateExists = await _db.Suppliers.AnyAsync(
            other => other.Id != id && other.Name == name,
            cancellationToken);

        if (duplicateExists)
        {
            return Conflict("A supplier with this name already exists.");
        }

        supplier.Name = name;
        supplier.ContactPerson = request.ContactPerson?.Trim();
        supplier.Email = request.Email?.Trim();
        supplier.Phone = request.Phone?.Trim();
        supplier.Address = request.Address?.Trim();

        await _db.SaveChangesAsync(cancellationToken);

        return Ok(ToResponse(supplier));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSupplier(Guid id, CancellationToken cancellationToken)
    {
        var supplier = await _db.Suppliers.FindAsync([id], cancellationToken);
        if (supplier is null)
        {
            return NotFound();
        }

        await _db.InventoryItems
            .Where(item => item.SupplierId == id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(item => item.SupplierId, (Guid?)null), cancellationToken);

        _db.Suppliers.Remove(supplier);
        await _db.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private static SupplierResponse ToResponse(Supplier supplier)
    {
        return new SupplierResponse(
            supplier.Id,
            supplier.Name,
            supplier.ContactPerson,
            supplier.Email,
            supplier.Phone,
            supplier.Address,
            supplier.CreatedAtUtc);
    }
}
