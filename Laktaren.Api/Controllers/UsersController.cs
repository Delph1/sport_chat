using BCrypt.Net; // Möjliggör lösenordskrypteringen!
using Laktaren.Application.Interfaces;
using Laktaren.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Laktaren.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase // Rättad stavning
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<User>),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            if (users == null)
            {
                return BadRequest("No users were found.");
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            User? user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound("Användaren hittades inte på läktaren.");
            }
            return Ok(user);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync([FromBody] User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Användardata och lösenord krävs.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var createdUser = await _userRepository.CreateUserAsync(user);

            if (createdUser != null)
            {
                createdUser.Password = "";
                return CreatedAtAction("GetById", new { id = createdUser.Id }, createdUser);
            }

            return BadRequest("Kunde inte skapa användaren.");
        }

        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] User user)
        {
            if (id == Guid.Empty || id != user.Id)
            {
                return BadRequest("ID matchar inte användaren.");
            }

            User updatedUser = await _userRepository.UpdateUserAsync(user);
            return Ok(updatedUser);
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Användar-ID krävs.");
            }

            bool result = await _userRepository.DeleteUserAsync(id);

            if (result)
            {
                return NoContent();
            }

            return BadRequest("Kunde inte radera användaren.");
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            // 1. Försök hitta användaren via e-post
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                // Vi ger ett generiskt felmeddelande av säkerhetsskäl (avslöja inte vad som var fel)
                return Unauthorized("Wrong password or e-mail.");
            }

            // 2. Verifiera lösenordet med BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!isPasswordValid)
            {
                return Unauthorized("Wrong password or e-mail.");
            }

            // 3. Om vi är här = inloggningen lyckades! Dags att skapa biljetten (JWT).
            var tokenHandler = new JwtSecurityTokenHandler();

            // OBS: I en riktig produktionsmiljö ska denna nyckel ligga skyddad i appsettings.json!
            var key = Encoding.ASCII.GetBytes("DenHarNyckelnMasteVaraMinst32TeckenLangOchMycketHemlig!!!");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Claims är information vi "bakar in" i biljetten, t.ex. användarens ID.
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                // Byt ut user.Username mot vad din egenskap faktiskt heter i din User-modell
                new Claim(ClaimTypes.Name, user.Username ?? "Supporter")
            }),
                Expires = DateTime.UtcNow.AddDays(7), // Biljetten är giltig i en vecka
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Skicka tillbaka biljetten till din Svelte-klient!
            return Ok(new
            {
                Message = "Välkommen in på läktaren!",
                Token = tokenString,
                UserId = user.Id
            });
        }
    }
}