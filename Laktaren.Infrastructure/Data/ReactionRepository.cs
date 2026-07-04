using Microsoft.EntityFrameworkCore; // Krävs för .ToListAsync() och .FirstOrDefaultAsync()
using Laktaren.Application.Interfaces;
using Laktaren.Domain.Entities;
using Laktaren.Domain.Enums;

namespace Laktaren.Infrastructure.Data
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly ApplicationDbContext _context;

        public ReactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReactionDto> GetReactionsByPostIdAsync(Guid postId)
        {
            var reactions = await _context.Reactions
                .Include(r => r.User)
                    .ThenInclude(u => u.FavoriteTeam) // Nu kommer vi åt Team-namnet!
                .Where(r => r.PostId == postId)
                .ToListAsync();

            return new ReactionStatsDto
            {
                LikeCount = reactions.Count(r => r.Type == ReactionType.Like),
                BooCount = reactions.Count(r => r.Type == ReactionType.Boo),
                LikesPerTeam = reactions
                    .Where(r => r.Type == ReactionType.Like && r.User.FavoriteTeam != null)
                    .GroupBy(r => r.User.FavoriteTeam!.Name) // Här har vi nu namnet
                    .ToDictionary(g => g.Key, g => g.Count()),
                BoosPerTeam = reactions
                    .Where(r => r.Type == ReactionType.Boo && r.User.FavoriteTeam != null)
                    .GroupBy(r => r.User.FavoriteTeam!.Name)
                    .ToDictionary(g => g.Key, g => g.Count())
            };
        }

        public async Task<ReactionDto> ToggleReactionAsync(Reaction reaction)
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

            var reactions = await _context.Reactions
                .Include(r => r.User)
                    .ThenInclude(u => u.FavoriteTeam) // Nu kommer vi åt Team-namnet!
                .Where(r => r.PostId == postId)
                .ToListAsync();

            return new ReactionStatsDto
            {
                LikeCount = reactions.Count(r => r.Type == ReactionType.Like),
                BooCount = reactions.Count(r => r.Type == ReactionType.Boo),
                LikesPerTeam = reactions
                    .Where(r => r.Type == ReactionType.Like && r.User.FavoriteTeam != null)
                    .GroupBy(r => r.User.FavoriteTeam!.Name) // Här har vi nu namnet
                    .ToDictionary(g => g.Key, g => g.Count()),
                BoosPerTeam = reactions
                    .Where(r => r.Type == ReactionType.Boo && r.User.FavoriteTeam != null)
                    .GroupBy(r => r.User.FavoriteTeam!.Name)
                    .ToDictionary(g => g.Key, g => g.Count())
            };
        }
    }
}