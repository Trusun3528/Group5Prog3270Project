namespace Group5.src.Presentaion.Tests;
using Group5.src.domain.models;
using Group5.src.infrastructure;
using Group5.src.Presentaion.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class CardControllerTests
{
    private Group5DbContext _context;
    private CardController _controller;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<Group5DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new Group5DbContext(options);
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ProductController>();
        _controller = new CardController(_context, logger);

        // Seed the database with test data
        _context.Cards.Add(new Card { Id = 5, CreditCardNumber = "1234567890123456", ExpirationDate = new DateTime(2025, 12, 31), CVV = "123" });
        _context.Cards.Add(new Card { Id = 3, CreditCardNumber = "1234567890123456", ExpirationDate = new DateTime(2025, 12, 31), CVV = "123" });

        _context.SaveChanges();
    }

    [TestMethod]
    public async Task GetCard_ValidId_ReturnsOk()
    {
        // Seed the database with test data
        var result = await _controller.GetCard(3);
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetCard_InvalidId_ReturnsNotFound()
    {
        var result = await _controller.GetCard(1);
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task EditCard_ValidId_ReturnsOk()
    {
        var editCard = new Card { CreditCardNumber = "6543210987654321", ExpirationDate = new DateTime(2026, 12, 31), CVV = "321" };
        var result = await _controller.EditCard(3, editCard);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task EditCard_InvalidId_ReturnsNotFound()
    {
        var editCard = new Card { CreditCardNumber = "6543210987654321", ExpirationDate = new DateTime(2026, 12, 31), CVV = "321" };
        var result = await _controller.EditCard(99, editCard);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }
}
