using Laktaren.Application.Interfaces;
using Laktaren.Domain.Entities;
using Laktaren.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Laktaren.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;

        public TeamsController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Team>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTeamsAsync()
        {
            var teams = await _teamRepository.GetAllTeamsAsync();
            return Ok(teams);
        }

        [Authorize]
        [HttpPost]
        [Route("{teamId}/follow")]
        public async Task<IActionResult> FollowTeamAsync(Guid teamId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            await _teamRepository.FollowTeamAsync(userId, teamId);
            return Ok(new { Message = "Du följer nu laget" });
        }

    }
}
