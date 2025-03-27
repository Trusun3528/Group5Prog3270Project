namespace Group5.src.Presentaion.Tests;
using Group5.src.domain.models;
using Group5.src.infrastructure;
using Group5.src.Presentaion.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestClass]
public class AccountControllerTests
{
    private DbContextOptions<Group5DbContext> _dbContextOptions;
    private Group5DbContext _context;
    private IConfiguration _config;
    private ILogger<AccountController> _logger;
    private AccountController _controller;

    [TestInitialize]
    public void Setup()
    {
        _dbContextOptions = new DbContextOptionsBuilder<Group5DbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new Group5DbContext(_dbContextOptions);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
        {
            { "Jwt:Key", "UdqytHqwif2VWb7iKp9EC4GSt0onIyPe" }
        }).Build();
        _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<AccountController>();
        _controller = new AccountController(_context, _config, _logger);
    }

    [TestMethod]
    public async Task CreateAccount_ShouldReturnOk()
    {
        var request = new CreateUserModel
        {
            UserName = "Austin",
            Email = "acameron1391@conestogac.on.ca",
            Password = "AdminPassword123"
        };

        var result = await _controller.CreateAccount(request) as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
    }

    [TestMethod]
    public async Task SignInAccount_ShouldReturnOk()
    {
        var user = new User
        {
            UserName = "testuser",
            Email = "testuser@example.com",
            Password = BCrypt.Net.BCrypt.HashPassword("Test@123"),
            Role = "Customer"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var request = new SignInModel
        {
            Email = "testuser@example.com",
            Password = "Test@123"
        };

        var result = await _controller.SignInAccount(request) as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
    }

    [TestMethod]
    public void SignOutAccount_ShouldReturnOk()
    {
        var result = _controller.SignOutAccount() as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
    }

    [TestMethod]
    public async Task EditAccount_ShouldReturnOk()
    {
        var user = new User
        {
            UserName = "testuser",
            Email = "testuser@example.com",
            Password = "Test@123",
            Role = "Customer"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var editAccount = new User
        {
            UserName = "updateduser",
            Email = "updateduser@example.com"
        };

        var result = await _controller.EditAccount(user.Id, editAccount) as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
    }

    [TestMethod]
    public async Task EditAddress_ShouldReturnOk()
    {
        var address = new Address
        {
            customerName = "testuser",
            city = "Test City",
            street = "Test Street",
            state = "Test State",
            postalCode = "12345",
            country = "Test Country"
        };
        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();

        var editAddress = new Address
        {
            city = "Updated City"
        };

        var result = await _controller.EditAddress(address.id, editAddress) as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
    }

    [TestMethod]
    public async Task AllUsers_ShouldReturnOk()
    {
        var result = await _controller.AllUsers() as OkObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
    }

    [TestMethod]
    public async Task GetUser_ShouldReturnOk()
    {
        var user = new User
        {
            UserName = "testuser",
            Email = "testuser@example.com",
            Password = "Test@123",
            Role = "Customer"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _controller.GetUser(user.Id) as ActionResult<User>;

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        Assert.AreEqual(200, ((OkObjectResult)result.Result).StatusCode);
    }

    [TestMethod]
    public async Task SignInAccount_ShouldReturnUnauthorized_WhenUserNotFound()
    {
        var request = new SignInModel
        {
            Email = "nonexistentuser@example.com",
            Password = "WrongPassword"
        };

        var result = await _controller.SignInAccount(request) as UnauthorizedObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.AreEqual("Invalid email or password", result.Value);
    }

    [TestMethod]
    public async Task SignInAccount_ShouldReturnUnauthorized_WhenPasswordIsIncorrect()
    {
        var user = new User
        {
            UserName = "testuser",
            Email = "testuser@example.com",
            Password = BCrypt.Net.BCrypt.HashPassword("CorrectPassword"),
            Role = "Customer"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var request = new SignInModel
        {
            Email = "testuser@example.com",
            Password = "WrongPassword"
        };

        var result = await _controller.SignInAccount(request) as UnauthorizedObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.AreEqual("Invalid email or password", result.Value);
    }

    [TestMethod]
    public async Task EditAddress_ShouldReturnNotFound_WhenAddressNotFound()
    {
        var editAddress = new Address
        {
            city = "Updated City"
        };

        var result = await _controller.EditAddress(999, editAddress) as NotFoundObjectResult;

        Assert.IsNotNull(result);
        Assert.AreEqual(404, result.StatusCode);
        Assert.AreEqual("Address not found", result.Value);
    }

}
