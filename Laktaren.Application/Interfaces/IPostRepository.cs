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
        Task <IActionResult> CreatePostAsync(Post post);
        Task<IActionResult> DeletePostAsync(Post post);
        Task <Post>UpdatePostAsync(Post post);
    }
}
