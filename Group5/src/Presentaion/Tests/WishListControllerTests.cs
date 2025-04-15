using Group5.src.domain.models;
using Group5.src.infrastructure;
using Group5.src.Presentaion.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Security.Claims;

namespace Group5.src.Presentaion.Tests
{
    [TestClass]
    public class WishListControllerTests
    {
        private Group5DbContext _context;
        private WishListController _controller;
        private Mock<UserManager<User>> _userManagerMock;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Group5DbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new Group5DbContext(options);

            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            var testUser = new User { Id = "test-user-id", UserName = "testuser" };
            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(testUser);

            _context.Users.Add(testUser);
            _context.SaveChanges();

            _controller = new WishListController(_context, _userManagerMock.Object);
        }

        [TestMethod]
        public async Task AddToWishlist_ValidProduct_ReturnsOk()
        {
            // Arrange
            var product = new Product { Id = 1, ProductName = "Test Product" };
            _context.Products.Add(product);
            _context.SaveChanges();

            // Act
            var result = await _controller.AddToWishlist(product.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public async Task GetWishlist_ReturnsWishlistItems()
        {
            // Arrange
            var product = new Product { Id = 1, ProductName = "Test Product" };
            _context.Products.Add(product);
            _context.WishListItems.Add(new WishListItem { UserId = "test-user-id", ProductId = product.Id, Product = product });
            _context.SaveChanges();

            // Act
            var result = await _controller.GetWishlist();

            // Assert
            var actionResult = result as ActionResult<IEnumerable<Product>>;
            Assert.IsNotNull(actionResult);
            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var products = okResult.Value as IEnumerable<Product>;
            Assert.IsNotNull(products);
            Assert.AreEqual(1, products.Count());
            Assert.AreEqual("Test Product", products.First().ProductName);
        }

        [TestMethod]
        public async Task DeleteFromWishlist_ValidProduct_RemovesItem()
        {
            // Arrange
            var product = new Product { Id = 1, ProductName = "Test Product" };
            _context.Products.Add(product);
            _context.WishListItems.Add(new WishListItem { UserId = "test-user-id", ProductId = product.Id });
            _context.SaveChanges();

            // Act
            var result = await _controller.DeleteFromWishlist(product.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var wishlistItem = _context.WishListItems.FirstOrDefault(w => w.UserId == "test-user-id" && w.ProductId == product.Id);
            Assert.IsNull(wishlistItem);
        }
    }
}
