using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace Group5.src.Presentaion.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserCartController : Controller
    {
        //private readonly CartModel _cartModel;
        private readonly Group5DbContext _context;
        private readonly ILogger<ProductController> _logger;
        private readonly UserManager<User> _userManager;

        public UserCartController(Group5DbContext context, ILogger<ProductController> logger, UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        private async Task<Cart?> GetUserCart() {
            User? user = await _userManager.GetUserAsync(User);

            if (user == null) {
                return null;
            }

            Cart? cart = await _context.Carts
                .Include(c => c.CartItems)
                .Where(c => c.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (cart == null) {
                cart = new Cart() {
                    UserId = user.Id
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }
        
        [Authorize]
        [HttpGet("GetCart")]
        public async Task<ActionResult<List<CartItem>>> GetCart()
        {
            Cart? cart = await GetUserCart();

            if (cart == null) {
                return Unauthorized();
            }

            List<CartItem> cartItems = await _context.CartItems
                .Where(ci => ci.CartID == cart.Id)
                .Include(ci => ci.Product)
                .ToListAsync();

            return cartItems;
        }

        [Authorize]
        [HttpPost("AddItem")]
        public async Task<ActionResult> AddItem([FromBody]AddCartItemModel itemModel)
        {
            Cart? cart = await GetUserCart();

            if (cart == null) {
                return Unauthorized();
            }

            CartItem? cartItem = await _context.CartItems
                .Where(ci => ci.CartID == cart.Id && ci.ProductID == itemModel.ProductId)
                .FirstOrDefaultAsync();

            if (cartItem == null) {
                cartItem = new CartItem() {
                    ProductID = itemModel.ProductId,
                    Quantity = itemModel.Quantity
                };

                cart.CartItems.Add(cartItem);
            }
            else {
                cartItem.Quantity = itemModel.Quantity;
            }
            
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpDelete("RemoveItem/{id}")]
        public async Task<ActionResult> RemoveItem(int id) {
            Cart? cart = await GetUserCart();

            if (cart == null) {
                return Unauthorized();
            }

            CartItem? cartItem = await _context.CartItems
                .Where(ci => ci.CartID == cart.Id && ci.Id == id)
                .FirstOrDefaultAsync();
            
            if (cartItem == null) {
                return NotFound();
            }

            cart.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpPost("Checkout")]
        public async Task<ActionResult> Checkout()
        {
            try
            {
                _logger.LogInformation("Checkout process started.");

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger.LogWarning("User not found.");
                    return Unauthorized();
                }

                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

                if (cart == null || !cart.CartItems.Any())
                {
                    _logger.LogWarning("Cart is empty.");
                    return BadRequest("Cart is empty.");
                }

                var order = new Order
                {
                    UserId = user.Id,
                    OrderDate = DateTime.UtcNow,
                    OrderItems = new List<OrderItem>()
                };

                var cartItemsForResponse = new List<object>();
                decimal totalAmount = 0;

                foreach (var item in cart.CartItems)
                {
                    if (item.Product == null)
                    {
                        _logger.LogWarning($"Product not found for cart item with ID {item.Id}.");
                        return BadRequest($"Product not found for cart item with ID {item.Id}.");
                    }

                    if (item.Product.Stock < item.Quantity)
                    {
                        _logger.LogWarning($"Insufficient stock for product with ID {item.ProductID}.");
                        return BadRequest($"Sorry, we can't process your order. Insufficient stock for product: {item.Product.ProductName}.");
                    }

                    var price = (decimal)item.Product.Price;
                    var discountedPrice = CalculateDiscountedPrice(price);

                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductID,
                        Quantity = item.Quantity,
                        Price = discountedPrice
                    });

                    totalAmount += discountedPrice * item.Quantity;

                    // Update stock
                    item.Product.Stock -= item.Quantity;

                    // Add formatted cart item for frontend
                    cartItemsForResponse.Add(new
                    {
                        id = item.Id,
                        quantity = item.Quantity,
                        price = discountedPrice,
                        product = new
                        {
                            name = item.Product.ProductName
                        }
                    });
                }

                _context.Orders.Add(order);
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Checkout process completed successfully.");

                return Ok(new
                {
                    orderId = order.Id,
                    totalAmount = totalAmount,
                    cartItems = cartItemsForResponse
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during checkout process.");
                return StatusCode(500, "An error occurred during the checkout process.");
            }
        }

        private decimal CalculateDiscountedPrice(decimal price)
        {
            // Example discount logic
            return price * 0.9m;
        }
    }
}
