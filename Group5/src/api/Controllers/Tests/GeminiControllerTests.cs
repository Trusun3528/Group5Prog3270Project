namespace Group5.Tests;
using Group5.src.api.Controllers;
using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestClass]
public class GeminiControllerTests
{
    private Group5DbContext _dbContext;
    private GeminiController _controller;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<Group5DbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _dbContext = new Group5DbContext(options);
        _controller = new GeminiController(_dbContext);
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
        // Arrange
        var userPrompt = new UserPrompt { Prompt = "Ttell me about the left tire" };

        // Act
        var result = await _controller.GetChatResponse(userPrompt);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult.Value);
        // Additional assertions can be added here based on the expected content of the response
    }





    [TestMethod]
    public async Task GetChatResponse_ReturnsInternalServerError_OnException()
    {
        // Arrange
        var userPrompt = new UserPrompt { Prompt = "TestProduct" };
        _dbContext.Dispose(); // Force an exception by disposing the context

        // Act
        var result = await _controller.GetChatResponse(userPrompt);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ObjectResult));
        var objectResult = result as ObjectResult;
        Assert.AreEqual(500, objectResult.StatusCode);
        Assert.IsTrue(objectResult.Value.ToString().Contains("Internal server error"));
    }
}
