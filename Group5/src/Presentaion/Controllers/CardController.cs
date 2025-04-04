﻿using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group5.src.Presentaion.Controllers
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
            if (editCard.ExpirationDate != default)
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

        [Authorize]
        [HttpDelete("DeleteCard/{id}")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            _logger.LogInformation("DeleteCard starts");
            var card = await _context.Cards.FindAsync(id);

            if (card == null)
            {
                _logger.LogInformation($"Card with id {id} not found");
                return NotFound($"Card with id {id} not found");
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();

            return Ok("Card removed succesfully");
        }

        [Authorize]
        [HttpPost("CreateCard")]
        public async Task<ActionResult<Card>> CreateCard(Card card)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (card.ExpirationDate <= DateTime.UtcNow)
            {
                return BadRequest("Expiration date must be in the future.");
            }

            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCard), new { id = card.Id }, card);
        }
    }
}
