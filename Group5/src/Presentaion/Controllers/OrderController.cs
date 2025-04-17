using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Group5.src.Presentaion.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {

        private readonly Group5DbContext _context;
        private readonly ILogger<ProductController> _logger;
        public OrderController(Group5DbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetOrders")]
        public async Task<ActionResult<IEnumerable<Product>>> GetOrders()
        {
            _logger.LogInformation("Entering GetOrders");
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .ToListAsync();
            _logger.LogInformation("Got Orders");
            return Ok(orders);
        }
    }
}
