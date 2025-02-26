using Microsoft.EntityFrameworkCore;
using Group5.src.domain.models; 

namespace Group5.src.infrastructure
{
    public class Group5DbContext : DbContext
    {
        public Group5DbContext(DbContextOptions<Group5DbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    ProductName = "Left Tire",
                    Price = 119.99,
                    ProductDescription = "Its a tire",
                    Stock = 100,
                    Catagory = "Car",
                    ImageURL = "https://cool.com"
                },
                new Product
                {
                    Id = 2,
                    ProductName = "Sink Plunger",
                    Price = 29.99,
                    ProductDescription = "Plunge your sink",
                    Stock = 50,
                    Catagory = "Kitchen",
                    ImageURL = "https://cool.com"
                });
        }
    }
}