using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Laktaren.Application.Interfaces;
using Laktaren.Domain.Enums;
using Laktaren.Domain.Entities;
using Laktaren.Domain.Contracts;

namespace Laktaren.Infrastructure.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get all Parent Posts
        public async Task<List<PostDto>> GetAllPostsAsync()
        {
            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Reactions).ThenInclude(r => r.Team)
                .OrderByDescending(p => p.CreatedAt)
                .Where(p => p.ParentPostId == null && !p.IsClubHouseOnly)
                .ToListAsync();
                
            return posts.Select(p => new PostDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Content = p.Content,
                IsDeleted = p.IsDeleted,
                Author = p.Author,
                ReplyCount = p.ReplyCount,
                ParentPostId = p.ParentPostId,
                IsClubHouseOnly = p.IsClubHouseOnly,
                TargetTeamId = p.TargetTeamId,
                ParentPost = p.ParentPost,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Reactions = new ReactionsDto
                {
                    LikeCount = p.Reactions.Count(r => r.Type == ReactionType.Like),
                    BooCount = p.Reactions.Count(r => r.Type == ReactionType.Boo),
                    LikesPerTeam = p.Reactions
                        .Where(r => r.Type == ReactionType.Like && r.Team != null)
                        .GroupBy(r => r.Team.Name)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    BoosPerTeam = p.Reactions
                        .Where(r => r.Type == ReactionType.Boo && r.Team != null)
                        .GroupBy(r => r.Team.Name)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    UserReaction = p.Reactions.FirstOrDefault(r => r.UserId == currentUserId)?.Type
                }
            }).ToList();
        }

        //Get all parent posts for a specific user, including club house posts if the user is part of the target team
        public async Task<List<PostDto>> GetPostsForUserIdAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Reactions).ThenInclude(r => r.Team)
                .OrderByDescending(p => p.CreatedAt)
                .Where(p => p.ParentPostId == null && 
                    (!p.IsClubHouseOnly || (p.IsClubHouseOnly && p.TargetTeamId == user.TeamId)))
                .ToListAsync();

            return posts.Select(p => new PostDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Content = p.Content,
                IsDeleted = p.IsDeleted,
                Author = p.Author,
                ReplyCount = p.ReplyCount,
                ParentPostId = p.ParentPostId,
                IsClubHouseOnly = p.IsClubHouseOnly,
                TargetTeamId = p.TargetTeamId,
                Reactions = new ReactionsDto
                {
                    LikeCount = p.Reactions.Count(r => r.Type == ReactionType.Like),
                    BooCount = p.Reactions.Count(r => r.Type == ReactionType.Boo),
                    LikesPerTeam = p.Reactions
                        .Where(r => r.Type == ReactionType.Like && r.Team != null)
                        .GroupBy(r => r.Team.Name)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    BoosPerTeam = p.Reactions
                        .Where(r => r.Type == ReactionType.Boo && r.Team != null)
                        .GroupBy(r => r.Team.Name)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    UserReaction = p.Reactions.FirstOrDefault(r => r.UserId == currentUserId)?.Type
                }
            }).ToList();
        }

        //Get all replies for a specific post
        public async Task<List<PostDto>> GetRepliesAsync(Guid postId)
        {
            var replies =  await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Reactions).ThenInclude(r => r.Team)
                .OrderByDescending(p => p.CreatedAt)
                .Where(p => p.ParentPostId == postId)
                .ToListAsync();
            
            return replies.Select(p => new PostDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Content = p.Content,
                IsDeleted = p.IsDeleted,
                Author = p.Author,
                ReplyCount = p.ReplyCount,
                ParentPostId = p.ParentPostId,
                IsClubHouseOnly = p.IsClubHouseOnly,
                TargetTeamId = p.TargetTeamId,
                ParentPost = p.ParentPost,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Reactions = new ReactionsDto
                {
                    LikeCount = p.Reactions.Count(r => r.Type == ReactionType.Like),
                    BooCount = p.Reactions.Count(r => r.Type == ReactionType.Boo),
                    LikesPerTeam = p.Reactions
                        .Where(r => r.Type == ReactionType.Like && r.Team != null)
                        .GroupBy(r => r.Team.Name)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    BoosPerTeam = p.Reactions
                        .Where(r => r.Type == ReactionType.Boo && r.Team != null)
                        .GroupBy(r => r.Team.Name)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    UserReaction = p.Reactions.FirstOrDefault(r => r.UserId == currentUserId)?.Type
                }
            }).ToList();
        }

        public async Task<PostDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }
            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Reactions).ThenInclude(r => r.Team)
                .FirstOrDefaultAsync(p => p.Id == id);

            return post != null ? new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                Content = post.Content,
                IsDeleted = post.IsDeleted,
                Author = post.Author,
                ReplyCount = post.ReplyCount,
                ParentPostId = post.ParentPostId,
                IsClubHouseOnly = post.IsClubHouseOnly,
                TargetTeamId = post.TargetTeamId,
                ParentPost = post.ParentPost,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                Reactions = new ReactionsDto
                {
                    LikeCount = post.Reactions.Count(r => r.Type == ReactionType.Like),
                    BooCount = post.Reactions.Count(r => r.Type == ReactionType.Boo),
                    LikesPerTeam = post.Reactions
                        .Where(r => r.Type == ReactionType.Like && r.Team != null)
                        .GroupBy(r => r.Team.Name)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    BoosPerTeam = post.Reactions
                        .Where(r => r.Type == ReactionType.Boo && r.Team != null)
                        .GroupBy(r => r.Team.Name)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    UserReaction = post.Reactions.FirstOrDefault(r => r.UserId == currentUserId)?.Type
                }
            } : null;
        }

        public async Task<List<PostDto>> GetPostsByUserIdAsync(Guid userId)
        {
            var posts = await _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.Reactions)
                    .ThenInclude(r => r.Team)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync() ?? new List<Post>();

            return posts.Select(p => new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                Content = post.Content,
                IsDeleted = post.IsDeleted,
                Author = post.Author,
                ReplyCount = post.ReplyCount,
                ParentPostId = post.ParentPostId,
                IsClubHouseOnly = post.IsClubHouseOnly,
                TargetTeamId = post.TargetTeamId,
                ParentPost = post.ParentPost,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                Reactions = new ReactionsDto
                {
                    LikeCount = p.Reactions.Count(r => r.Type == ReactionType.Like),
                    BooCount = p.Reactions.Count(r => r.Type == ReactionType.Boo),
                    LikesPerTeam = p.Reactions
                        .Where(r => r.Type == ReactionType.Like && r.Team != null)
                        .GroupBy(r => r.Team.Name)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    BoosPerTeam = p.Reactions
                        .Where(r => r.Type == ReactionType.Boo && r.Team != null)
                        .GroupBy(r => r.Team.Name)
                        .ToDictionary(g => g.Key, g => g.Count()),
                    UserReaction = p.Reactions.FirstOrDefault(r => r.UserId == currentUserId)?.Type
                }
            }).ToList();
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            if (post.ParentPostId.HasValue)
            {
                var parent = await _context.Posts.FindAsync(post.ParentPostId);
                if (parent != null)
                {
                    parent.ReplyCount++; 
                    await _context.SaveChangesAsync();
                }
            }
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();


            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Reactions)
                    .ThenInclude(r => r.Team)
                .FirstOrDefaultAsync(p => p.Id == post.Id);
        }

        public async Task<Post?> DeletePostAsync(Guid id)
{
            var post = await _context.Posts
            .Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == id);
            
            if (post == null) return null;

            post.IsDeleted = true;

            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<Post> UpdatePostAsync(Post post)
        {
            if (post.Id == Guid.Empty)
            {
                post.Id = Guid.NewGuid();
                await _context.Posts.AddAsync(post);
            }
            else
            {
                _context.Posts.Update(post);
            }
            await _context.SaveChangesAsync();
            return post;
        }
    }

}
