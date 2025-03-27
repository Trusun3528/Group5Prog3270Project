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

        [HttpPost("AddCart")]
        public async Task<ActionResult<Cart>> AddCart([FromBody] Cart cart, int Id)
        {
            var Cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.Id == Id);


            _context.Carts.Add(cart);

            await _context.SaveChangesAsync();


            return Ok(cart);
        }

        [HttpGet("GetCarts")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts(int id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {

                return NotFound($"Cart with id {id} not found");
            }

            return Ok(cart);
        }

        [HttpGet("GetCarts/{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
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

            return Ok(cartItem);
        }

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

            var cartItem = await _context.CartItems.Where(ci => ci.CartID == id && ci.Id == id2)
                                 .FirstOrDefaultAsync();

            if (cartItem == null)
            {

                return NotFound($"Cart item with id {id2} not found");
            }

            return Ok(cartItem);
        }

        [HttpDelete("DeleteCartItem/{id}")]
        public async Task<ActionResult> DeleteCartItem(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound($"Error 404: Cart with id {id} not found");
            }

            _context.CartItems.Remove(cartItem);

            await _context.SaveChangesAsync();

            return Ok($"cart Item with id {id} has been deleted");
        }

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


