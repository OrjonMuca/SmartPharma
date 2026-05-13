namespace smartPharmaAPI.Models;

public sealed class Prescription
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? PatientUserId { get; set; }
    public required string PatientName { get; set; }
    public required string DoctorName { get; set; }
    public string? Notes { get; set; }
    public PrescriptionStatus Status { get; set; } = PrescriptionStatus.Pending;
    public DateOnly IssuedDate { get; set; }
    public DateOnly ExpiresDate { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public AppUser? PatientUser { get; set; }
    public ICollection<PrescriptionItem> Items { get; set; } = new List<PrescriptionItem>();
}
