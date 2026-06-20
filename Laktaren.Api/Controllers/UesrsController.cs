using Microsoft.AspNetCore.Mvc;
using Laktaren.Domain.Entities;
using Laktaren.Application.Interfaces;

namespace Laktaren.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UesrsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        [HttpGet]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<User?> GetByIdAsync(Guid Id)
        {
            User user = await _userRepository.GetByIdAsync(Id) ?? throw new InvalidOperationException("User not found");
            return user;
        }

        [HttpPost]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateUserAsync(User user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }
            IResult result = (IResult)_userRepository.CreateUserAsync(user);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserAsync(Guid id, User user)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }

            if (id != user.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            User updatedUser = await _userRepository.UpdateUserAsync(user);
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }

            IResult result = (IResult)await _userRepository.DeleteUserAsync(Id);
            return (IActionResult)result;
        }
    }
}
