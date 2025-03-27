using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace Group5.src.Presentaion.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : Controller
    {
        //private readonly CartModel _cartModel;
        private readonly Group5DbContext _context;
        private readonly ILogger<ProductController> _logger;

        public CartController(Group5DbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }


        //adds a cart
        [HttpPost("AddCart")]
        public async Task<ActionResult<Cart>> AddCart([FromBody] Cart cart)
        {
            try
            {
                _logger.LogInformation("start addcart");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Cart body does not match the model");
                    return BadRequest(ModelState);
                }

                //makes sure the user exists
                var user = await _context.Users.Include(u => u.Carts).FirstOrDefaultAsync(u => u.Id == cart.UserId);
                if (user == null)
                {
                    _logger.LogWarning($"User with id {cart.UserId} not found");
                    return NotFound($"User with id {cart.UserId} not found");
                }

                // Ensure the Carts collection is there
                if (user.Carts == null)
                {
                    user.Carts = new List<Cart>();
                }

                cart.TotalAmount = 0;
                user.Carts.Add(cart);

                _context.Carts.Add(cart);
                _logger.LogInformation("end and saved addcart");
                await _context.SaveChangesAsync();

                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the cart.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }






        //gets all the carts
        [HttpGet("GetCarts")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            _logger.LogInformation("start getcarts");
            var carts = await _context.Carts.ToListAsync();
            
            return Ok(carts);
        }

        //gets one of the carts
        [HttpGet("GetCarts/{id}")]
        public async Task<ActionResult<Product>> GetCarts(int id)
        {
            _logger.LogInformation("start getcarts");
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                _logger.LogWarning("Cart with that id does not exist");
                return NotFound($"Cart with id {id} not found");
            }
            _logger.LogInformation("ended getcarts");
            return Ok(cart);
        }

        //Deletes a cart
        [HttpDelete("DeleteCart/{id}")]
        public async Task<ActionResult> DeleteCart(int id)
        {
            _logger.LogInformation("start deletecart");
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                _logger.LogWarning("cart with that id does not exist");
                return NotFound($"Cart with id {id} not found");
            }

            _context.Carts.Remove(cart);
            _logger.LogInformation("end deletecart");
            await _context.SaveChangesAsync();

            return Ok($"Cart with id {id} has been deleted");
        }

        //adds a item to a cart

        [HttpPost("AddCartItem")]
        public async Task<ActionResult<CartItem>> AddCartItem([FromBody] CartItem cartItem)
        {
            try
            {
                _logger.LogInformation("started addcartitem");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("cartitem body does not match the model");
                    return BadRequest(ModelState);
                }

                //sees if the cart exists
                var cart = await _context.Carts.FindAsync(cartItem.CartID);
                if (cart == null)
                {
                    _logger.LogWarning($"Cart with id {cartItem.CartID} not found");
                    return NotFound($"Cart with id {cartItem.CartID} not found");
                }

                //sees if the product exists
                var product = await _context.Products.FindAsync(cartItem.ProductID);
                if (product == null)
                {
                    _logger.LogWarning($"Product with id {cartItem.ProductID} not found");
                    return NotFound($"Product with id {cartItem.ProductID} not found");
                }

                //gets the price based on the product and the quantity
                cartItem.Price = product.Price * cartItem.Quantity;
                cartItem.Cart = cart;
                cartItem.Product = product;

                _context.CartItems.Add(cartItem);
                _logger.LogInformation("end and saved cart item");
                await _context.SaveChangesAsync();

                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                //catches for and other errors
                _logger.LogError(ex, "An error occurred while adding the cart item.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        //gets a specific item from a cart
        [HttpGet("GetCartItems/{id}")]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems(int id)
        {
            _logger.LogInformation("started getcartitems");
            var cartItems = await _context.CartItems
                .Where(C => C.CartID == id)
                .ToListAsync();
            if (cartItems == null || !cartItems.Any())
            {
                _logger.LogWarning($"Cart item with the id of {id} not found");
                return NotFound($"Cart item with the id of {id} not found");
            }
            _logger.LogInformation("end addcartitem");
            return Ok(cartItems);
        }
        //Gets all the items in a cart
        [HttpGet("GetCartItems/{id}/{id2}")]
        public async Task<ActionResult<CartItem>> GetCarts(int id, int id2)
        {
            _logger.LogInformation("started getcartitems/id/id");
            var cartItem = await _context.CartItems.Where(ci => ci.CartID == id && ci.Id == id2)
                                 .FirstOrDefaultAsync();

            if (cartItem == null)
            {
                _logger.LogWarning($"Cart item with id {id2} not found");
                return NotFound($"Cart item with id {id2} not found");
            }
            _logger.LogInformation("started getcartitems/id/id");
            return Ok(cartItem);
        }


        //deletes a cartitem
        [HttpDelete("DeleteCartItem/{id}")]
        public async Task<ActionResult> DeleteCartItem(int id)
        {
            _logger.LogInformation("started deletecartitem");
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                _logger.LogWarning($"Cart with id {id} not found");

                return NotFound($"Cart with id {id} not found");
            }

            _context.CartItems.Remove(cartItem);
            _logger.LogInformation("ended deletecartitem");

            await _context.SaveChangesAsync();

            return Ok($"cart Item with id {id} has been deleted");
        }

        //edit a cart items
        [HttpPut("EditCartItem/{id}")]
        public async Task<ActionResult> EditCartItem(int id, [FromBody] CartItem editedCartItem)
        {
            _logger.LogInformation("editcaritem started");

            if (!ModelState.IsValid)
            {
                _logger.LogInformation("cartitem body does not match the model");
                return BadRequest(ModelState);
            }

            //Finds the product
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                _logger.LogWarning($"CartItem with id {id} not found");
                //returns if not found
                return NotFound($"Cart Item with id {id} not found");
            }

            //checks to see if the product exists and get the info
            var product = await _context.Products.FindAsync(cartItem.ProductID);
            if (product == null)
            {
                _logger.LogWarning($"Product with id {cartItem.ProductID} not found");
                return NotFound($"Product with id {cartItem.ProductID} not found");
            }

            //updates the product
            if (editedCartItem.Quantity > 0)
            {
                cartItem.Quantity = editedCartItem.Quantity;
            }

            cartItem.Price = product.Price * cartItem.Quantity;

            _context.Entry(cartItem).State = EntityState.Modified;
            _logger.LogInformation("editcaritem ended");

            await _context.SaveChangesAsync();
            return Ok(cartItem);
        }

    }
}
