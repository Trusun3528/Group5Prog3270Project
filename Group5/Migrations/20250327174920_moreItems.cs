using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Group5.Migrations
{
    /// <inheritdoc />
    public partial class moreItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    customerName = table.Column<string>(type: "TEXT", nullable: true),
                    city = table.Column<string>(type: "TEXT", nullable: true),
                    street = table.Column<string>(type: "TEXT", nullable: true),
                    state = table.Column<string>(type: "TEXT", nullable: true),
                    postalCode = table.Column<string>(type: "TEXT", nullable: true),
                    country = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Price = table.Column<double>(type: "decimal(18,2)", nullable: true),
                    ProductDescription = table.Column<string>(type: "TEXT", maxLength: 400, nullable: true),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false),
                    CatagoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageURL = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Rating = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RatingNumber = table.Column<double>(type: "REAL", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    addressId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreditCardNumber = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CVV = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingAddress = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CartID = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductID = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartID",
                        column: x => x.CartID,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Auto", "Auto Parts" },
                    { 2, "Kitchen", "Kitchen Stuff" },
                    { 3, "Electronics", "Devices" },
                    { 4, "Books", "Books!" },
                    { 5, "Furniture", "Furniture Stuff" },
                    { 6, "Clothing", "Clothing Stuff" },
                    { 7, "Sports", "Sporting Goods" },
                    { 8, "Toys", "Toys and Games" },
                    { 9, "Beauty", "Beauty Products" },
                    { 10, "Health", "Health Products" },
                    { 11, "Groceries", "Food and Beverages" },
                    { 12, "Garden", "Gardening Tools" },
                    { 13, "Pets", "Pet Products" },
                    { 14, "Office", "Office Supplies" },
                    { 15, "Travel", "Travel Gear" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CatagoryId", "ImageURL", "Price", "ProductDescription", "ProductName", "Rating", "Stock" },
                values: new object[,]
                {
                    { 1, 1, "https://i5.walmartimages.com/seo/Goodyear-Reliant-All-Season-225-55R18-98V-All-Season-Tire_97e6df10-5771-4701-a040-6b6b015b3773.400874d938bd6a36e3391979ef30825f.jpeg", 119.98999999999999, "Its a tire", "Left Tire", 0.0, 100 },
                    { 2, 2, "https://images.homedepot.ca/productimages/p_1000514524.jpg?product-images=l", 29.989999999999998, "Plunge your sink", "Sink Plunger", 0.0, 50 },
                    { 3, 10, "https://m.media-amazon.com/images/I/81en3yalOyL.jpg", 19.989999999999998, "This waterproof towel never needs washing!", "Waterproof Towel", 0.0, 18 },
                    { 4, 12, "https://m.media-amazon.com/images/I/71JxuOqyiYL.jpg", 14.99, "A fantastic left-handed only hammer!", "Left Handed Hammer", 0.0, 2 },
                    { 5, 2, "https://images-cdn.ubuy.co.in/652914fc6922273b68176c08-voysign-charcoal-bbq-grill-barrel-bbq.jpg", 189.99000000000001, "Enjoy some delicious food with a truly tropical twist!", "Underwater BBQ", 0.0, 17 },
                    { 6, 2, "https://img.apmcdn.org/514b0eec7310ebc40d46d8f08b0ec4836f0398a2/square/9b4aab-20240226-matches-in-a-box-2000.jpg", 3.9900000000000002, "Perfect for the saftey conscientious!", "Fireproof Matches", 0.0, 42 },
                    { 7, 1, "https://atlas-content-cdn.pixelsquid.com/stock-images/led-light-bulb-lightbulb-ENAno48-600.jpg", 14.99, "This incredible lightbulb can brighten any room!", "Glow-In-The-Dark™ Lightbulb", 0.0, 32 },
                    { 8, 1, "https://m.media-amazon.com/images/I/41O3lGVtjML.jpg", 7.9900000000000002, "Never have to bother tying your laces again!", "Bluetooth Shoelaces", 0.0, 24 },
                    { 9, 1, "https://media.istockphoto.com/id/467479468/photo/car-wheel.jpg?s=612x612&w=0&k=20&c=FVAl5bqn5DJAgEOQtt8Ca3Mb9Dzk0BqwTJ3SiQ3L3ts=", 114.98999999999999, "Its a square tire", "Square Tire", 0.0, 13 },
                    { 10, 1, "https://workstuff.co.in/wp-content/uploads/2020/10/Workstuff_Office_Supplies_Office_Basics_Amos_Glue_Stick_15_Grams600x600.jpg", 4.9900000000000002, "Never get sticky fingers again!", "Non-Stick Glue", 0.0, 12 },
                    { 11, 1, "https://imgs.michaels.com/MAM/assets/1/4498F4AD976B45D3A3FA77D38829F622/img/FB0B4E000A29494A8948C957B0F4CA20/D207038S_1.jpg", 99.989999999999995, "Find the comfort of a warm fireplace anywhere!", "Inflatable Fireplace", 0.0, 100 },
                    { 12, 1, "https://urbanwoodcraft.com/wp-content/uploads/2023/07/CELINE-83%E2%80%B3-X-40%E2%80%B3-OVAL-MIRROR-BARN-DOOR.jpg", 49.990000000000002, "Great to see beyond the physical", "See-through Mirror", 0.0, 100 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserName", "addressId" },
                values: new object[,]
                {
                    { 1, "acameron1391@conestogac.on.ca", "$2a$11$5skyn5sF5DfIjtt8DLK/nuOR7r.OKjSn9mGDkBJyrzvBaE5C4Rjf2", "Admin", "Austin", 0 },
                    { 2, "Patrick@google.com", "$2a$11$5skyn5sF5DfIjtt8DLK/nuOR7r.OKjSn9mGDkBJyrzvBaE5C4Rjf2", "User", "Patrick", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserId",
                table: "Cards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartID",
                table: "CartItems",
                column: "CartID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductID",
                table: "CartItems",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
