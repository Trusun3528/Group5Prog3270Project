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
        
        [Authorize]
        [HttpGet("GetCart")]
        public async Task<ActionResult<List<CartItem>>> GetCart()
        {
            User? user = await _userManager.GetUserAsync(User);

            if (user == null) {
                return Unauthorized();
            }

            Cart? cart = await _context.Carts
                .Where(c => c.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (cart == null) {
                cart = new Cart() {
                    UserId = user.Id
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            List<CartItem> cartItems = await _context.CartItems
                .Where(ci => ci.CartID == cart.Id)
                .ToListAsync();

            return cartItems;
        }
    }
}
