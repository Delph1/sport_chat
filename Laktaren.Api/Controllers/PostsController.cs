using Laktaren.Application.Interfaces;
using Laktaren.Domain.Entities;
using Laktaren.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Laktaren.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {

        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            Post? post = await _postRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound("Post not found.");
            }
            return Ok(post);
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(List<Post>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostsByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("Ogiltigt användar-ID.");
            }

            List<Post> posts = await _postRepository.GetPostsByUserIdAsync(userId);
            return Ok(posts);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreatePostAsync([FromBody] Post newPost)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Token invalid or missing.");
            }

            newPost.UserId = Guid.Parse(userIdString);

            newPost.UserId = userIdString;
            newPost.CreatedAt = DateTime.UtcNow; 
            newPost.ReplyCount = 0; 
            newPost.IsDeleted = false;

            var result = await _postRepository.CreatePostAsync(newPost);

            if (result != null)
            {
                return CreatedAtAction("GetById", new { id = result.Id }, result);
            }
            else
            {
                // Returnerar HTTP 400 Bad Request om något gick snett
                return BadRequest("Post could not be created.");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePostAsync(Guid id, [FromBody] Post post)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Token invalid or missing.");
            }

            if (id == Guid.Empty || id != post.Id)
            {
                return BadRequest("ID matchar inte inlägget.");
            }

            // 3. Vi använder 'await' istället för '.Result'
            Post updatedPost = await _postRepository.UpdatePostAsync(post);
            return Ok(updatedPost);
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePostAsync(Guid id)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Token invalid or missing.");
            }

            if (id == Guid.Empty)
            {
                return BadRequest("Post ID is required.");
            }
            
            bool result = await _postRepository.DeletePostAsync(new Post { Id = id });

            if (result)
            {
                return NoContent();
            }
            return BadRequest("Unable to delete post.");

        }
    }
}
