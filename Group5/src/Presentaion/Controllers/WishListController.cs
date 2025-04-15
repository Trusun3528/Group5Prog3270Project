using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group5.src.Presentaion.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WishListController : Controller
    {
        private readonly Group5DbContext _context;
        private readonly UserManager<User> _userManager;

        public WishListController(Group5DbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //adds a item to a wishlist
        [Authorize]
        [HttpPost("AddToWishlist")]
        public async Task<ActionResult> AddToWishlist(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            //will return unauthorized if not logged in
            if (user == null)
            {
                return Unauthorized();
            }

            //makes sure the product exists
            var product = await _context.Products.FindAsync(productId);
            if (product == null) 
            { 
                return NotFound($"Product with id {productId} not found."); 
            }

            //makes sure product is not already in the wishlist
            var existingItem = await _context.WishListItems
                .FirstOrDefaultAsync(w => w.UserId == user.Id && w.ProductId == productId);

            //returns if product is already in the wishlist
            if (existingItem != null)
            {
                return BadRequest("Product is already in the wishlist.");
            }

            //makes a new wishlist item
            var wishlistItem = new WishListItem
            {
                UserId = user.Id,
                ProductId = productId
            };

            //saves wishlist item
            _context.WishListItems.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return Ok("product added to wishlist.");
        }

        //gets all the wishlist items for the user
        [Authorize]
        [HttpGet("GetWishlist")]
        public async Task<ActionResult<IEnumerable<Product>>> GetWishlist()
        {
            //gets the user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            //gets all the wishlist items for the user
            var wishlistItems = await _context.WishListItems
                .Where(wl => wl.UserId == user.Id)
                .Include(wl => wl.Product)
                .ToListAsync();

            //puts the items in a var
            var products = wishlistItems.Select(w => w.Product).ToList();
            return Ok(products);
        }

        //deletes a wishlist item
        [Authorize]
        [HttpDelete("DeleteFromWishlist/{productId}")]
        public async Task<ActionResult> DeleteFromWishlist(int productId)
        {
            //gets the user
            var user = await _userManager.GetUserAsync(User);
            //returns if the user is not logged in
            if (user == null)
            {
                return Unauthorized();
            }
            //gets the wishlist items
            var wishlistItem = await _context.WishListItems
                .FirstOrDefaultAsync(wl => wl.UserId == user.Id && wl.ProductId == productId);

            //reaturns if no wishlist items are found
            if (wishlistItem == null)
            {
                return NotFound("product not found in wishlist.");
            }

            //deletes the wishlist item
            _context.WishListItems.Remove(wishlistItem);
            await _context.SaveChangesAsync();
            return Ok("product removed from wishlist.");
        }

        
    }
}
