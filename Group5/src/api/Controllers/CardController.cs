using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group5.src.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : Controller
    {
        private readonly Group5DbContext _context;

        public CardController(Group5DbContext context)
        {
            _context = context;
        }

        [HttpPut("EditCard/{id}")]
        public async Task<ActionResult> EditCard(int id, [FromBody] Card editCard)
        {
            var card = await _context.Cards.FindAsync(id);


            if (card == null)
            {
                //returns if not found
                return NotFound($"Address not found");
            }
            //edits card
            card.CreditCardNumber = editCard.CreditCardNumber;
            card.ExpirationDate = editCard.ExpirationDate;
            card.CVV= editCard.CVV;

            //save
            _context.Entry(card).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(card);
        }

        [HttpGet("GetCard/{id}")]
        public async Task<ActionResult<Card>> GetCard(int id)
        {
            //finds the card
            var card = await _context.Cards.FindAsync(id);

            if (card == null)
            {
                //returns if cannot find the xard
                return NotFound($"Card with id {id} not found");
            }
            //returns the card
            return Ok(card);
        }
    }
}
