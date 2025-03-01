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
            var address = new Address
            {
                customerName = null,
                city = null,
                street = null,
                state = null,
                postalCode = null,
                country = null
            };
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            string EncryptPass = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = EncryptPass,
                Role = "Customer",
                addressId = address.id,
                Cards = new List<Card>(),
                Carts = new List<Cart>(),
                Orders = new List<Order>()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var Card = new Card
            {
                UserId = user.Id,
                CreditCardNumber = "1234567890123456",
                ExpirationDate = DateTime.UtcNow,
                CVV = "123"
            };

            _context.Cards.Add(Card);
            await _context.SaveChangesAsync();
            return Ok(user);


        }

        [HttpPost("SignInAccount")]
        public async Task<ActionResult> SignInAccount([FromBody] SignInModel request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized("Invalid Sign in.");
            }

            bool encryptPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);


            if (!encryptPassword)
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

        [HttpPut("EditAddress/{id}")]
        public async Task<ActionResult> EditAddress(int id, [FromBody] Address editAddress)
        {
            var address = await _context.Addresses.FindAsync(id);


            if (address == null)
            {
                //returns if not found
                return NotFound($"Address not found");
            }

            address.customerName = editAddress.customerName;
            address.city = editAddress.city;
            address.street = editAddress.street;
            address.state = editAddress.state;
            address.postalCode = editAddress.postalCode;
            address.country = editAddress.country;


            _context.Entry(address).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(address);
        }
    }

}
