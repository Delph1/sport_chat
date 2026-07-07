using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Laktaren.Application.Interfaces;
using Laktaren.Domain.Entities;
using Laktaren.Domain.Enums;
using Laktaren.Domain.Contracts;

namespace Laktaren.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReactionsController : ControllerBase
    {
        private readonly IReactionRepository _reactionRepository;

        public ReactionsController(IReactionRepository reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }

        [HttpGet("{postId}")]
        [ProducesResponseType(typeof(ReactionsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReactionsByPostIdAsync(Guid postId)
        {
            if (postId == Guid.Empty)
            {
                return BadRequest("Inget giltigt inlägg angavs.");
            }
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var reactions = await _reactionRepository.GetReactionsByPostIdAsync(postId, userIdString != null && Guid.TryParse(userIdString, out Guid userId) ? userId : Guid.Empty);
            
            return Ok(reactions);
        }

        [Authorize]
        [HttpPost("{postId}")]
        [ProducesResponseType(typeof(ReactionsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ToggleReactionAsync(Guid postId, ReactionType reactionType)
        {
            if (postId == Guid.Empty)
            {
                return BadRequest("Inget giltigt inlägg angavs.");
            }

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized("Din biljett är ogiltig. Logga in igen.");
            }

            var reaction = new Reaction
            {
                PostId = postId,
                UserId = userId,
                Type = reactionType
            };

            var reactions = await _reactionRepository.ToggleReactionAsync(reaction);

            if (reactions != null)
            {
                return Ok(reactions);
            }
            else
            {
                return NotFound("Reaktionen kunde inte hittas eller togs bort.");
            }
        }
    }
}