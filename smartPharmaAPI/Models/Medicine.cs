namespace smartPharmaAPI.Models;

public sealed class Medicine
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string? GenericName { get; set; }
    public string? Description { get; set; }
    public required string Manufacturer { get; set; }
    public required string DosageForm { get; set; }
    public required string Strength { get; set; }
    public required string Category { get; set; }
    public decimal Price { get; set; }
    public bool RequiresPrescription { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    public ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
