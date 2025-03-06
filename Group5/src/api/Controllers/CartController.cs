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
using System.Security.Claims;
using Newtonsoft.Json;


namespace Group5.src.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : Controller
    {
        private readonly Group5DbContext _context;
        private readonly ILogger<ProductController> _logger;

        public CartController(Group5DbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("AddCart")]
        public async Task<ActionResult<Cart>> AddCart([FromBody] Cart cart)
        {
            var Cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == Id);
            try
            {
                _logger.LogInformation("start addcart");

                if (!TryValidateModel(cart))
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


            _context.Carts.Add(cart);
                // Ensure the Carts collection is there
                if (user.Carts == null)
                {
                    user.Carts = new List<Cart>();
                }

                cart.TotalAmount = 0;
                user.Carts.Add(cart);

            await _context.SaveChangesAsync();
                _context.Carts.Add(cart);
                _logger.LogInformation("end and saved addcart");
                await _context.SaveChangesAsync();


            return Ok(cart);
        }
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
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts(int id)
        {
            _logger.LogInformation("start getcarts");
            var Id = User.Identity.Name;
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

        [HttpGet("GetCarts/{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
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

        [HttpDelete("DeleteCart/{id}")]
        public async Task<ActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null) return NotFound($"Cart with id {id} not found");
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return Ok($"Cart with id {id} has been deleted");
        }

        [HttpPost("AddCartItem")]
        public async Task<ActionResult<CartItem>> AddCartItem([FromBody] CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);

            await _context.SaveChangesAsync();
        public async Task<ActionResult<CartItem>> AddCartItem([FromBody] CartItem cartItem)
        {
            try
            {
                _logger.LogInformation("started addcartitem");

                if (!TryValidateModel(cartItem))
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
                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                //catches for and other errors
                _logger.LogError(ex, "An error occurred while adding the cart item.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("GetCartItems/{id}")]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems(int id)
        {
            _logger.LogInformation("started getcartitems");
            var cartItems = await _context.CartItems
                .Where(C => C.CartID == id)
                .ToListAsync();
            if(cartItems == null)
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

        [HttpDelete("DeleteCartItem/{id}")]
        public async Task<ActionResult> DeleteCartItem(int id)
        {
            _logger.LogInformation("started deletecartitem");
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound($"Error 404: Cart with id {id} not found");
                _logger.LogWarning($"Cart with id {id} not found");

                return NotFound($"Cart with id {id} not found");
            }

            _context.CartItems.Remove(cartItem);
            _logger.LogInformation("ended deletecartitem");

            await _context.SaveChangesAsync();

            return Ok($"cart Item with id {id} has been deleted");
        }

        [HttpPut("EditCartItem/{id}")]
        public async Task<ActionResult> EditCartItem(int id, [FromBody] dynamic editedCartItemJson)
        {
            _logger.LogInformation("editcaritem started");
            var editedCartItem = JsonConvert.DeserializeObject<CartItem>(editedCartItemJson.ToString());

            if (!TryValidateModel(editedCartItem))
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
            cartItem.Quantity = editedCartItem.Quantity;

            if (!string.IsNullOrEmpty(editedCartItem.Quantity))
            {
                cartItem.Quantity = editedCartItem.Quantity;
            }

            cartItem.Price = product.Price * cartItem.Quantity;


            _context.Entry(cartItem).State = EntityState.Modified;
            _logger.LogInformation("editcaritem ended");

            await _context.SaveChangesAsync();
            return Ok(cartItem);
        }

        [HttpPost("Checkout")]
        public async Task<ActionResult> CheckoutCart([FromBody] GuestCheckoutModel? guestData)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int? userId = userIdClaim != null ? int.Parse(userIdClaim.Value) : null;
            Cart? userCart;

            if (userId != null)
            {

                userCart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

            }
            else
            {

                //if guest data is not shared the return badrequest is sent to the user
                if (guestData == null || guestData.CartItems == null || !guestData.CartItems.Any())

                    return BadRequest("Error 400: Cart is empty or missing.");

                //the cart that only a user can access
                userCart = new Cart
                {

                    CartItems = guestData.CartItems.Select(i => new CartItem
                    {
                        ProductID = i.ProductId,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList()

                };
            }

            //if the cart is empty it shows the BadRequest quote
            if (userCart == null || !userCart.CartItems.Any())
                return BadRequest("Error 400: Cart is empty.");

            //this is to make sure that a user is logged in so they can access the discount
            bool isUserLoggedIn = userId != null;

            //this variable is a sum of all variables in the cart
            decimal totalAmount = userCart.CartItems.Sum(i => i.Price * i.Quantity);

            //this shows the discount percent that only user can access
            decimal discountPercentage = userId != null ? 0.10m : 0.00m;

            //this shows the amount off of the total amount which is what happens when a discount is applied to a total like how 10% of of $10 would be $1 off
            decimal discountAmount = totalAmount * discountPercentage;

            //like for the above variable a total of $10 with a 10% discount is $1 off so $10 - 1$ = $9 new total
            decimal finalAmount = totalAmount - discountAmount;

            var order = new Order
            {
                //registered user id
                UserId = userId,

                //the email of the guest if they give us their email
                GuestEmail = guestData?.GuestEmail,

                //the date and time of when they checkout
                OrderDate = DateTime.UtcNow,

                //the amount the user or guest pays 
                TotalAmount = finalAmount,

                //the discount that only users can use
                DiscountApplied = discountAmount,

                //the cart data for the instance
                OrderItems = userCart.CartItems.Select(i => new OrderItem
                {
                    ProductId = i.ProductID,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            if (userId != null)
            {
                _context.CartItems.RemoveRange(userCart.CartItems);
                await _context.SaveChangesAsync();
            }

            //this will be returned when someone checks out
            return Ok(new
            {
                Message = "Checkout successful!",
                Order = order,
                DiscountApplied = discountAmount,
                FinalTotal = finalAmount
            });
        }
    }
}


