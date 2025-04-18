﻿using Group5.src.domain.models;
using Group5.src.infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;

namespace Group5.src.Presentaion.Controllers
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

                //gets conversation history from session
                var conversationHistory = HttpContext.Session.GetString("ConversationHistory") ?? string.Empty;

                //adds the user prompt to the conversation history
                conversationHistory += $"\nUser: {userPrompt.Prompt}";
                //get the wanted words from the users prompt
                var promptKeywords = getWantedWords(userPrompt.Prompt, conversationHistory);


                //Search the database for the products
                var allProducts = await _dbContext.Products.ToListAsync();
                var allCategories = await _dbContext.Categories.ToListAsync();

                //Filter the products based on the prompt
                var relevantProducts = allProducts
                    .Where(p => promptKeywords.Any(keyword =>
                                p.ProductName?.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                p.ProductDescription?.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();

                //Format the product for the Ais response
                var productData = relevantProducts.Any()
                    ? string.Join("\n", relevantProducts.Select(p =>
                    {
                        // Find the matching category name based on CategoryId
                        var categoryName = allCategories.FirstOrDefault(c => c.Id == p.CatagoryId)?.CategoryName ?? "Unknown";
                        return $"Name: {p.ProductName}, Price: {p.Price:C}, Description: {p.ProductDescription}, Stock: {p.Stock}, Category: {categoryName}, Image: {p.ImageURL}";
                    }))
                    : "No Matching Product";


                //Telling the ai who it is and what it should do
                var assistantRole = "You are an assistant named UselessProductsAi designed to help users learn more about products. Provide clear, and helpful information using the product database, use dollars for any money. Be concise and speak like a human. Only give responses that are relevent to the products, if the user asks a question that is not relevent tell them what you are for";

                //Gets the final prompt to give the ai
                var FinalPrompt = $"{assistantRole}\n\nConversation History:\n{conversationHistory}\n\nUser Query: {userPrompt.Prompt}\n\nAvailable Products:\n{productData} \n Respond using all the history into account including product information if it aligns for what the user just asked for, and take into consideration what product the user is talking about in their history, dont make up products";

                //Gets the respons from the ai
                var aiResponse = await _chatService.GetChatMessageContentAsync(FinalPrompt);

                conversationHistory += $"\nAI: {aiResponse.Content}";
                HttpContext.Session.SetString("ConversationHistory", conversationHistory);
                Console.WriteLine("Session updated with conversation history." + conversationHistory);

                Console.WriteLine("Final Prompt Sent to AI:");
                Console.WriteLine(FinalPrompt);
                return Ok(new { Response = aiResponse.Content });


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Will clear the session of the conversation history
        [HttpPost("ClearSession")]
        public IActionResult ClearSession()
        {
            HttpContext.Session.Clear();
            return Ok();
        }

        private List<string> getWantedWords(string prompt, string conversationHistory)
        {
            //Tries to filter out words to isolate the wanted words
            var unWantedWords = new[] { "tell", "me", "about", "the", "a", "an", "is", "what", "are" };
            var wantedWords = prompt
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(word => !unWantedWords.Contains(word.ToLower()))
                .ToList();
            //if there is not conversation history and the wanted words count is less than 2 it will not isolate the wanted words
            if (string.IsNullOrEmpty(conversationHistory) && wantedWords.Count < 2)
            {
                return prompt.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            return wantedWords;
        }
    }

}

