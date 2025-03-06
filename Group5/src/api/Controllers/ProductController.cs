/*
 * Project: Open Source Web Programing Midterm Check-In
 * Group Number: 5
 * Group Members: Patrick Harte, Austin Casselman, Austin Cameron, Leif Johannesson
 * Revision History:
 *      Created: January 25th, 2025
 *      Submitted: March 6th, 2025
 */

using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group5.src.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {

        private readonly Group5DbContext _context;

        public ProductController(Group5DbContext context)
        {
            _context = context;
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
            //adds the product
            _context.Products.Add(product);
            //saves the changes
            await _context.SaveChangesAsync();
            //returns the product
            return Ok(product);
        }

        //gets all the products
        // /Product/GetProducts
        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            //gets all the products
            var products = await _context.Products.ToListAsync();
            //returns them
            return Ok(products);
        }
        //gets one product by id
        // /Product/GetProducts/{id}
        [HttpGet("GetProducts/{id}")]
        public async Task<ActionResult<Product>> GetProducts(int id)
        {
            //finds the product
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                //returns if cannot find the product
                return NotFound($"Product with id {id} not found");
            }
            //returns the product
            return Ok(product);
        }
        //edits a product by id
        // /Product/EditProduct/{id}
        [HttpPut("EditProduct/{id}")]
        public async Task<ActionResult> EditProduct(int id, [FromBody] Product editedProduct)
        {
            //Finds the product
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                //returns if not found
                return NotFound($"Product with id {id} not found");
            }

            //updates the product
            product.ProductName = editedProduct.ProductName;
            product.Price = editedProduct.Price;
            product.ProductDescription = editedProduct.ProductDescription;
            product.Stock = editedProduct.Stock;
            product.Catagory = editedProduct.Catagory;
            product.ImageURL = editedProduct.ImageURL;


            //sets the product as edited
            _context.Entry(product).State = EntityState.Modified;
            //Saves the product
            await _context.SaveChangesAsync();
            //returns the edited product
            return Ok(product);
        }
        //deletes a product by id
        // /Product/DeleteProduct/{id}
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            //finds the product
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                //returns if the priduct was not found
                return NotFound($"Product with id {id} not found");
            }

            //removes the product
            _context.Products.Remove(product);

            //saves the changess
            await _context.SaveChangesAsync();

            return Ok($"Product with id {id} has been deleted");
        }
    }
}
