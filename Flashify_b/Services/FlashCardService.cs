using Flashify_b.Data;
using Flashify_b.DTOs;
using Flashify_b.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flashify_b.Services
{
    public class FlashCardService
    {
        private readonly ApplicationDbContext _context;

        public FlashCardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FlashCard> CreateFlashCardAsync(FlashCardDTO flashCardDto)
        {
            var user = await _context.Users.FindAsync(flashCardDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var flashCard = new FlashCard
            {
                Category = flashCardDto.Category,
                Question = flashCardDto.Question,
                Answer = flashCardDto.Answer,
                UserId = flashCardDto.UserId,
                User = user 
            };

            _context.FlashCards.Add(flashCard);
            await _context.SaveChangesAsync();
            return flashCard;
        }

        public async Task<List<FlashCard>> GetFlashCardsByUserAndCategoryAsync(int userId, string category)
        {
            return await _context.FlashCards
                .Where(fc => fc.UserId == userId && fc.Category == category)
                .ToListAsync();
        }
    }
}
