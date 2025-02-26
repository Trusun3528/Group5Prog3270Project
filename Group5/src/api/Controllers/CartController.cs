using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group5.src.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : Controller
    {

        private readonly Group5DbContext _context;

        public CartController(Group5DbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        
        [HttpPost("AddCart")]
        public async Task<ActionResult<Product>> AddProduct([FromBody] Cart cart)
        {
           
            return Ok(cart);
        }

        
        [HttpGet("GetCarts")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            
            var carts = await _context.Products.ToListAsync();
            
            return Ok(carts);
        }
        

        [HttpGet("GetCarts/{id}")]
        public async Task<ActionResult<Product>> GetCarts(int id)
        {
            
            var cart = await _context.Products.FindAsync(id);

            if (cart == null)
            {
                
                return NotFound($"Product with id {id} not found");
            }
            
            return Ok(cart);
        }
       
        [HttpPut("EditCart/{id}")]
        public async Task<ActionResult> EditProduct(int id, [FromBody] Cart editedCart)
        {
            var cart = await _context.Products.FindAsync(id);

            
            return Ok(cart);
        }
        

        [HttpDelete("DeleteCart/{id}")]
        public async Task<ActionResult> DeleteCart(int id)
        {
            var cart = await _context.Products.FindAsync(id);

            if (cart == null)
            {
                return NotFound($"Product with id {id} not found");
            }

            _context.Products.Remove(cart);

            await _context.SaveChangesAsync();

            return Ok($"Product with id {id} has been deleted");
        }
    }
}
