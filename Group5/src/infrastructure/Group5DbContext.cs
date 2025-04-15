using Microsoft.EntityFrameworkCore;
using Group5.src.domain.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Group5.src.infrastructure
{
    public class Group5DbContext : IdentityDbContext<User>
    {
        public Group5DbContext(DbContextOptions<Group5DbContext> options)
            : base(options)
        {
        }
        public Group5DbContext()
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<WishListItem> WishListItems { get; set; }


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
                    CatagoryId = 1,
                    ImageURL = "https://i5.walmartimages.com/seo/Goodyear-Reliant-All-Season-225-55R18-98V-All-Season-Tire_97e6df10-5771-4701-a040-6b6b015b3773.400874d938bd6a36e3391979ef30825f.jpeg"
                },
                new Product
                {
                    Id = 2,
                    ProductName = "Sink Plunger",
                    Price = 29.99,
                    ProductDescription = "Plunge your sink",
                    Stock = 50,
                    CatagoryId = 2,
                    ImageURL = "https://images.homedepot.ca/productimages/p_1000514524.jpg?product-images=l"
                },
                new Product
                {
                    Id = 3,
                    ProductName = "Waterproof Towel",
                    Price = 19.99,
                    ProductDescription = "This waterproof towel never needs washing!",
                    Stock = 18,
                    CatagoryId = 10,
                    ImageURL = "https://m.media-amazon.com/images/I/81en3yalOyL.jpg"
                }, new Product
                {
                    Id = 4,
                    ProductName = "Left Handed Hammer",
                    Price = 14.99,
                    ProductDescription = "A fantastic left-handed only hammer!",
                    Stock = 2,
                    CatagoryId = 12,
                    ImageURL = "https://m.media-amazon.com/images/I/71JxuOqyiYL.jpg"
                }, new Product
                {
                    Id = 5,
                    ProductName = "Underwater BBQ",
                    Price = 189.99,
                    ProductDescription = "Enjoy some delicious food with a truly tropical twist!",
                    Stock = 17,
                    CatagoryId = 2,
                    ImageURL = "https://images-cdn.ubuy.co.in/652914fc6922273b68176c08-voysign-charcoal-bbq-grill-barrel-bbq.jpg"
                }, new Product
                {
                    Id = 6,
                    ProductName = "Fireproof Matches",
                    Price = 3.99,
                    ProductDescription = "Perfect for the saftey conscientious!",
                    Stock = 42,
                    CatagoryId = 2,
                    ImageURL = "https://img.apmcdn.org/514b0eec7310ebc40d46d8f08b0ec4836f0398a2/square/9b4aab-20240226-matches-in-a-box-2000.jpg"
                }, new Product
                {
                    Id = 7,
                    ProductName = "Glow-In-The-Dark™ Lightbulb",
                    Price = 14.99,
                    ProductDescription = "This incredible lightbulb can brighten any room!",
                    Stock = 32,
                    CatagoryId = 1,
                    ImageURL = "https://atlas-content-cdn.pixelsquid.com/stock-images/led-light-bulb-lightbulb-ENAno48-600.jpg"
                }, new Product
                {
                    Id = 8,
                    ProductName = "Bluetooth Shoelaces",
                    Price = 7.99,
                    ProductDescription = "Never have to bother tying your laces again!",
                    Stock = 24,
                    CatagoryId = 1,
                    ImageURL = "https://m.media-amazon.com/images/I/41O3lGVtjML.jpg"
                }, new Product
                {
                    Id = 9,
                    ProductName = "Square Tire",
                    Price = 114.99,
                    ProductDescription = "Its a square tire",
                    Stock = 13,
                    CatagoryId = 1,
                    ImageURL = "https://media.istockphoto.com/id/467479468/photo/car-wheel.jpg?s=612x612&w=0&k=20&c=FVAl5bqn5DJAgEOQtt8Ca3Mb9Dzk0BqwTJ3SiQ3L3ts="
                }, new Product
                {
                    Id = 10,
                    ProductName = "Non-Stick Glue",
                    Price = 4.99,
                    ProductDescription = "Never get sticky fingers again!",
                    Stock = 12,
                    CatagoryId = 1,
                    ImageURL = "https://workstuff.co.in/wp-content/uploads/2020/10/Workstuff_Office_Supplies_Office_Basics_Amos_Glue_Stick_15_Grams600x600.jpg"
                }, new Product
                {
                    Id = 11,
                    ProductName = "Inflatable Fireplace",
                    Price = 99.99,
                    ProductDescription = "Find the comfort of a warm fireplace anywhere!",
                    Stock = 100,
                    CatagoryId = 1,
                    ImageURL = "https://imgs.michaels.com/MAM/assets/1/4498F4AD976B45D3A3FA77D38829F622/img/FB0B4E000A29494A8948C957B0F4CA20/D207038S_1.jpg"
                }, new Product
                {
                    Id = 12,
                    ProductName = "See-through Mirror",
                    Price = 49.99,
                    ProductDescription = "Great to see beyond the physical",
                    Stock = 100,
                    CatagoryId = 1,
                    ImageURL = "https://urbanwoodcraft.com/wp-content/uploads/2023/07/CELINE-83%E2%80%B3-X-40%E2%80%B3-OVAL-MIRROR-BARN-DOOR.jpg"
                });

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    CategoryName = "Auto",
                    Description = "Auto Parts"
                },
                new Category
                {
                    Id = 2,
                    CategoryName = "Kitchen",
                    Description = "Kitchen Stuff"
                },
                new Category
                {
                    Id = 3,
                    CategoryName = "Electronics",
                    Description = "Devices"
                },
                new Category
                {
                    Id = 4,
                    CategoryName = "Books",
                    Description = "Books!"
                },
                new Category
                {
                    Id = 5,
                    CategoryName = "Furniture",
                    Description = "Furniture Stuff"
                },
                new Category
                {
                    Id = 6,
                    CategoryName = "Clothing",
                    Description = "Clothing Stuff"
                },
                new Category
                {
                    Id = 7,
                    CategoryName = "Sports",
                    Description = "Sporting Goods"
                },
                new Category
                {
                    Id = 8,
                    CategoryName = "Toys",
                    Description = "Toys and Games"
                },
                new Category
                {
                    Id = 9,
                    CategoryName = "Beauty",
                    Description = "Beauty Products"
                },
                new Category
                {
                    Id = 10,
                    CategoryName = "Health",
                    Description = "Health Products"
                },
                new Category
                {
                    Id = 11,
                    CategoryName = "Groceries",
                    Description = "Food and Beverages"
                },
                new Category
                {
                    Id = 12,
                    CategoryName = "Garden",
                    Description = "Gardening Tools"
                },
                new Category
                {
                    Id = 13,
                    CategoryName = "Pets",
                    Description = "Pet Products"
                },
                new Category
                {
                    Id = 14,
                    CategoryName = "Office",
                    Description = "Office Supplies"
                },
                new Category
                {
                    Id = 15,
                    CategoryName = "Travel",
                    Description = "Travel Gear"
                }
            );

        }
        public virtual DbSet<Card> GetCards() => Cards;
    }
}