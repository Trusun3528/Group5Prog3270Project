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
        public Group5DbContext()
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(//populating some products
                new Product
                {
                    Id = 1,
                    ProductName = "Left Tire",
                    Price = 119.99,
                    ProductDescription = "Its a tire",
                    Stock = 100,
                    Catagory = "Car",
                    ImageURL = "https://i5.walmartimages.com/seo/Goodyear-Reliant-All-Season-225-55R18-98V-All-Season-Tire_97e6df10-5771-4701-a040-6b6b015b3773.400874d938bd6a36e3391979ef30825f.jpeg"
                },
                new Product
                {
                    Id = 2,
                    ProductName = "Sink Plunger",
                    Price = 29.99,
                    ProductDescription = "Plunge your sink",
                    Stock = 50,
                    Catagory = "Kitchen",
                    ImageURL = "https://images.homedepot.ca/productimages/p_1000514524.jpg?product-images=l"
                });

            modelBuilder.Entity<User>().HasData(//populating a user account and a admin account
                new User
                {
                    Id = 1,
                    UserName = "Austin",
                    Email = "acameron1391@conestogac.on.ca",
                    Password = "$2a$11$5skyn5sF5DfIjtt8DLK/nuOR7r.OKjSn9mGDkBJyrzvBaE5C4Rjf2",
                    Role = "Admin"
                },
                new User
                {
                    Id = 2,
                    UserName = "Patrick",
                    Email = "Patrick@google.com",
                    Password = "$2a$11$5skyn5sF5DfIjtt8DLK/nuOR7r.OKjSn9mGDkBJyrzvBaE5C4Rjf2",
                    Role = "User"
                });
        }
        public virtual DbSet<Card> GetCards() => Cards;
    }
}