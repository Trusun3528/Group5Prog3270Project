using Microsoft.VisualStudio.TestTools.UnitTesting;
using Group5.src.domain.models;
using Group5.src.infrastructure;
using Group5.src.Presentaion.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Group5.src.Presentaion.Tests
{
    // Helper extension methods for Mock<ISession>
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }

    [TestClass]
    public class UserCartControllerTests
    {
        private Group5DbContext _context;
        private Mock<ILogger<ProductController>> _loggerMock;
        private Mock<UserManager<User>> _userManagerMock;
        private UserCartController _controller;
        private Mock<ISession> _sessionMock;
        private Mock<HttpContext> _httpContextMock;


        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<Group5DbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new Group5DbContext(options);

            _context.Products.AddRange(
                new Product { Id = 1, ProductName = "Test Product 1", Price = 10.00, Stock = 10, CatagoryId = 1 },
                new Product { Id = 2, ProductName = "Test Product 2", Price = 20.00, Stock = 5, CatagoryId = 1 }
            );
            _context.Users.Add(new User { Id = "test-user-id", UserName = "testuser" });
            _context.SaveChanges();

            _loggerMock = new Mock<ILogger<ProductController>>();

            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                            .ReturnsAsync(_context.Users.First(u => u.Id == "test-user-id"));


            _sessionMock = new Mock<ISession>();
            _httpContextMock = new Mock<HttpContext>();
            _httpContextMock.Setup(ctx => ctx.Session).Returns(_sessionMock.Object);

            _controller = new UserCartController(_context, _loggerMock.Object, _userManagerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContextMock.Object
                }
            };

            var sessionStore = new Dictionary<string, byte[]>();
            _sessionMock.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                        .Callback<string, byte[]>((key, value) => sessionStore[key] = value);
            _sessionMock.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
                        .Callback(new TryGetValueCallback((string key, out byte[] value) =>
                        {
                            sessionStore.TryGetValue(key, out value);
                        }))
                        .Returns((string key, out byte[] value) => {
                            value = null; // Initialize the out parameter
                            return sessionStore.TryGetValue(key, out value);
                        });
             _sessionMock.Setup(s => s.Remove(It.IsAny<string>()))
                        .Callback<string>(key => sessionStore.Remove(key));

        }

        private delegate void TryGetValueCallback(string key, out byte[] value);


        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SetupSessionCart(Cart cart)
        {
            if (cart == null)
            {
                _sessionMock.Setup(s => s.TryGetValue("Cart", out It.Ref<byte[]>.IsAny)).Returns(false);
                return;
            }
            
            var cartJson = JsonConvert.SerializeObject(cart);
            var cartBytes = Encoding.UTF8.GetBytes(cartJson);
            
            var sessionStore = new Dictionary<string, byte[]>();
            sessionStore["Cart"] = cartBytes;
            
            _sessionMock.Setup(s => s.TryGetValue("Cart", out It.Ref<byte[]>.IsAny))
                .Callback(new TryGetValueCallback((string key, out byte[] value) =>
                {
                    sessionStore.TryGetValue(key, out value);
                }))
                .Returns((string key, out byte[] value) => {
                    return sessionStore.TryGetValue(key, out value);
                });
        }

        private Cart GetSessionCart()
        {
             if (_sessionMock.Object.TryGetValue("Cart", out byte[] cartBytes))
            {
                var cartJson = Encoding.UTF8.GetString(cartBytes);
                return JsonConvert.DeserializeObject<Cart>(cartJson);
            }
            return null;
        }


        [TestMethod]
        public async Task GetCart_ReturnsCartItems()
        {
            var cart = new Cart
            {
                Items = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 2 } }
            };
            SetupSessionCart(cart);

            var result = await _controller.GetCart();

            Assert.IsInstanceOfType(result.Value, typeof(List<CartItemResponse>));
            var cartItems = result.Value as List<CartItemResponse>;
            Assert.AreEqual(1, cartItems.Count);
            Assert.AreEqual(1, cartItems[0].Product.Id);
            Assert.AreEqual(2, cartItems[0].Quantity);
        }

        [TestMethod]
        public async Task GetCart_EmptyCart_ReturnsEmptyList()
        {
            var cart = new Cart { Items = new List<CartItem>() };
             SetupSessionCart(cart); 

            var result = await _controller.GetCart();

            Assert.IsInstanceOfType(result.Value, typeof(List<CartItemResponse>));
            var cartItems = result.Value as List<CartItemResponse>;
            Assert.AreEqual(0, cartItems.Count);
        }


        
        [TestMethod]
        public void AddItem_ExistingItem_UpdatesQuantity()
        {
             var existingItem = new CartItem { ProductId = 1, Quantity = 1 };
            var cart = new Cart { Items = new List<CartItem> { existingItem } };
            SetupSessionCart(cart);

            var itemToAdd = new CartItem { ProductId = 1, Quantity = 3 };

            var result = _controller.AddItem(itemToAdd);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
             var updatedCart = GetSessionCart();
            Assert.IsNotNull(updatedCart);
            Assert.AreEqual(1, updatedCart.Items.Count); 
            Assert.AreEqual(1, updatedCart.Items[0].ProductId);
            Assert.AreEqual(1, updatedCart.Items[0].Quantity);
        }


       
        [TestMethod]
        public void RemoveItem_NonExistentItem_ReturnsNotFound()
        {
            var cart = new Cart
            {
                Items = new List<CartItem> { new CartItem { ProductId = 1, Quantity = 2 } }
            };
            SetupSessionCart(cart);

            var result = _controller.RemoveItem(99); 

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
             var updatedCart = GetSessionCart(); 
            Assert.IsNotNull(updatedCart);
            Assert.AreEqual(1, updatedCart.Items.Count);
        }

       

        [TestMethod]
        public async Task Checkout_EmptyCart_ReturnsBadRequest()
        {
            var cart = new Cart { Items = new List<CartItem>() }; 
            SetupSessionCart(cart);

            var result = await _controller.Checkout();

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Cart is empty.", badRequestResult.Value);
        }

        [TestMethod]
        public async Task Checkout_ProductNotFound_ReturnsBadRequest()
        {
            var cart = new Cart
            {
                Items = new List<CartItem> { new CartItem { ProductId = 99, Quantity = 1 } } 
            };
            SetupSessionCart(cart);


            var result = await _controller.Checkout();

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


        [TestMethod]
        public async Task Checkout_InsufficientStock_ReturnsBadRequest()
        {
            var cart = new Cart
            {
                Items = new List<CartItem> { new CartItem { ProductId = 2, Quantity = 10 } } 
            };
            SetupSessionCart(cart);

            var result = await _controller.Checkout();


            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsTrue((badRequestResult.Value as string).Contains("Insufficient stock"));
        }
    }
}
