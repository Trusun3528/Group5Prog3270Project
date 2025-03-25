using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Group5.src.Presentaion.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {

        private readonly Group5DbContext _context;
        private readonly ILogger<ProductController> _logger;
        public ProductController(Group5DbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        //adds a product
        // /Product/AddProduct
        [HttpPost("AddProduct")]
        public async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
        {
            _logger.LogInformation("Start Add product");

            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Product body does not match the model");
                return BadRequest(ModelState);
            }

            // Adds the product
            _context.Products.Add(product);
            _logger.LogInformation("End and saved addproduct");

            // Saves the changes
            await _context.SaveChangesAsync();

            // Returns the product
            return Ok(product);
        }



        //gets all the products
        // /Product/GetProducts
        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            _logger.LogInformation("Entering GetProducts");
            //gets all the products
            var products = await _context.Products.ToListAsync();
            _logger.LogInformation("Got Products");
            //returns them
            return Ok(products);
        }
        //gets one product by id
        // /Product/GetProducts/{id}
        [HttpGet("GetProducts/{id}")]
        public async Task<ActionResult<Product>> GetProducts(int id)
        {
            _logger.LogInformation("Entering GetProducts/id");
            //finds the product
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product with that id is not found");
                //returns if cannot find the product
                return NotFound($"Product with id {id} not found");
            }
            _logger.LogInformation("Got the product");
            //returns the product
            return Ok(product);
        }
        //edits a product by id
        // /Product/EditProduct/{id}
        [HttpPut("EditProduct/{id}")]
        public async Task<ActionResult> EditProduct(int id, [FromBody] Product editedProduct)
        {
            _logger.LogInformation("Start of editproduct");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Product body does not match the model");
                return BadRequest(ModelState);
            }

            // Finds the product
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product with that id is not found");
                // Returns if not found
                return NotFound($"Product with id {id} not found");
            }

            // Updates the product
            if (!string.IsNullOrEmpty(editedProduct.ProductName))
            {
                product.ProductName = editedProduct.ProductName;
            }
            if (editedProduct.Price.HasValue)
            {
                product.Price = editedProduct.Price;
            }
            if (!string.IsNullOrEmpty(editedProduct.ProductDescription))
            {
                product.ProductDescription = editedProduct.ProductDescription;
            }
            if (editedProduct.Stock > 0)
            {
                product.Stock = editedProduct.Stock;
            }
            if (!string.IsNullOrEmpty(editedProduct.Catagory))
            {
                product.Catagory = editedProduct.Catagory;
            }
            if (!string.IsNullOrEmpty(editedProduct.ImageURL))
            {
                product.ImageURL = editedProduct.ImageURL;
            }

            // Sets the product as edited
            _context.Entry(product).State = EntityState.Modified;

            // Saves the product
            await _context.SaveChangesAsync();

            // Returns the edited product
            _logger.LogInformation("Editproduct done and saved product");
            return Ok(product);
        }

        //deletes a product by id
        // /Product/DeleteProduct/{id}
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation("Product delete started");
            //finds the product
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                _logger.LogInformation("Product with that id does not exist");
                //returns if the priduct was not found
                return NotFound($"Product with id {id} not found");
            }

            //removes the product
            _context.Products.Remove(product);

            //saves the changess
            await _context.SaveChangesAsync();
            _logger.LogInformation("Product deleted");
            return Ok($"Product with id {id} has been deleted");
        }

        [HttpPost("AddRating")]
        public async Task<ActionResult> AddRating([FromBody] Rating rating)
        {
            _logger.LogInformation("Start of AddRatingWithComment");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Rating body does not match the model");
                return BadRequest(ModelState);
            }

            //Adds the rating
            _context.Ratings.Add(rating);
            _logger.LogInformation("Saved Rating");

            // Saves the changes
            await _context.SaveChangesAsync();

            //Gets the updated average rating
            var productRatings = _context.Ratings
                .Where(r => r.ProductId == rating.ProductId)
                .Select(r => r.RatingNumber);

            double averageRating = productRatings.Any() ? productRatings.Average() : 0.0;

            //Updates the product rating
            var productToUpdate = await _context.Products.FindAsync(rating.ProductId);
            if (productToUpdate != null)
            {
                productToUpdate.Rating = averageRating;
                _logger.LogInformation($"Updated Product ID {rating.ProductId} with new Rating: {averageRating}");
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"Product ID {rating.ProductId} not found.");
            }

            //Returns the rating
            return Ok(rating);
        }

        [HttpGet("GetRatingsByProduct/{productId}")]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatingsByProduct(int productId)
        {
            _logger.LogInformation($"Fetching ratings for Product ID: {productId}");

            var ratings = await _context.Ratings
                .Where(r => r.ProductId == productId)
                .ToListAsync();

            if (ratings == null || !ratings.Any())
            {
                _logger.LogWarning($"No ratings found for Product ID: {productId}");
                return NotFound($"No ratings found for Product ID: {productId}");
            }

            return Ok(ratings);
        }


    }
}
