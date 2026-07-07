using Microsoft.EntityFrameworkCore; // Krävs för .ToListAsync() och .FirstOrDefaultAsync()
using Laktaren.Application.Interfaces;
using Laktaren.Domain.Entities;
using Laktaren.Domain.Enums;
using Laktaren.Domain.Contracts;

namespace Laktaren.Infrastructure.Data
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly ApplicationDbContext _context;

        public ReactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReactionsDto> GetReactionsByPostIdAsync(Guid postId, Guid currentUserId)
        {
            var reactions = await _context.Reactions
                .Include(r => r.User)
                    .ThenInclude(u => u.FavoriteTeam)
                .Where(r => r.PostId == postId)
                .ToListAsync();

            return new ReactionsDto
            {
                LikeCount = reactions.Count(r => r.Type == ReactionType.Like),
                BooCount = reactions.Count(r => r.Type == ReactionType.Boo),
                LikesPerTeam = reactions
                    .Where(r => r.Type == ReactionType.Like && r.User.FavoriteTeam != null)
                    .GroupBy(r => r.User.FavoriteTeam!.Name)
                    .ToDictionary(g => g.Key, g => g.Count()),
                BoosPerTeam = reactions
                    .Where(r => r.Type == ReactionType.Boo && r.User.FavoriteTeam != null)
                    .GroupBy(r => r.User.FavoriteTeam!.Name)
                    .ToDictionary(g => g.Key, g => g.Count()),
                UserReaction = reactions.FirstOrDefault(r => r.UserId == currentUserId)?.Type
            };
        }

        public async Task<ReactionsDto> ToggleReactionAsync(Reaction reaction)
        {
            var currentUserId = reaction.UserId;

            var existingReaction = await _context.Reactions
                .FirstOrDefaultAsync(r => r.UserId == reaction.UserId && r.PostId == reaction.PostId);

            if (existingReaction != null)
            {
               if (existingReaction.Type == reaction.Type)
                {
                    _context.Reactions.Remove(existingReaction);
                }
                else
                {
                    existingReaction.Type = reaction.Type;
                }
            }

            await _context.SaveChangesAsync();

            var reactions = await _context.Reactions
                .Include(r => r.User)
                    .ThenInclude(u => u.FavoriteTeam)
                .Where(r => r.PostId == reaction.PostId)
                .ToListAsync();

            return new ReactionsDto
            {
                LikeCount = reactions.Count(r => r.Type == ReactionType.Like),
                BooCount = reactions.Count(r => r.Type == ReactionType.Boo),
                LikesPerTeam = reactions
                    .Where(r => r.Type == ReactionType.Like && r.User.FavoriteTeam != null)
                    .GroupBy(r => r.User.FavoriteTeam!.Name)
                    .ToDictionary(g => g.Key, g => g.Count()),
                BoosPerTeam = reactions
                    .Where(r => r.Type == ReactionType.Boo && r.User.FavoriteTeam != null)
                    .GroupBy(r => r.User.FavoriteTeam!.Name)
                    .ToDictionary(g => g.Key, g => g.Count()),
                UserReaction = reactions.FirstOrDefault(r => r.UserId == currentUserId)?.Type
            };
        }
    }
}