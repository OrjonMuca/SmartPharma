namespace smartPharmaAPI.Models;

public sealed class InventoryItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MedicineId { get; set; }
    public required string BatchNumber { get; set; }
    public int Quantity { get; set; }
    public int ReorderLevel { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public Guid? SupplierId { get; set; }
    public DateTime LastUpdatedAtUtc { get; set; } = DateTime.UtcNow;

    public Medicine? Medicine { get; set; }
    public Supplier? Supplier { get; set; }
}
