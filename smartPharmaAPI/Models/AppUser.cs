namespace smartPharmaAPI.Models;

public sealed class AppUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; } = UserRole.Customer;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
