using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smartPharmaAPI.Data;
using smartPharmaAPI.Dtos;
using smartPharmaAPI.Models;

namespace smartPharmaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PrescriptionsController : ControllerBase
{
    private readonly SmartPharmaDbContext _db;

    public PrescriptionsController(SmartPharmaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<PrescriptionResponse>>> GetPrescriptions(CancellationToken cancellationToken)
    {
        var query = PrescriptionQuery();
        if (!IsStaff())
        {
            var currentUserId = GetCurrentUserId();
            query = query.Where(prescription => prescription.PatientUserId == currentUserId);
        }

        var prescriptions = await query
            .OrderByDescending(prescription => prescription.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return Ok(prescriptions.Select(ToResponse).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PrescriptionResponse>> GetPrescription(Guid id, CancellationToken cancellationToken)
    {
        var prescription = await PrescriptionQuery()
            .FirstOrDefaultAsync(prescription => prescription.Id == id, cancellationToken);

        if (prescription is null)
        {
            return NotFound();
        }

        if (!IsStaff() && prescription.PatientUserId != GetCurrentUserId())
        {
            return Forbid();
        }

        return Ok(ToResponse(prescription));
    }

    [HttpPost]
    public async Task<ActionResult<PrescriptionResponse>> CreatePrescription(
        PrescriptionRequest request,
        CancellationToken cancellationToken)
    {
        var validationError = await ValidateRequestAsync(request, cancellationToken);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        var patientUserId = request.PatientUserId;
        if (!IsStaff())
        {
            patientUserId = GetCurrentUserId();
        }

        var prescription = new Prescription
        {
            PatientUserId = patientUserId,
            PatientName = request.PatientName.Trim(),
            DoctorName = request.DoctorName.Trim(),
            Notes = request.Notes?.Trim(),
            IssuedDate = request.IssuedDate,
            ExpiresDate = request.ExpiresDate,
            Status = PrescriptionStatus.Pending,
            Items = request.Items.Select(item => new PrescriptionItem
            {
                MedicineId = item.MedicineId,
                Quantity = item.Quantity,
                Instructions = item.Instructions?.Trim()
            }).ToList()
        };

        _db.Prescriptions.Add(prescription);
        await _db.SaveChangesAsync(cancellationToken);

        prescription = await PrescriptionQuery().SingleAsync(item => item.Id == prescription.Id, cancellationToken);
        return CreatedAtAction(nameof(GetPrescription), new { id = prescription.Id }, ToResponse(prescription));
    }

    [Authorize(Roles = "Admin,Pharmacist")]
    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<PrescriptionResponse>> UpdateStatus(
        Guid id,
        UpdatePrescriptionStatusRequest request,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<PrescriptionStatus>(request.Status, true, out var status))
        {
            return BadRequest("Status must be Pending, Approved, Rejected, or Fulfilled.");
        }

        var prescription = await _db.Prescriptions.FindAsync([id], cancellationToken);
        if (prescription is null)
        {
            return NotFound();
        }

        prescription.Status = status;
        await _db.SaveChangesAsync(cancellationToken);

        prescription = await PrescriptionQuery().SingleAsync(item => item.Id == id, cancellationToken);
        return Ok(ToResponse(prescription));
    }

    [Authorize(Roles = "Admin,Pharmacist")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePrescription(Guid id, CancellationToken cancellationToken)
    {
        var prescription = await _db.Prescriptions.FindAsync([id], cancellationToken);
        if (prescription is null)
        {
            return NotFound();
        }

        _db.Prescriptions.Remove(prescription);
        await _db.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private IQueryable<Prescription> PrescriptionQuery()
    {
        return _db.Prescriptions
            .Include(prescription => prescription.Items)
            .ThenInclude(item => item.Medicine)
            .AsNoTracking();
    }

    private async Task<string?> ValidateRequestAsync(PrescriptionRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.PatientName) || string.IsNullOrWhiteSpace(request.DoctorName))
        {
            return "Patient name and doctor name are required.";
        }

        if (request.ExpiresDate < request.IssuedDate)
        {
            return "Expiration date cannot be before issued date.";
        }

        if (request.Items.Count == 0)
        {
            return "At least one prescription item is required.";
        }

        if (request.Items.Any(item => item.Quantity <= 0))
        {
            return "Prescription item quantities must be greater than zero.";
        }

        if (request.PatientUserId.HasValue &&
            !await _db.Users.AnyAsync(user => user.Id == request.PatientUserId.Value, cancellationToken))
        {
            return "Patient user does not exist.";
        }

        var medicineIds = request.Items.Select(item => item.MedicineId).Distinct().ToList();
        var existingMedicineCount = await _db.Medicines
            .CountAsync(medicine => medicineIds.Contains(medicine.Id), cancellationToken);

        return existingMedicineCount == medicineIds.Count ? null : "One or more medicines do not exist.";
    }

    private bool IsStaff()
    {
        return User.IsInRole(UserRole.Admin.ToString()) || User.IsInRole(UserRole.Pharmacist.ToString());
    }

    private Guid? GetCurrentUserId()
    {
        var rawUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(rawUserId, out var userId) ? userId : null;
    }

    private static PrescriptionResponse ToResponse(Prescription prescription)
    {
        return new PrescriptionResponse(
            prescription.Id,
            prescription.PatientUserId,
            prescription.PatientName,
            prescription.DoctorName,
            prescription.Notes,
            prescription.Status.ToString(),
            prescription.IssuedDate,
            prescription.ExpiresDate,
            prescription.CreatedAtUtc,
            prescription.Items
                .OrderBy(item => item.Medicine!.Name)
                .Select(item => new PrescriptionItemResponse(
                    item.Id,
                    item.MedicineId,
                    item.Medicine?.Name ?? "Unknown medicine",
                    item.Quantity,
                    item.Instructions))
                .ToList());
    }
}
