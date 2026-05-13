namespace smartPharmaAPI.Models;

public sealed class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? CustomerUserId { get; set; }
    public required string CustomerName { get; set; }
    public Guid? PrescriptionId { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Confirmed;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public AppUser? CustomerUser { get; set; }
    public Prescription? Prescription { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
