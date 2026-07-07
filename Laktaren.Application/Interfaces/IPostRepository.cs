using Laktaren.Domain.Contracts;
using Laktaren.Domain.Entities;
using Laktaren.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Laktaren.Application.Interfaces
{
    public interface IPostRepository
    {
        Task<List<PostDto>> GetAllPostsAsync();
        Task<List<PostDto>> GetPostsForUserIdAsync(Guid userId);
        Task<List<PostDto>> GetRepliesAsync(Guid postId, Guid currentUserId);
        Task<PostDto?> GetByIdAsync(Guid id, Guid currentUserId);
        Task<List<PostDto>> GetPostsByUserIdAsync(Guid userId, Guid currentUserId);
        Task<Post> CreatePostAsync(Post post);
        Task<Post> UpdatePostAsync(Post post);
        Task<Post?> DeletePostAsync(Guid id);
    }
}
