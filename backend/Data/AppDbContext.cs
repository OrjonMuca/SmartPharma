using Microsoft.EntityFrameworkCore;
using Backend_myMeds.Models;

namespace Backend_myMeds.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Medicine> Medicines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.HasKey(m => m.Id);
                
                entity.Property(m => m.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(m => m.Dosage)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.Property(m => m.Frequency)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(m => m.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                entity.Property(m => m.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
                // Create indexes
                entity.HasIndex(m => m.Name);
                entity.HasIndex(m => m.IsActive);
            });
        }
    }
}