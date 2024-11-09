using Flashify_b.DTOs;
using Flashify_b.Models;
using Flashify_b.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flashify_b.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashCardController : ControllerBase
    {
        private readonly FlashCardService _flashCardService;

        public FlashCardController(FlashCardService flashCardService)
        {
            _flashCardService = flashCardService;
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
    }
}
