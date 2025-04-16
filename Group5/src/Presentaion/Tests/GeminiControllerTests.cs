namespace Group5.src.Presentaion.Tests;
using Group5.src.domain.models;
using Group5.src.infrastructure;
using Group5.src.Presentaion.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestClass]
public class GeminiControllerTests
{
    private Group5DbContext _dbContext;
    private GeminiController _controller;
    private Mock<ISession> _mockSession;

    [TestInitialize]
     public void Setup()
    {
        var options = new DbContextOptionsBuilder<Group5DbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _dbContext = new Group5DbContext(options);

        // Mock the session
        _mockSession = new Mock<ISession>();
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(ctx => ctx.Session).Returns(_mockSession.Object);

        // Initialize the controller with the mocked HttpContext
        _controller = new GeminiController(_dbContext)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            }
        };
    }

    [TestMethod]
    public async Task GetChatResponse_ReturnsBadRequest_WhenPromptIsEmpty()
    {
        // Arrange
        var userPrompt = new UserPrompt { Prompt = "" };

        // Act
        var result = await _controller.GetChatResponse(userPrompt);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        var badRequestResult = result as BadRequestObjectResult;
        Assert.AreEqual("Prompt cannot be empty.", badRequestResult.Value);
    }

    [TestMethod]
    public async Task GetChatResponse_ReturnsOk()
    {
        
        var userPrompt = new UserPrompt { Prompt = "Tell me about the left tire" };

        
        var result = await _controller.GetChatResponse(userPrompt);

        
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult.Value);
        
    }





    [TestMethod]
    public async Task GetChatResponse_ReturnsInternalServerError_OnException()
    {
        
        var userPrompt = new UserPrompt { Prompt = "TestProduct" };
        _dbContext.Dispose(); 

        
        var result = await _controller.GetChatResponse(userPrompt);

        
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        var objectResult = result as ObjectResult;
        Assert.AreEqual(500, objectResult.StatusCode);
        Assert.IsTrue(objectResult.Value.ToString().Contains("Internal server error"));
    }

    [TestMethod]
    public void ClearSession_ClearsOK()
    { 
        _mockSession.Setup(s => s.Clear());

        var result = _controller.ClearSession();

        _mockSession.Verify(s => s.Clear(), Times.Once);
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }
}
