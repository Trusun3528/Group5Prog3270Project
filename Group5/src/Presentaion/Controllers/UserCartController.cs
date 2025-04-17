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
        private readonly ILogger<ProductController> _logger;
        private readonly Group5DbContext _context;
        private readonly UserManager<User> _userManager;

        public UserCartController(Group5DbContext context, ILogger<ProductController> logger, UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        private Cart GetUserCart() {
            Cart? cart = HttpContext.Session.GetObject<Cart?>("Cart");

            if (cart == null) {
                cart = new Cart();
                SetUserCart(cart);
            }

            return cart;
        }

        private void SetUserCart(Cart? cart) {
            HttpContext.Session.SetObject("Cart", cart);
        }
        
        [HttpGet("GetCart")]
        public async Task<ActionResult<List<CartItemResponse>>> GetCart()
        {
            Cart cart = GetUserCart();
            List<CartItemResponse> cartItemResponses = [];

            foreach (CartItem item in cart.Items) {
                cartItemResponses.Add(new CartItemResponse() {
                    Product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId),
                    Quantity = item.Quantity
                });
            }

            return cartItemResponses;
        }

        [HttpPost("AddItem")]
        public ActionResult AddItem([FromBody]CartItem newItem)
        {
            Cart cart = GetUserCart();
            CartItem? item = cart.Items.Find(item => item.ProductId == newItem.ProductId);

            if (item == null) {
                cart.Items.Add(newItem);
            }
            else {
                item.Quantity = item.Quantity;
            }

            SetUserCart(cart);
            return Ok();
        }

        [HttpDelete("RemoveItem/{id}")]
        public ActionResult RemoveItem(int id) {
            Cart cart = GetUserCart();
            CartItem? item = cart.Items.Find(item => item.ProductId == id);
            
            if (item == null) {
                return NotFound();
            }

            cart.Items.Remove(item);

            SetUserCart(cart);
            return Ok();
        }

        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            return Ok(new
            {
                user.Id,
                user.UserName                
            });
        }


        [HttpPost("Checkout")]
        public async Task<ActionResult> Checkout()
        {
            try
            {
                _logger.LogInformation("Checkout process started.");

                var cart = GetUserCart();

                if (cart == null || !cart.Items.Any())
                {
                    _logger.LogWarning("Cart is empty.");
                    return BadRequest("Cart is empty.");
                }

                User? user = await _userManager.GetUserAsync(User);

                var order = new Order
                {
                    UserId = user?.Id,
                    OrderDate = DateTime.UtcNow,
                    OrderItems = new List<OrderItem>()
                };

                var cartItemsForResponse = new List<object>();
                decimal totalAmount = 0;

                foreach (var item in cart.Items)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);

                    if (product == null)
                    {
                        _logger.LogWarning($"Product not found for cart item with ID {item.ProductId}.");
                        return BadRequest($"Product not found for cart item with ID {item.ProductId}.");
                    }

                    if (product.Stock < item.Quantity)
                    {
                        _logger.LogWarning($"Insufficient stock for product with ID {item.ProductId}.");
                        return BadRequest($"Sorry, we can't process your order. Insufficient stock for product: {product.ProductName}.");
                    }

                    var price = (decimal)product.Price;
                    var discountedPrice = CalculateDiscountedPrice(price);

                    order.OrderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = discountedPrice
                    });

                    totalAmount += discountedPrice * item.Quantity;

                    //update stock
                    product.Stock -= item.Quantity;

                    //make this the item format
                    cartItemsForResponse.Add(new
                    {
                        id = item.ProductId,
                        quantity = item.Quantity,
                        price = discountedPrice,
                        product = new
                        {
                            name = product.ProductName
                        }
                    });
                }

                _context.Orders.Add(order);
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
            return price * 0.9m;
        }
    }
}
