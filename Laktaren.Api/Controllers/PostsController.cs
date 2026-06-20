using Microsoft.AspNetCore.Mvc;
using Laktaren.Domain.Entities;
using Laktaren.Application.Interfaces;

namespace Laktaren.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {

        private readonly IPostRepository _postRepository;

        [HttpGet]
        [ProducesResponseType(typeof(List<Post>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _postRepository.GetAllPostsAsync();
        }

        [HttpGet("id")]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Post> GetByIdAsync(Guid id)
        {
            Post? post = await _postRepository.GetByIdAsync(id);
            return post;
        }

        [HttpGet("userid")]
        [ProducesResponseType(typeof(List<Post>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public List<Post> GetPostsByUserIdAsync (Guid userId) 
        {
            if (userId == Guid.Empty)
            {
                return [];
            }
            List<Post> posts = _postRepository.GetPostsByUserIdAsync(userId).Result ?? [];
            return posts;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreatePostAsync(Post post)
        {
            IActionResult result = await _postRepository.CreatePostAsync(post);
            return (ActionResult)result;
        }

        [HttpPut("id")]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdatePostAsync(Guid id, Post post)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Post ID is required.");
            }

            if (id != post.Id)
            {
                return BadRequest("Post ID mismatch.");
            }

            if (post == null)
            {
                return BadRequest("Post data is required.");
            }
            Post updatedPost = _postRepository.UpdatePostAsync(post).Result;
            return Ok(updatedPost);
        }

        [HttpDelete("id")]
        public IActionResult DeletePostAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Post ID is required.");
            }
            IActionResult result = _postRepository.DeletePostAsync(new Post { Id = id }).Result;
            return result;

        }
    }
}
