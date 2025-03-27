namespace Group5.src.Presentaion.Tests;
using Group5.src.domain.models;
using Group5.src.infrastructure;
using Group5.src.Presentaion.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestClass]
public class CartControllerTests
{
    private Group5DbContext _context;
    private CartController _controller;
    private ILogger<ProductController> _logger;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<Group5DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new Group5DbContext(options);
        _logger = new LoggerFactory().CreateLogger<ProductController>();
        _controller = new CartController(_context, _logger);

        
        _context.Carts.Add(new Cart { Id = 1, UserId = 1 });
        _context.Carts.Add(new Cart { Id = 2, UserId = 2 });

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
            UserId = 3,
            TotalAmount = 100.00m
        };

        
        var result = await _controller.AddCart(newCart);

        
        Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
    }
}
