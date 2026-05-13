using Microsoft.EntityFrameworkCore;
using smartPharmaAPI.Models;

namespace smartPharmaAPI.Data;

public sealed class SmartPharmaDbContext : DbContext
{
    public SmartPharmaDbContext(DbContextOptions<SmartPharmaDbContext> options) : base(options)
    {
    }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Medicine> Medicines => Set<Medicine>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<PrescriptionItem> PrescriptionItems => Set<PrescriptionItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasIndex(user => user.Email).IsUnique();
            entity.Property(user => user.FullName).HasMaxLength(160);
            entity.Property(user => user.Email).HasMaxLength(256);
            entity.Property(user => user.Role).HasConversion<string>().HasMaxLength(32);
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.HasIndex(medicine => medicine.Name);
            entity.Property(medicine => medicine.Name).HasMaxLength(180);
            entity.Property(medicine => medicine.GenericName).HasMaxLength(180);
            entity.Property(medicine => medicine.Manufacturer).HasMaxLength(180);
            entity.Property(medicine => medicine.DosageForm).HasMaxLength(80);
            entity.Property(medicine => medicine.Strength).HasMaxLength(80);
            entity.Property(medicine => medicine.Category).HasMaxLength(100);
            entity.Property(medicine => medicine.Price).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasIndex(supplier => supplier.Name).IsUnique();
            entity.Property(supplier => supplier.Name).HasMaxLength(180);
            entity.Property(supplier => supplier.ContactPerson).HasMaxLength(160);
            entity.Property(supplier => supplier.Email).HasMaxLength(256);
            entity.Property(supplier => supplier.Phone).HasMaxLength(40);
        });

        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.HasIndex(item => new { item.MedicineId, item.BatchNumber }).IsUnique();
            entity.Property(item => item.BatchNumber).HasMaxLength(80);

            entity.HasOne(item => item.Medicine)
                .WithMany(medicine => medicine.InventoryItems)
                .HasForeignKey(item => item.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(item => item.Supplier)
                .WithMany(supplier => supplier.InventoryItems)
                .HasForeignKey(item => item.SupplierId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.Property(prescription => prescription.PatientName).HasMaxLength(160);
            entity.Property(prescription => prescription.DoctorName).HasMaxLength(160);
            entity.Property(prescription => prescription.Status).HasConversion<string>().HasMaxLength(32);

            entity.HasOne(prescription => prescription.PatientUser)
                .WithMany(user => user.Prescriptions)
                .HasForeignKey(prescription => prescription.PatientUserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<PrescriptionItem>(entity =>
        {
            entity.HasOne(item => item.Prescription)
                .WithMany(prescription => prescription.Items)
                .HasForeignKey(item => item.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(item => item.Medicine)
                .WithMany(medicine => medicine.PrescriptionItems)
                .HasForeignKey(item => item.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(order => order.CustomerName).HasMaxLength(160);
            entity.Property(order => order.Status).HasConversion<string>().HasMaxLength(32);
            entity.Property(order => order.TotalAmount).HasPrecision(18, 2);

            entity.HasOne(order => order.CustomerUser)
                .WithMany(user => user.Orders)
                .HasForeignKey(order => order.CustomerUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(order => order.Prescription)
                .WithMany()
                .HasForeignKey(order => order.PrescriptionId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.Property(item => item.UnitPrice).HasPrecision(18, 2);

            entity.HasOne(item => item.Order)
                .WithMany(order => order.Items)
                .HasForeignKey(item => item.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(item => item.Medicine)
                .WithMany(medicine => medicine.OrderItems)
                .HasForeignKey(item => item.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
