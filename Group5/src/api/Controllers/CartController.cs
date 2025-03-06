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
        //private readonly CartModel _cartModel;
        private readonly Group5DbContext _context;

        public CartController(Group5DbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }


        //adds a cart
        [HttpPost("AddCart")]
        public async Task<ActionResult<Cart>> AddCart([FromBody] Cart cart, int Id)
        {

            var Cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CartId == Id);

         
                _context.Carts.Add(cart);

                await _context.SaveChangesAsync();


            return Ok(cart);

 
        }

        //gets all the carts
        [HttpGet("GetCarts")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            var Id = User.Identity.Name;
            var carts = await _context.Carts.ToListAsync();
            
            return Ok(carts);
        }

        //gets one of the carts
        [HttpGet("GetCarts/{id}")]
        public async Task<ActionResult<Product>> GetCarts(int id)
        {

            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {

                return NotFound($"Cart with id {id} not found");
            }

            return Ok(cart);
        }

        //Deletes a cart
        [HttpDelete("DeleteCart/{id}")]
        public async Task<ActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound($"Cart with id {id} not found");
            }

            _context.Carts.Remove(cart);

            await _context.SaveChangesAsync();

            return Ok($"Cart with id {id} has been deleted");
        }

        //adds a item to a cart

        [HttpPost("AddCartItem")]
        public async Task<ActionResult<Cart>> AddCartItem([FromBody] CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);

            await _context.SaveChangesAsync();

            return Ok(cartItem);


        }

        //gets a specific item from a cart
        [HttpGet("GetCartItems/{id}")]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems(int id)
        {
            var cartItems = await _context.CartItems
                .Where(C => C.CartID == id)
                .ToListAsync();

            return Ok(cartItems);
        }
        //Gets all the items in a cart
        [HttpGet("GetCartItems/{id}/{id2}")]
        public async Task<ActionResult<CartItem>> GetCarts(int id, int id2)
        {

            var cartItem = await _context.CartItems.Where(ci => ci.CartID == id && ci.CartItemId == id2)
                                 .FirstOrDefaultAsync();

            if (cartItem == null)
            {

                return NotFound($"Cart item with id {id2} not found");
            }

            return Ok(cartItem);
        }


        //deletes a cartitem
        [HttpDelete("DeleteCartItem/{id}")]
        public async Task<ActionResult> DeleteCartItem(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound($"Cart with id {id} not found");
            }

            _context.CartItems.Remove(cartItem);

            await _context.SaveChangesAsync();

            return Ok($"cart Item with id {id} has been deleted");
        }

        //edit a cart items
        [HttpPut("EditCartItem/{id}")]
        public async Task<ActionResult> EditCartItem(int id, [FromBody] CartItem editedCartItem)
        {
            //Finds the product
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                //returns if not found
                return NotFound($"Cart Item with id {id} not found");
            }

            //updates the product
            cartItem.Quantity = editedCartItem.Quantity;
     


            _context.Entry(cartItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(cartItem);
        }

    }
}
