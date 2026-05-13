using Microsoft.EntityFrameworkCore;
using smartPharmaAPI.Models;
using smartPharmaAPI.Services;

namespace smartPharmaAPI.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<SmartPharmaDbContext>();
        var passwordService = scope.ServiceProvider.GetRequiredService<PasswordService>();

        await db.Database.EnsureCreatedAsync();

        if (!await db.Users.AnyAsync())
        {
            db.Users.AddRange(
                new AppUser
                {
                    FullName = "System Admin",
                    Email = "admin@smartpharma.local",
                    PasswordHash = passwordService.Hash("Admin123!"),
                    Role = UserRole.Admin
                },
                new AppUser
                {
                    FullName = "Default Pharmacist",
                    Email = "pharmacist@smartpharma.local",
                    PasswordHash = passwordService.Hash("Pharmacist123!"),
                    Role = UserRole.Pharmacist
                });
        }

        if (!await db.Suppliers.AnyAsync())
        {
            db.Suppliers.AddRange(
                new Supplier
                {
                    Name = "HealthSource Wholesale",
                    ContactPerson = "Elira Duka",
                    Email = "orders@healthsource.local",
                    Phone = "+355 69 100 0001",
                    Address = "Tirana"
                },
                new Supplier
                {
                    Name = "MediSupply Partners",
                    ContactPerson = "Arben Kola",
                    Email = "support@medisupply.local",
                    Phone = "+355 69 100 0002",
                    Address = "Durres"
                });
        }

        if (!await db.Medicines.AnyAsync())
        {
            db.Medicines.AddRange(
                new Medicine
                {
                    Name = "Paracetamol 500mg",
                    GenericName = "Acetaminophen",
                    Manufacturer = "Balkan Pharma",
                    DosageForm = "Tablet",
                    Strength = "500mg",
                    Category = "Pain Relief",
                    Description = "Used for mild pain and fever.",
                    Price = 2.50m,
                    RequiresPrescription = false
                },
                new Medicine
                {
                    Name = "Ibuprofen 400mg",
                    GenericName = "Ibuprofen",
                    Manufacturer = "AdriaMed",
                    DosageForm = "Tablet",
                    Strength = "400mg",
                    Category = "Anti-inflammatory",
                    Description = "Non-steroidal anti-inflammatory medicine.",
                    Price = 3.20m,
                    RequiresPrescription = false
                },
                new Medicine
                {
                    Name = "Amoxicillin 500mg",
                    GenericName = "Amoxicillin",
                    Manufacturer = "EuroGenerics",
                    DosageForm = "Capsule",
                    Strength = "500mg",
                    Category = "Antibiotic",
                    Description = "Antibiotic used for bacterial infections.",
                    Price = 6.80m,
                    RequiresPrescription = true
                });
        }

        await db.SaveChangesAsync();

        if (!await db.InventoryItems.AnyAsync())
        {
            var supplier = await db.Suppliers.OrderBy(supplier => supplier.Name).FirstAsync();
            var medicines = await db.Medicines.OrderBy(medicine => medicine.Name).ToListAsync();
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            db.InventoryItems.AddRange(medicines.Select((medicine, index) => new InventoryItem
            {
                MedicineId = medicine.Id,
                SupplierId = supplier.Id,
                BatchNumber = $"BATCH-{index + 1:000}",
                Quantity = medicine.RequiresPrescription ? 40 : 120,
                ReorderLevel = medicine.RequiresPrescription ? 10 : 25,
                ExpirationDate = today.AddMonths(18 + index)
            }));

            await db.SaveChangesAsync();
        }
    }
}
