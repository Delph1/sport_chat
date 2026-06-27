using Laktaren.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Laktaren.Application.Interfaces;

namespace Laktaren.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;

        TeamsController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Team>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTeamsAsync()
        {
            var teams = _teamRepository.GetAllTeamsAsync();
            return Ok(teams);
        }
    }
}
