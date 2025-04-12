namespace Group5.src.Presentaion.Tests;
using Group5.src.domain.models;
using Group5.src.infrastructure;
using Group5.src.Presentaion.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestClass]
public class CartControllerTests
{
    private Group5DbContext _context;
    private AdminCartController _controller;
    private ILogger<ProductController> _logger;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<Group5DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new Group5DbContext(options);
        _logger = new LoggerFactory().CreateLogger<ProductController>();
        _controller = new AdminCartController(_context, _logger);

        
        _context.Carts.Add(new Cart { Id = 1, UserId = "1" });
        _context.Carts.Add(new Cart { Id = 2, UserId = "2" });
        _context.Carts.AddRange(
            new Cart { Id = 3, UserId = "1", TotalAmount = 100 },
            new Cart { Id = 4, UserId = "2", TotalAmount = 200 }
        );
        _context.SaveChanges();
    }

    [TestMethod]
    public async Task GetCart_ValidId_ReturnsOk()
    {
        var result = await _controller.GetCart(1);
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetCart_InvalidId_ReturnsNotFound()
    {
        var result = await _controller.GetCart(99);
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task AddCart_ValidCart_ReturnsOk()
    {
       
        var newCart = new Cart
        {
            Id = 3,
            UserId = "3",
            TotalAmount = 100.00m
        };

        
        var result = await _controller.AddCart(newCart);

        
        Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
    }

    [TestMethod]
    public void Index_ReturnsViewResult()
    {
        var result = _controller.Index();
        Assert.IsInstanceOfType(result, typeof(ViewResult));
    }

    [TestMethod]
    public async Task GetCarts_ReturnsOk()
    {
        // Act
        var result = await _controller.GetCarts();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var carts = okResult.Value as IEnumerable<Cart>;
        Assert.IsNotNull(carts);
        Assert.IsTrue(carts.Any());
    }

    [TestMethod]
    public async Task DeleteCart_ValidId_ReturnsOk()
    {
        var result = await _controller.DeleteCart(1);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task DeleteCart_InvalidId_ReturnsNotFound()
    {
        var result = await _controller.DeleteCart(99);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task AddCartItem_ValidCartItem_ReturnsOk()
    {
        var newCartItem = new CartItem
        {
            Id = 1,
            CartID = 1,
            ProductID = 1,
            Quantity = 1,
            Price = 10.00
        };

        var result = await _controller.AddCartItem(newCartItem);
        Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
    }

    [TestMethod]
    public async Task GetCartItems_ValidId_ReturnsOk()
    {
        
        var cartItem1 = new CartItem { Id = 1, CartID = 1, ProductID = 1, Quantity = 1, Price = 10.0 };
        var cartItem2 = new CartItem { Id = 2, CartID = 1, ProductID = 2, Quantity = 2, Price = 20.0 };

        
        _context.CartItems.AddRange(cartItem1, cartItem2);
        await _context.SaveChangesAsync();

        
        var result = await _controller.GetCartItems(1);

       
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetCartItems_InvalidId_ReturnsNotFound()
    {
        var result = await _controller.GetCartItems(99);
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task GetCarts_WithTwoIds_ValidIds_ReturnsOk()
    {
        var result = await _controller.GetCarts(1, 1);
        Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
    }

    [TestMethod]
    public async Task GetCarts_WithTwoIds_InvalidIds_ReturnsNotFound()
    {
        var result = await _controller.GetCarts(99, 99);
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task DeleteCartItem_ValidId_ReturnsOk()
    {
        var result = await _controller.DeleteCartItem(1);
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
    }

    [TestMethod]
    public async Task DeleteCartItem_InvalidId_ReturnsNotFound()
    {
        var result = await _controller.DeleteCartItem(99);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task EditCartItem_ValidId_ReturnsOk()
    {
        // Arrange: Add a product to the database
        var product = new Product
        {
            Id = 1,
            ProductName = "Test Product",
            Price = 10.00,
            Stock = 100,
            CatagoryId = 1
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Arrange: Add a cart item to the database
        var cartItem = new CartItem
        {
            Id = 99,
            CartID = 1,
            ProductID = 1,
            Quantity = 1,
            Price = 10.00
        };
        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();

        // Act: Edit the cart item
        var editedCartItem = new CartItem
        {
            Id = 99,
            CartID = 1,
            ProductID = 1,
            Quantity = 2,
            Price = 20.00
        };
        var result = await _controller.EditCartItem(99, editedCartItem);

        // Assert: Check if the result is OkObjectResult
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task EditCartItem_InvalidId_ReturnsNotFound()
    {
        var editedCartItem = new CartItem
        {
            Id = 99,
            CartID = 99,
            ProductID = 99,
            Quantity = 2,
            Price = 20.00
        };

        var result = await _controller.EditCartItem(99, editedCartItem);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }
}
