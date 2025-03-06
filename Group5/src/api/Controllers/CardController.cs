/*
 * Project: Open Source Web Programing Midterm Check-In
 * Group Number: 5
 * Group Members: Patrick Harte, Austin Casselman, Austin Cameron, Leif Johannesson
 * Revision History:
 *      Created: January 25th, 2025
 *      Submitted: March 6th, 2025
 */
using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group5.src.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : Controller
    {
        private readonly Group5DbContext _context;
        private readonly ILogger<ProductController> _logger;

        public CardController(Group5DbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        [HttpPut("EditCard/{id}")]
        public async Task<ActionResult> EditCard(int id, [FromBody] Card editCard)
        {
            _logger.LogInformation("EditCard starts");
            var card = await _context.Cards.FindAsync(id);


            if (card == null)
            {
                //returns if not found
                return NotFound($"Address not found");
            }
            //edits card
            if (!string.IsNullOrEmpty(editCard.CreditCardNumber))
            {
                card.CreditCardNumber = editCard.CreditCardNumber;
            }
            if (editCard.ExpirationDate != default(DateTime))
            {
                card.ExpirationDate = editCard.ExpirationDate;
            }
            if (!string.IsNullOrEmpty(editCard.CVV))
            {
                card.CVV = editCard.CVV;
            }

            //save
            _context.Entry(card).State = EntityState.Modified;
            _logger.LogInformation("EditCard ends");
            await _context.SaveChangesAsync();
            return Ok(card);
        }

        [Authorize]
        [HttpGet("GetCard/{id}")]
        public async Task<ActionResult<Card>> GetCard(int id)
        {
            _logger.LogInformation("GetCard starts");
            //finds the card
            var card = await _context.Cards.FindAsync(id);

            if (card == null)
            {
                _logger.LogWarning($"Card with id {id} not found");
                //returns if cannot find the card
                return NotFound($"Card with id {id} not found");
            }
            //returns the card
            _logger.LogInformation("GetCard ends");
            return Ok(card);
        }
    }
}
