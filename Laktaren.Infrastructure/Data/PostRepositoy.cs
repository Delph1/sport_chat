using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Laktaren.Application.Interfaces;
using Laktaren.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Laktaren.Infrastructure.Data
{
    public class PostRepositoy : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public async Task<List<Post>> GetAllPostsAsync()
        {
            List<Post> posts = await _context.Posts.ToListAsync();
            return posts;
        }

        public async Task<Post> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }
            return await _context.Posts.FindAsync(id);
        }
        
        public async Task<List<Post>> GetPostsByUserIdAsync(Guid userId)
        {
            List<Post> posts = await _context.Posts.Where(p => p.UserId == userId).ToListAsync() ?? [];
            return posts;
        }

        public async Task<IActionResult> CreatePostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return new CreatedAtActionResult(nameof(GetByIdAsync), "Posts", new { id = post.Id }, post);
        }
        public async Task<IActionResult> DeletePostAsync(Post post)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return new CreatedAtActionResult(nameof(DeletePostAsync), "Posts", null, null);
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
