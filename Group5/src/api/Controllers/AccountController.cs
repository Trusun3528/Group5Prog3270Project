using Microsoft.AspNetCore.Mvc;
using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Group5.src.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly Group5DbContext _context;

        public AccountController(Group5DbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("CreateAccount")]
        public async Task<ActionResult> CreateAccount([FromBody] CreateUserModel request)
        {
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
                Role = "Customer",
                Cards = new List<Card>(),
                Carts = new List<Cart>(),
                Orders = new List<Order>()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPost("SignInAccount")]
        public async Task<ActionResult> SignInAccount([FromBody] SignInModel request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid Sign in.");
            }

            return Ok(new { Message = "Sign-in successful.", User = user });
        }

        [HttpPost("SignOutAccount")]
        public ActionResult SignOutAccount()
        {
            HttpContext.Session.Clear();
            return Ok(new { Message = "Sign out done" });
        }


        [HttpPut("EditAccount/{id}")]
        public async Task<ActionResult> EditAccount(int id, [FromBody] User editAccount)
        {
            var account = await _context.Users.FindAsync(id);


            if (account == null)
            {
                //returns if not found
                return NotFound($"Account not found");
            }

            account.UserName = editAccount.UserName;
            account.Email = editAccount.Email;
            account.Password = editAccount.Password;
            account.Role = editAccount.Role;

            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(account);
        }
    }

}
