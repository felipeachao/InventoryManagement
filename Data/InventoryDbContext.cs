using Microsoft.EntityFrameworkCore;
using InventoryManagement.Data.Entities;

namespace InventoryManagement.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ConsumptionLog> ConsumptionLogs { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PartNumber).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CostPrice).IsRequired();
                entity.Property(e => e.StockQuantity).IsRequired();
            });

            modelBuilder.Entity<ConsumptionLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.QuantityConsumed).IsRequired();
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.TotalCost).IsRequired();

                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId);
            });

            modelBuilder.Entity<ErrorLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.StackTrace).IsRequired();
                entity.Property(e => e.Date).IsRequired();
            });
        }
    }
}
