using Microsoft.AspNetCore.Mvc;
using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;
using System.ComponentModel;

namespace Group5.src.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {

        private readonly IConfiguration _config;
        private readonly Group5DbContext _context;



        public AccountController(Group5DbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("CreateAccount")]
        public async Task<ActionResult> CreateAccount([FromBody] CreateUserModel request)
        {//Creates the address for the new account
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
            //creates the account with a encrypted password and adds the newly made address
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
            //adds a card to the account
            var Card = new Card
            {
                UserId = user.Id,
                CreditCardNumber = "1234567890123456",//fillerdata
                ExpirationDate = DateTime.UtcNow,//utc is required for postgress
                CVV = "123"//fillerdata
            };

            _context.Cards.Add(Card);
            await _context.SaveChangesAsync();
            return Ok(user);


        }
        //allows for the user to sign in
        [HttpPost("SignInAccount")]
        public async Task<ActionResult> SignInAccount([FromBody] SignInModel request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized("Invalid Sign in.");
            }
            //encryps the password for verification
            bool encryptPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);


            if (!encryptPassword)
            {
                return Unauthorized("Invalid Sign in.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("UdqytHqwif2VWb7iKp9EC4GSt0onIyPe");

            //creates the token discriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)

            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            //returns the user, and token if verified
            return Ok(new { Message = "Sign-in successful.", User = user, Token = tokenString });
        }
        //signs out the user
        [HttpPost("SignOutAccount")]
        public ActionResult SignOutAccount()
        {
            HttpContext.Session.Clear();
            return Ok(new { Message = "Sign out done" });
        }

        [Authorize]
        [HttpPut("EditAccount/{id}")]
        public async Task<ActionResult> EditAccount(int id, [FromBody] User editAccount)
        {
            var account = await _context.Users.FindAsync(id);


            if (account == null)
            {
                //returns if not found
                return NotFound($"Account not found");
            }
            //edits the account
            account.UserName = editAccount.UserName;
            account.Email = editAccount.Email;
            account.Password = editAccount.Password;
            account.Role = editAccount.Role;
            //saves
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(account);
        }

        [Authorize]
        [HttpPut("EditAddress/{id}")]
        public async Task<ActionResult> EditAddress(int id, [FromBody] Address editAddress)
        {
            var address = await _context.Addresses.FindAsync(id);


            if (address == null)
            {
                //returns if not found
                return NotFound($"Address not found");
            }
            //edits the address
            address.customerName = editAddress.customerName;
            address.city = editAddress.city;
            address.street = editAddress.street;
            address.state = editAddress.state;
            address.postalCode = editAddress.postalCode;
            address.country = editAddress.country;

            //saves
            _context.Entry(address).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(address);
        }

        [Authorize]
        [HttpGet("AllUsers")]
        public async Task<ActionResult> AllUsers()
        {

            var users = await _context.Users.Select(u => new { u.Id, u.Email, u.UserName }).ToListAsync();

            return Ok(users);
        }

    }

}
