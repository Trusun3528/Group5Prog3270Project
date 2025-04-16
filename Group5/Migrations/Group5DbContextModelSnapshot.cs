﻿// <auto-generated />
using System;
using Group5.src.infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Group5.Migrations
{
    [DbContext(typeof(Group5DbContext))]
    partial class Group5DbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("Group5.src.domain.models.Address", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("city")
                        .HasColumnType("TEXT");

                    b.Property<string>("country")
                        .HasColumnType("TEXT");

                    b.Property<string>("customerName")
                        .HasColumnType("TEXT");

                    b.Property<string>("postalCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("state")
                        .HasColumnType("TEXT");

                    b.Property<string>("street")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Group5.src.domain.models.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CVV")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("TEXT");

                    b.Property<string>("CreditCardNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("GuestId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GuestId");

                    b.HasIndex("UserId");

                    b.ToTable("Cards");
                });

<<<<<<< HEAD
=======
            modelBuilder.Entity("Group5.src.domain.models.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GuestId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GuestId");

                    b.HasIndex("UserId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("Group5.src.domain.models.CartItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CartID")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CartID");

                    b.HasIndex("ProductID");

                    b.ToTable("CartItems");
                });

>>>>>>> 024f501a53082d9495e001eaf51a61d86cba5bfa
            modelBuilder.Entity("Group5.src.domain.models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryName = "Auto",
                            Description = "Auto Parts"
                        },
                        new
                        {
                            Id = 2,
                            CategoryName = "Kitchen",
                            Description = "Kitchen Stuff"
                        },
                        new
                        {
                            Id = 3,
                            CategoryName = "Electronics",
                            Description = "Devices"
                        },
                        new
                        {
                            Id = 4,
                            CategoryName = "Books",
                            Description = "Books!"
                        },
                        new
                        {
                            Id = 5,
                            CategoryName = "Furniture",
                            Description = "Furniture Stuff"
                        },
                        new
                        {
                            Id = 6,
                            CategoryName = "Clothing",
                            Description = "Clothing Stuff"
                        },
                        new
                        {
                            Id = 7,
                            CategoryName = "Sports",
                            Description = "Sporting Goods"
                        },
                        new
                        {
                            Id = 8,
                            CategoryName = "Toys",
                            Description = "Toys and Games"
                        },
                        new
                        {
                            Id = 9,
                            CategoryName = "Beauty",
                            Description = "Beauty Products"
                        },
                        new
                        {
                            Id = 10,
                            CategoryName = "Health",
                            Description = "Health Products"
                        },
                        new
                        {
                            Id = 11,
                            CategoryName = "Groceries",
                            Description = "Food and Beverages"
                        },
                        new
                        {
                            Id = 12,
                            CategoryName = "Garden",
                            Description = "Gardening Tools"
                        },
                        new
                        {
                            Id = 13,
                            CategoryName = "Pets",
                            Description = "Pet Products"
                        },
                        new
                        {
                            Id = 14,
                            CategoryName = "Office",
                            Description = "Office Supplies"
                        },
                        new
                        {
                            Id = 15,
                            CategoryName = "Travel",
                            Description = "Travel Gear"
                        });
                });

            modelBuilder.Entity("Group5.src.domain.models.Guest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("addressId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Guest");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Role = "Guest",
                            addressId = 0
                        });
                });

            modelBuilder.Entity("Group5.src.domain.models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GuestId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("ShippingAddress")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GuestId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Group5.src.domain.models.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Group5.src.domain.models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CatagoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageURL")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<double?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProductDescription")
                        .HasMaxLength(400)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<double>("Rating")
                        .HasColumnType("REAL");

                    b.Property<int>("Stock")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CatagoryId = 1,
                            ImageURL = "https://i5.walmartimages.com/seo/Goodyear-Reliant-All-Season-225-55R18-98V-All-Season-Tire_97e6df10-5771-4701-a040-6b6b015b3773.400874d938bd6a36e3391979ef30825f.jpeg",
                            Price = 119.98999999999999,
                            ProductDescription = "Its a tire",
                            ProductName = "Left Tire",
                            Rating = 0.0,
                            Stock = 100
                        },
                        new
                        {
                            Id = 2,
                            CatagoryId = 2,
                            ImageURL = "https://images.homedepot.ca/productimages/p_1000514524.jpg?product-images=l",
                            Price = 29.989999999999998,
                            ProductDescription = "Plunge your sink",
                            ProductName = "Sink Plunger",
                            Rating = 0.0,
                            Stock = 50
                        },
                        new
                        {
                            Id = 3,
                            CatagoryId = 10,
                            ImageURL = "https://m.media-amazon.com/images/I/81en3yalOyL.jpg",
                            Price = 19.989999999999998,
                            ProductDescription = "This waterproof towel never needs washing!",
                            ProductName = "Waterproof Towel",
                            Rating = 0.0,
                            Stock = 18
                        },
                        new
                        {
                            Id = 4,
                            CatagoryId = 12,
                            ImageURL = "https://m.media-amazon.com/images/I/71JxuOqyiYL.jpg",
                            Price = 14.99,
                            ProductDescription = "A fantastic left-handed only hammer!",
                            ProductName = "Left Handed Hammer",
                            Rating = 0.0,
                            Stock = 2
                        },
                        new
                        {
                            Id = 5,
                            CatagoryId = 2,
                            ImageURL = "https://images-cdn.ubuy.co.in/652914fc6922273b68176c08-voysign-charcoal-bbq-grill-barrel-bbq.jpg",
                            Price = 189.99000000000001,
                            ProductDescription = "Enjoy some delicious food with a truly tropical twist!",
                            ProductName = "Underwater BBQ",
                            Rating = 0.0,
                            Stock = 17
                        },
                        new
                        {
                            Id = 6,
                            CatagoryId = 2,
                            ImageURL = "https://img.apmcdn.org/514b0eec7310ebc40d46d8f08b0ec4836f0398a2/square/9b4aab-20240226-matches-in-a-box-2000.jpg",
                            Price = 3.9900000000000002,
                            ProductDescription = "Perfect for the saftey conscientious!",
                            ProductName = "Fireproof Matches",
                            Rating = 0.0,
                            Stock = 42
                        },
                        new
                        {
                            Id = 7,
                            CatagoryId = 1,
                            ImageURL = "https://atlas-content-cdn.pixelsquid.com/stock-images/led-light-bulb-lightbulb-ENAno48-600.jpg",
                            Price = 14.99,
                            ProductDescription = "This incredible lightbulb can brighten any room!",
                            ProductName = "Glow-In-The-Dark™ Lightbulb",
                            Rating = 0.0,
                            Stock = 32
                        },
                        new
                        {
                            Id = 8,
                            CatagoryId = 1,
                            ImageURL = "https://m.media-amazon.com/images/I/41O3lGVtjML.jpg",
                            Price = 7.9900000000000002,
                            ProductDescription = "Never have to bother tying your laces again!",
                            ProductName = "Bluetooth Shoelaces",
                            Rating = 0.0,
                            Stock = 24
                        },
                        new
                        {
                            Id = 9,
                            CatagoryId = 1,
                            ImageURL = "https://media.istockphoto.com/id/467479468/photo/car-wheel.jpg?s=612x612&w=0&k=20&c=FVAl5bqn5DJAgEOQtt8Ca3Mb9Dzk0BqwTJ3SiQ3L3ts=",
                            Price = 114.98999999999999,
                            ProductDescription = "Its a square tire",
                            ProductName = "Square Tire",
                            Rating = 0.0,
                            Stock = 13
                        },
                        new
                        {
                            Id = 10,
                            CatagoryId = 1,
                            ImageURL = "https://workstuff.co.in/wp-content/uploads/2020/10/Workstuff_Office_Supplies_Office_Basics_Amos_Glue_Stick_15_Grams600x600.jpg",
                            Price = 4.9900000000000002,
                            ProductDescription = "Never get sticky fingers again!",
                            ProductName = "Non-Stick Glue",
                            Rating = 0.0,
                            Stock = 12
                        },
                        new
                        {
                            Id = 11,
                            CatagoryId = 1,
                            ImageURL = "https://imgs.michaels.com/MAM/assets/1/4498F4AD976B45D3A3FA77D38829F622/img/FB0B4E000A29494A8948C957B0F4CA20/D207038S_1.jpg",
                            Price = 99.989999999999995,
                            ProductDescription = "Find the comfort of a warm fireplace anywhere!",
                            ProductName = "Inflatable Fireplace",
                            Rating = 0.0,
                            Stock = 100
                        },
                        new
                        {
                            Id = 12,
                            CatagoryId = 1,
                            ImageURL = "https://urbanwoodcraft.com/wp-content/uploads/2023/07/CELINE-83%E2%80%B3-X-40%E2%80%B3-OVAL-MIRROR-BARN-DOOR.jpg",
                            Price = 49.990000000000002,
                            ProductDescription = "Great to see beyond the physical",
                            ProductName = "See-through Mirror",
                            Rating = 0.0,
                            Stock = 100
                        });
                });

            modelBuilder.Entity("Group5.src.domain.models.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("RatingNumber")
                        .HasColumnType("REAL");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Group5.src.domain.models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Group5.src.domain.models.WishListItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("WishListItems");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Group5.src.domain.models.Card", b =>
                {
                    b.HasOne("Group5.src.domain.models.Guest", null)
                        .WithMany("Cards")
                        .HasForeignKey("GuestId");

                    b.HasOne("Group5.src.domain.models.User", null)
                        .WithMany("Cards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

<<<<<<< HEAD
=======
            modelBuilder.Entity("Group5.src.domain.models.Cart", b =>
                {
                    b.HasOne("Group5.src.domain.models.Guest", null)
                        .WithMany("Carts")
                        .HasForeignKey("GuestId");

                    b.HasOne("Group5.src.domain.models.User", "User")
                        .WithMany("Carts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Group5.src.domain.models.CartItem", b =>
                {
                    b.HasOne("Group5.src.domain.models.Cart", "Cart")
                        .WithMany("CartItems")
                        .HasForeignKey("CartID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Group5.src.domain.models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Product");
                });

>>>>>>> 024f501a53082d9495e001eaf51a61d86cba5bfa
            modelBuilder.Entity("Group5.src.domain.models.Order", b =>
                {
                    b.HasOne("Group5.src.domain.models.Guest", null)
                        .WithMany("Orders")
                        .HasForeignKey("GuestId");

                    b.HasOne("Group5.src.domain.models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Group5.src.domain.models.OrderItem", b =>
                {
                    b.HasOne("Group5.src.domain.models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Group5.src.domain.models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Group5.src.domain.models.WishListItem", b =>
                {
                    b.HasOne("Group5.src.domain.models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Group5.src.domain.models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Group5.src.domain.models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Group5.src.domain.models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Group5.src.domain.models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

<<<<<<< HEAD
=======
            modelBuilder.Entity("Group5.src.domain.models.Cart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("Group5.src.domain.models.Guest", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("Carts");

                    b.Navigation("Orders");
                });

>>>>>>> 024f501a53082d9495e001eaf51a61d86cba5bfa
            modelBuilder.Entity("Group5.src.domain.models.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Group5.src.domain.models.User", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
