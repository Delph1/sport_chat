using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Laktaren.Application.Interfaces;
using Laktaren.Domain.Entities;

namespace Laktaren.Infrastructure.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.Reactions)
                    .ThenInclude(r => r.Team)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Post> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }
            return await _context.Posts
                .Include(p => p.Reactions)
                    .ThenInclude(r => r.Team)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task<List<Post>> GetPostsByUserIdAsync(Guid userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.Reactions)
                    .ThenInclude(r => r.Team)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync() ?? new List<Post>();
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }
        public async Task<bool> DeletePostAsync(Post post)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
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
