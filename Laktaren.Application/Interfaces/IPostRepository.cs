using Laktaren.Domain.Entities;
using Laktaren.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Laktaren.Application.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllPostsAsync();
        Task<Post> GetByIdAsync(Guid id);
        Task<List<Post>> GetPostsByUserIdAsync(Guid userId);
        Task<Post> CreatePostAsync(Post post);
        Task<Post> UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(Post post);
    }
}
