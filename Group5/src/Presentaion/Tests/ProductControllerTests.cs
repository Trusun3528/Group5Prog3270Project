namespace Group5.src.Presentaion.Tests;
using Group5.src.domain.models;
using Group5.src.infrastructure;
using Group5.src.Presentaion.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
[TestClass]
public class ProductControllerTests
{
    private Group5DbContext _context;
    private ProductController _controller;

    [TestInitialize]
    public void Setup()
    {

        var options = new DbContextOptionsBuilder<Group5DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new Group5DbContext(options);
        var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<ProductController>();
        _controller = new ProductController(_context, logger);

        
        _controller = new ProductController(_context, logger)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };

        // Seed the database with test data
        _context.Products.Add(new Product { Id = 1, ProductName = "Test Product 1", Price = 10.00, ProductDescription = "Description 1", Stock = 100, Catagory = "Category 1", ImageURL = "http://example.com/image1.jpg" });
        _context.Products.Add(new Product { Id = 2, ProductName = "Test Product 2", Price = 20.00, ProductDescription = "Description 2", Stock = 200, Catagory = "Category 2", ImageURL = "http://example.com/image2.jpg" });

        _context.SaveChanges();
    }

    [TestMethod]
    public async Task AddProduct_ValidProduct_ReturnsOk()
    {
        
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var newProduct = new Product
        {
            Id =10,
            ProductName = "New Product",
            Price = 30.00,
            ProductDescription = "New Description",
            Stock = 300,
            Catagory = "Category 3",
            ImageURL = "http://example.com/image3.jpg"
        };

        _controller.ModelState.Clear(); 

        
        var result = await _controller.AddProduct(newProduct);

        
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task AddProduct_InvalidProduct_ReturnsBadRequest()
    {
        
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var invalidProduct = new Product
        {
            Id = 0, 
            ProductName = "", 
            Price = -1.00, 
            ProductDescription = "Invalid Description",
            Stock = -10, 
            Catagory = "Category Invalid",
            ImageURL = "" 
        };

        _controller.ModelState.AddModelError("ProductName", "Product Name is required");
        _controller.ModelState.AddModelError("Price", "Price must be non-negative");
        _controller.ModelState.AddModelError("Stock", "Stock must be non-negative");

        
        var result = await _controller.AddProduct(invalidProduct);

        
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
    }


    [TestMethod]
    public async Task GetProducts_ReturnsAllProducts()
    {
        var result = await _controller.GetProducts();
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        var products = (result.Result as OkObjectResult).Value as List<Product>;
        Assert.AreEqual(2, products.Count);
    }

    [TestMethod]
    public async Task GetProduct_ValidId_ReturnsOk()
    {
        var result = await _controller.GetProducts(1);
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetProduct_InvalidId_ReturnsNotFound()
    {
        var result = await _controller.GetProducts(99);
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task EditProduct_ValidId_ReturnsOk()
    {
        var editedProduct = new Product { ProductName = "Edited Product", Price = 40.00, ProductDescription = "Edited Description", Stock = 400, Catagory = "Category 4", ImageURL = "http://example.com/image4.jpg" };
        var result = await _controller.EditProduct(1, editedProduct);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task EditProduct_InvalidId_ReturnsNotFound()
    {
        var editedProduct = new Product
        {
            ProductName = "Edited Product",
            Price = 40.00,
            ProductDescription = "Edited Description",
            Stock = 400,
            Catagory = "Category 4",
            ImageURL = "http://example.com/image4.jpg"
        };

        var result = await _controller.EditProduct(99, editedProduct);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task DeleteProduct_ValidId_ReturnsOk()
    {
        var result = await _controller.DeleteProduct(1);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task DeleteProduct_InvalidId_ReturnsNotFound()
    {
        var result = await _controller.DeleteProduct(99);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task AddRating_ValidRating_ReturnsOk()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var newRating = new Rating
        {
            Id = 1,
            ProductId = 10,
            UserId = 5,
            RatingNumber = 4,
            Comment = "Great product!"
        };

        _controller.ModelState.Clear();

        // Act
        var result = await _controller.AddRating(newRating);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetRatingsForProduct_ValidProductId_ReturnsOk()
    {
        // Arrange
        var productId = 1; 
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        _context.Ratings.AddRange(new List<Rating>
        {
        new Rating { Id = 1, ProductId = productId, UserId = 1, RatingNumber = 5, Comment = "Excellent!" }
        });

        await _context.SaveChangesAsync();
        // Act
        var actionResult = await _controller.GetRatingsByProduct(productId);
        var result = actionResult.Result as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var ratings = result.Value as IEnumerable<Rating>;
        Assert.IsNotNull(ratings);
        Assert.IsTrue(ratings.Any()); 
    }

    [TestMethod]
    public async Task GetRatingsForProduct_InvalidProductId_ReturnsNotFound()
    {
        
        var invalidProductId = -1; 
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        
        var actionResult = await _controller.GetRatingsByProduct(invalidProductId);
        var result = actionResult.Result as NotFoundObjectResult;

        
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        Assert.AreEqual($"No ratings found for Product ID: {invalidProductId}", result.Value);
    }





}
