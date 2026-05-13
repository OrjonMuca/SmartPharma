namespace smartPharmaAPI.Models;

public sealed class PrescriptionItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PrescriptionId { get; set; }
    public Guid MedicineId { get; set; }
    public int Quantity { get; set; }
    public string? Instructions { get; set; }

    public Prescription? Prescription { get; set; }
    public Medicine? Medicine { get; set; }
}
