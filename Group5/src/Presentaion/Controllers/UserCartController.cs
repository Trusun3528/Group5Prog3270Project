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

        public UserCartController(Group5DbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        private Cart GetUserCart() {
            Cart? cart = HttpContext.Session.GetObject<Cart>("Cart");

            if (cart == null) {
                cart = new Cart();
                SetUserCart(cart);
            }

            return cart;
        }

        private void SetUserCart(Cart cart) {
            HttpContext.Session.SetObject("Cart", cart);
        }
        
        [Authorize]
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

        [Authorize]
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

        [Authorize]
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
    }
}
