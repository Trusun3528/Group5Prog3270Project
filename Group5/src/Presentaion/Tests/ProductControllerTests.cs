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

        
        _context.Products.Add(new Product { Id = 1, ProductName = "Test Product 1", Price = 10.00, ProductDescription = "Description 1", Stock = 100, CatagoryId = 1, ImageURL = "http://example.com/image1.jpg" });
        _context.Products.Add(new Product { Id = 2, ProductName = "Test Product 2", Price = 20.00, ProductDescription = "Description 2", Stock = 200, CatagoryId = 2, ImageURL = "http://example.com/image2.jpg" });
        _context.Products.Add(new Product { Id = 3, ProductName = "Test Product 3", Price = 30.00, ProductDescription = "Description 3", Stock = 300, CatagoryId = 3, ImageURL = "http://example.com/image3.jpg" });

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
            CatagoryId = 3,
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
            CatagoryId = 10000000,
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
        Assert.AreEqual(3, products.Count);
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
        var editedProduct = new Product { ProductName = "Edited Product", Price = 40.00, ProductDescription = "Edited Description", Stock = 400, CatagoryId = 4, ImageURL = "http://example.com/image4.jpg" };
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
            CatagoryId = 4,
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
       
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var newRating = new Rating
        {
            Id = 1,
            ProductId = 1,
            UserId = "5",
            RatingNumber = 4,
            Comment = "Great product!"
        };

        _controller.ModelState.Clear();

        
        var result = await _controller.AddRating(newRating);

        
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetRatingsForProduct_ValidProductId_ReturnsOk()
    {
       
        var productId = 1; 
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        _context.Ratings.AddRange(new List<Rating>
        {
        new Rating { Id = 1, ProductId = productId, UserId = "1", RatingNumber = 5, Comment = "Excellent!" }
        });

        await _context.SaveChangesAsync();
        
        var actionResult = await _controller.GetRatingsByProduct(productId);
        var result = actionResult.Result as OkObjectResult;

        
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

    [TestMethod]
    public async Task SearchProducts_ExistingProducts_ReturnsOk()
    {
       
        var searchWord = "Tire";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        
        _context.ChangeTracker.Clear();
        if (!_context.Products.Any(p => p.ProductName == "Left Tire"))
        {
            _context.Products.AddRange(new List<Product>
        {
            new Product { Id = 101, ProductName = "Left Tire" },
            new Product { Id = 102, ProductName = "Sink Plunger" }
        });

            await _context.SaveChangesAsync();
        }

        
        var actionResult = await _controller.SearchProducts(searchWord);
        var result = actionResult.Result as OkObjectResult;

       
        Assert.IsNotNull(result, "Expected OkObjectResult but got null.");
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var products = result.Value as IEnumerable<Product>;
        Assert.IsNotNull(products, "Expected a list of products but got null.");
        Assert.IsTrue(products.Any(p => p.ProductName.Contains("Tire")), "Expected at least one product containing 'Tire'.");
    }



    [TestMethod]
    public async Task SearchProducts_EmptySearchWord_ReturnsBadRequest()
    {
       
        var searchWord = string.Empty;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        
        var actionResult = await _controller.SearchProducts(searchWord);
        var result = actionResult.Result as BadRequestObjectResult;

        
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        Assert.AreEqual("searchWord must not be empty", result.Value);
    }

    [TestMethod]
    public async Task SearchProducts_NonExistentSearchWord_ReturnsNotFound()
    {
        var searchWord = "NonMatchingSearch";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        
        var actionResult = await _controller.SearchProducts(searchWord);
        var result = actionResult.Result as NotFoundObjectResult;
    
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        Assert.AreEqual($"No products found matching the searchWord '{searchWord}'", result.Value);
    }

    [TestMethod]
    public async Task SearchProductsByCategory_ValidCategory_ReturnsOk()
    {
        
        var categoryName = "Electronics";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

       
        _context.ChangeTracker.Clear();
        var category = new Category
        {
            Id = 1,
            CategoryName = categoryName,
            Description = "Category for electronic items." 
        };
        _context.Categories.Add(category);

        _context.Products.AddRange(new List<Product>
    {
        new Product { Id = 101, ProductName = "Smartphone", CatagoryId = category.Id },
        new Product { Id = 102, ProductName = "Laptop", CatagoryId = category.Id }
    });

        await _context.SaveChangesAsync();

        
        var actionResult = await _controller.SearchProductsByCategory(categoryName);
        var result = actionResult.Result as OkObjectResult;

     
        Assert.IsNotNull(result, "Expected OkObjectResult but got null.");
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var products = result.Value as IEnumerable<Product>;
        Assert.IsNotNull(products, "Expected a list of products but got null.");
        Assert.IsTrue(products.Any(), "Expected products but found none.");
        Assert.IsTrue(products.All(p => p.CatagoryId == category.Id), "All products should belong to the specified category.");
    }


    [TestMethod]
    public async Task SearchProductsByCategory_EmptyCategoryName_ReturnsBadRequest()
    {
        
        var categoryName = string.Empty;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        
        var actionResult = await _controller.SearchProductsByCategory(categoryName);
        var result = actionResult.Result as BadRequestObjectResult;

        
        Assert.IsNotNull(result, "Expected BadRequestObjectResult but got null.");
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        Assert.AreEqual("Category name must not be empty", result.Value);
    }

    [TestMethod]
    public async Task SearchProductsByCategory_NonExistentCategory_ReturnsNotFound()
    {
        
        var categoryName = "NonExistentCategory";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        
        _context.ChangeTracker.Clear();
        _context.Categories.Add(new Category
        {
            Id = 1,
            CategoryName = "Electronics",
            Description = "Category for electronic items." 
        });
        await _context.SaveChangesAsync();

        
        var actionResult = await _controller.SearchProductsByCategory(categoryName);
        var result = actionResult.Result as NotFoundObjectResult;

        
        Assert.IsNotNull(result, "Expected NotFoundObjectResult but got null.");
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        Assert.AreEqual($"No category found with the name '{categoryName}'", result.Value);
    }


    [TestMethod]
    public async Task SearchProductsByCategory_EmptyProductsInCategory_ReturnsNotFound()
    {
        
        var categoryName = "EmptyCategory";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        
        _context.ChangeTracker.Clear();
        var category = new Category
        {
            Id = 5,
            CategoryName = categoryName,
            Description = "This is a test category with no products." 
        };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        
        var existingProducts = _context.Products.Where(p => p.CatagoryId == category.Id).ToList();
        Assert.IsFalse(existingProducts.Any(), "Expected no products in the category, but found some.");

        var actionResult = await _controller.SearchProductsByCategory(categoryName);
        var result = actionResult.Result as NotFoundObjectResult;

        Assert.IsNotNull(result, "Expected NotFoundObjectResult but got null.");
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        Assert.AreEqual($"No products found in the category '{categoryName}'", result.Value);
    }

    [TestMethod]
    public async Task EditProduct_ModelStateError_ReturnsBadRequest()
    {
        
        var invalidProduct = new Product
        {
            ProductName = "", 
            Price = -1.00, 
            ProductDescription = "Invalid Description",
            Stock = -10, 
            CatagoryId = 10000000,
            ImageURL = "" 
        };

        _controller.ModelState.AddModelError("ProductName", "Product Name is required");
        _controller.ModelState.AddModelError("Price", "Price must be non-negative");
        _controller.ModelState.AddModelError("Stock", "Stock must be non-negative");

        
        var result = await _controller.EditProduct(1, invalidProduct);

        
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task AddRating_InvalidModelState_ReturnsBadRequest()
    {
       
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var invalidRating = new Rating
        {
            Id = 1,
            ProductId = 10,
            UserId = "5",
            RatingNumber = 6,
            Comment = "Great product!"
        };

        _controller.ModelState.AddModelError("RatingNumber", "RatingNumber must be between 1 and 5");

        
        var result = await _controller.AddRating(invalidRating);

        
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task AddRating_InvalidProductId_ReturnsNotFound()
    {
        
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var invalidRating = new Rating
        {
            Id = 1,
            ProductId = 999,
            UserId = "5",
            RatingNumber = 4,
            Comment = "Great product!"
        };

        _controller.ModelState.Clear();

        
        var result = await _controller.AddRating(invalidRating);

       
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        var notFoundResult = result as NotFoundObjectResult;
        Assert.AreEqual($"Product with ID {invalidRating.ProductId} not found.", notFoundResult.Value);
    }


    [TestMethod]
    public async Task PriceRangeFilter_ValidRange_ReturnsOk()
    {
        var priceRange = new PriceRange { MinPrice = 10, MaxPrice = 30 };

        var result = await _controller.PriceRangeFilter(priceRange);

        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        var products = (result.Result as OkObjectResult).Value as List<Product>;
        Assert.AreEqual(3, products.Count);
    }

    [TestMethod]
    public async Task PriceRangeFiltere_NoProductsInRange_ReturnsNotFound()
    {
        var priceRange = new PriceRange { MinPrice = 40, MaxPrice = 50 };

        var result = await _controller.PriceRangeFilter(priceRange);

        Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task PriceRangeFilter_InvalidRange_ReturnsBadRequest()
    {
        var priceRange = new PriceRange { MinPrice = 30, MaxPrice = 10 };

        var result = await _controller.PriceRangeFilter(priceRange);

        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
    }





}
