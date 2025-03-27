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

        
        _context.Cards.Add(new Card { Id = 5, CreditCardNumber = "1234567890123456", ExpirationDate = new DateTime(2025, 12, 31), CVV = "123" });
        _context.Cards.Add(new Card { Id = 3, CreditCardNumber = "1234567890123456", ExpirationDate = new DateTime(2025, 12, 31), CVV = "123" });

        _context.SaveChanges();
    }

    [TestMethod]
    public async Task GetCard_ValidId_ReturnsOk()
    {
        
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

    [TestMethod]
    public async Task DeleteCard_ValidId_ReturnsOk()
    {
        var result = await _controller.DeleteCard(3);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task DeleteCard_InvalidId_ReturnsNotFound()
    {
        var result = await _controller.DeleteCard(1);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task CreateCard_ValidCard_ReturnsOK()
    {
        var newCard = new Card { CreditCardNumber = "8765438726351893", ExpirationDate = new DateTime(2026, 10, 25), CVV = "863" };
        var result = await _controller.CreateCard(newCard);
        Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
    }

    [TestMethod]
    public async Task CreateCard_InvalidCard_ReturnsBadRequest()
    {

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var invalidCard = new Card
        {
            Id = 0,
            CreditCardNumber = "",
            ExpirationDate = new DateTime(2000, 01, 01),
            CVV = ""
        };


        _controller.ModelState.AddModelError("CreditCardNumber", "Credit Card Number is required");
        _controller.ModelState.AddModelError("ExpirationDate", "Expiration date must be in the future");
        _controller.ModelState.AddModelError("CVV", "CCV Number is Required");

        var result = await _controller.CreateCard(invalidCard);


        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
    }
}
