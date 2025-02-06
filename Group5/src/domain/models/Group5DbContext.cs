using Microsoft.EntityFrameworkCore;
using Group5.src.domain.models; // Adjust the namespace as necessary

namespace Group5.src.infrastructure
{
    public class Group5DbContext : DbContext
    {
        public Group5DbContext(DbContextOptions<Group5DbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } // Example entity
        // Add other DbSet properties for your entities here

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure your entity mappings here if needed

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id); // Explicit key configuration
                entity.Property(p => p.ProductName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(p => p.Price)
                    .HasColumnType("decimal(18,2)"); // Configures SQL decimal type
                entity.Property(p => p.ProductDescription)
                    .HasMaxLength(500);
            });
        }
    }
}