using Flashify_b.DTOs;
using Flashify_b.Models;
using Flashify_b.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flashify_b.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashCardController : ControllerBase
    {
        private readonly FlashCardService _flashCardService;
        private readonly FlashCardAnswerService _answerService;

        public FlashCardController(FlashCardService flashCardService, FlashCardAnswerService flashCardAnswerService)
        {
            _flashCardService = flashCardService;
            _answerService = flashCardAnswerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlashCard([FromBody] FlashCardDTO flashCardDto)
        {
            if (flashCardDto == null || flashCardDto.UserId <= 0 || string.IsNullOrEmpty(flashCardDto.Category))
            {
                return BadRequest("Invalid flashcard data.");
            }

            try
            {
                var createdFlashCard = await _flashCardService.CreateFlashCardAsync(flashCardDto);
                return CreatedAtAction(nameof(GetFlashCardsByUserAndCategory),
                    new { userId = createdFlashCard.UserId, category = createdFlashCard.Category },
                    createdFlashCard);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{userId}/{category}")]
        public async Task<ActionResult<List<FlashCard>>> GetFlashCardsByUserAndCategory(int userId, string category)
        {
            var flashCards = await _flashCardService.GetFlashCardsByUserAndCategoryAsync(userId, category);

            if (flashCards == null || flashCards.Count == 0)
            {
                return NotFound("No flashcards found for the specified user and category.");
            }

            return Ok(flashCards);
        }


        [HttpPost("validateAnswer/{flashCardId}")]
        public async Task<IActionResult> ValidateAnswer(int flashCardId, [FromBody] string userAnswer)
        {
            if (string.IsNullOrEmpty(userAnswer))
            {
                return BadRequest("User answer is required.");
            }

            var flashCard = await _flashCardService.GetFlashCardByIdAsync(flashCardId);

            if (flashCard == null)
            {
                return NotFound("Flashcard not found.");
            }

            bool isCorrect = await _answerService.ValidateAnswerAsync(userAnswer, flashCard.Answer);

            return Ok(new { isCorrect });
        }

        [HttpGet("categories/{userId}")]
        public async Task<IActionResult> GetCategoriesByUserId(int userId)
        {
            var categories = await _flashCardService.GetCategoriesByUserIdAsync(userId);

            if (categories == null || categories.Count == 0)
            {
                return NotFound("No categories found for this user.");
            }

            return Ok(categories);
        }


    }
}
