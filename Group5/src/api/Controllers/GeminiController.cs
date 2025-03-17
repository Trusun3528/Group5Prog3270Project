using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;

namespace Group5.src.api.Controllers
{
    [ApiController]
    [Route("Gemini")]
    public class GeminiController : Controller
    {
        private readonly IChatCompletionService _chatService;
        private readonly Group5DbContext _dbContext;

        public GeminiController(Group5DbContext dbContext)
        {
            _dbContext = dbContext;
            #pragma warning disable SKEXP0070 //this is for the chat GoogleAIGeminiChatCompletionService, it would not stop complainging becuase its in beta
            _chatService = new GoogleAIGeminiChatCompletionService("gemini-2.0-flash", "AIzaSyCUPJ-Dw3rvEACGVRAzJ0jhb32IsyHDGEU");
        }

        [HttpPost("Chat")]
        public async Task<IActionResult> GetChatResponse([FromBody] UserPrompt userPrompt)
        {
            //checks to see if the prompts is empty, if so it will return a bad request
            if (string.IsNullOrEmpty(userPrompt?.Prompt))
            {
                return BadRequest("Prompt cannot be empty.");
            }

            try
            {
                //get the wanted words from the users prompt
                var promptKeywords = getWantedWords(userPrompt.Prompt);

                //Search the database for the products
                var allProducts = await _dbContext.Products.ToListAsync();

                //Filter the products based on the prompt
                var relevantProducts = allProducts
                    .Where(p => promptKeywords.Any(keyword =>
                                (p.ProductName?.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                (p.Catagory?.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                (p.ProductDescription?.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)))
                    .ToList();

                //Format the product for the Ais response
                var productData = relevantProducts.Any()
                    ? string.Join("\n", relevantProducts.Select(p =>
                        $"Name: {p.ProductName}, Price: {p.Price:C}, Description: {p.ProductDescription}, Stock: {p.Stock}, Category: {p.Catagory}, Image: {p.ImageURL}"))
                    : "No matching products found.";

                
                //Telling the ai who it is and what it should do
                var assistantRole = "You are an assistant named Doesn'tExistAI designed to help users learn more about products. Provide clear, and helpful information using the product database. Be concise and speak like a human.";

                //Gets the final prompt to give the ai
                var FinalPrompt = $"{assistantRole}\n\nUser Query: {userPrompt.Prompt}\n\nAvailable Products:\n{productData}";

                //Gets the respons from the ai
                var aiResponse = await _chatService.GetChatMessageContentAsync(FinalPrompt);
                return Ok(new { Response = aiResponse });


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private List<string> getWantedWords(string prompt)
        {
            //Tries to filter out words to isolate the wanted words
            var unWantedWords = new[] { "tell", "me", "about", "the", "a", "an", "is", "what", "are" };
            var wantedWords = prompt
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(word => !unWantedWords.Contains(word.ToLower()))
                .ToList();

            return wantedWords;
        }
    }
    
}

