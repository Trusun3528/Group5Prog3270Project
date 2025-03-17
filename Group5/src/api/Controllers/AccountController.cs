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
        private readonly ILogger<AccountController> _logger;



        public AccountController(Group5DbContext context, IConfiguration config, ILogger<AccountController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("CreateAccount")]
        public async Task<ActionResult> CreateAccount([FromBody] CreateUserModel request)
        {//Creates the address for the new account
            _logger.LogInformation("Start account creation");
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
            _logger.LogInformation("address created");
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
            if (!TryValidateModel(user))
            {
                _logger.LogInformation("info does not match the model");
                return BadRequest(ModelState);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("account created");
            //adds a card to the account
            var Card = new Card
            {
                UserId = user.Id,
                CreditCardNumber = "1234567890123456",//fillerdata
                ExpirationDate = DateTime.UtcNow,//utc is required for postgress
                CVV = "123"//fillerdata
            };
            _context.Cards.Add(Card);
            _logger.LogInformation("card created");
            _logger.LogInformation("end account creation and saved");
            await _context.SaveChangesAsync();
            return Ok(user);


        }
        //allows for the user to sign in
        [HttpPost("SignInAccount")]
        public async Task<ActionResult> SignInAccount([FromBody] SignInModel request)
        {
            _logger.LogInformation("sign in started");
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                _logger.LogWarning("Invalid Sign in");
                return Unauthorized("Invalid Sign in");
            }
            //encryps the password for verification
            bool encryptPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);


            if (!encryptPassword)
            {
                _logger.LogWarning("Invalid Sign in");
                return Unauthorized("Invalid Sign in.");
            }

            //creates a token handler
            var tokenHandler = new JwtSecurityTokenHandler();
            //used to sign the token
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
            //creates the token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //writes the token
            var tokenString = tokenHandler.WriteToken(token);
            _logger.LogInformation($"Token created {token}");
            //returns the user, and token if verified
            return Ok(new { Message = "Sign-in successful.", User = user, Token = tokenString });
        }
        //signs out the user
        [HttpPost("SignOutAccount")]
        public ActionResult SignOutAccount()
        {
            _logger.LogInformation("Signout started");
            HttpContext.Session.Clear();
            _logger.LogInformation("Signout end");
            return Ok(new { Message = "Sign out done" });
            
        }

        [Authorize]
        [HttpPut("EditAccount/{id}")]
        public async Task<ActionResult> EditAccount(int id, [FromBody] User editAccount)
        {
            _logger.LogInformation("editaccount start");
            var account = await _context.Users.FindAsync(id);


            if (account == null)
            {
                _logger.LogWarning($"Account not found");
                //returns if not found
                return NotFound($"Account not found");
            }
            //edits the account
            if (!string.IsNullOrEmpty(editAccount.UserName))
            {
                account.UserName = editAccount.UserName;
            }
            if (!string.IsNullOrEmpty(editAccount.Email))
            {
                account.Email = editAccount.Email;
            }
            if (!string.IsNullOrEmpty(editAccount.Password))
            {
                account.Password = editAccount.Password;
            }
            if (!string.IsNullOrEmpty(editAccount.Role))
            {
                account.Role = editAccount.Role;
            }
            //saves
            
            _context.Entry(account).State = EntityState.Modified;
            _logger.LogInformation("editaccount end and saved");
            await _context.SaveChangesAsync();
            return Ok(account);
        }

        [Authorize]
        [HttpPut("EditAddress/{id}")]
        public async Task<ActionResult> EditAddress(int id, [FromBody] Address editAddress)
        {
            _logger.LogInformation("Editaddress start");
            var address = await _context.Addresses.FindAsync(id);


            if (address == null)
            {
                _logger.LogWarning($"Account not found");
                //returns if not found
                return NotFound($"Address not found");
            }
            //edits the address
            if (!string.IsNullOrEmpty(editAddress.customerName))
            {
                address.customerName = editAddress.customerName;
            }
            if (!string.IsNullOrEmpty(editAddress.city))
            {
                address.city = editAddress.city;
            }
            if (!string.IsNullOrEmpty(editAddress.street))
            {
                address.street = editAddress.street;
            }
            if (!string.IsNullOrEmpty(editAddress.state))
            {
                address.state = editAddress.state;
            }
            if (!string.IsNullOrEmpty(editAddress.postalCode))
            {
                address.postalCode = editAddress.postalCode;
            }
            if (!string.IsNullOrEmpty(editAddress.country))
            {
                address.country = editAddress.country;
            }

            //saves
            _context.Entry(address).State = EntityState.Modified;
            _logger.LogInformation("Editaddress end and saved");
            await _context.SaveChangesAsync();
            return Ok(address);
        }

        [Authorize]
        [HttpGet("AllUsers")]
        public async Task<ActionResult> AllUsers()
        {
            _logger.LogInformation($"allusers started");

            var users = await _context.Users.Select(u => new { u.Id, u.Email, u.UserName }).ToListAsync();

            _logger.LogInformation($"allusers end");
            return Ok(users);

        }
        //[Authorize]
        [HttpGet("GetUser/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            _logger.LogInformation($"GetUser started");


            //finds the user
            var user = await _context.Users
            .Include(u => u.Cards)
            .Include(u => u.Carts)
            .Include(u => u.Orders)
            .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                _logger.LogInformation($"User with id {id} not found");
                //returns if cannot find the user
                return NotFound($"User with id {id} not found");
            }

            _logger.LogInformation($"GetUser end");
            //returns the user
            return Ok(user);
        }


    }

}
