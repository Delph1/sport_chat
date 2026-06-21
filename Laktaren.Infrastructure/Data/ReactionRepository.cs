using Microsoft.EntityFrameworkCore; // Krävs för .ToListAsync() och .FirstOrDefaultAsync()
using Laktaren.Application.Interfaces;
using Laktaren.Domain.Entities;

namespace Laktaren.Infrastructure.Data
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly ApplicationDbContext _context;

        public ReactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reaction>> GetReactionsByPostIdAsync(Guid postId)
        {
            return await _context.Reactions
                .Where(r => r.PostId == postId)
                .ToListAsync();
        }

        public async Task<bool> ToggleReactionAsync(Reaction reaction)
        {
            var existingReaction = await _context.Reactions
                .FirstOrDefaultAsync(r => r.UserId == reaction.UserId && r.PostId == reaction.PostId);

            if (existingReaction != null)
            {
                _context.Reactions.Remove(existingReaction);
                await _context.SaveChangesAsync();
                return false;
            }
            else
            {
                await _context.Reactions.AddAsync(reaction);
                await _context.SaveChangesAsync();
                return true; 
            }
        }
    }
}