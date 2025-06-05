using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TurnoMedicoBackend.Models;
using TurnoMedicoBackend.Services;
using TurnoMedicoBackend.Settings;

namespace TurnoMedicoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfesionalController : ControllerBase
    {
        private readonly ProfesionalService _profesionalService;
        private readonly JwtSettings _jwtSettings;

        public ProfesionalController(ProfesionalService profesionalService, IOptions<JwtSettings> jwtSettings)
        {
            _profesionalService = profesionalService;
            _jwtSettings = jwtSettings.Value;
        }

        [Authorize(Roles = "Profesional")]
        [HttpGet]
        public async Task<ActionResult<List<Profesional>>> Get() =>
            await _profesionalService.GetAsync();

        [Authorize(Roles = "Profesional")]
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Profesional>> Get(string id)
        {
            var profesional = await _profesionalService.GetByIdAsync(id);
            if (profesional is null) return NotFound();
            return profesional;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Profesional profesional)
        {
            await _profesionalService.CreateAsync(profesional);
            return CreatedAtAction(nameof(Get), new { id = profesional.Id }, profesional);
        }

        [Authorize(Roles = "Profesional")]
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Profesional profesional)
        {
            var existing = await _profesionalService.GetByIdAsync(id);
            if (existing is null) return NotFound();

            profesional.Id = id;
            await _profesionalService.UpdateAsync(id, profesional);
            return NoContent();
        }

        [Authorize(Roles = "Profesional")]
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var profesional = await _profesionalService.GetByIdAsync(id);
            if (profesional is null) return NotFound();

            await _profesionalService.DeleteAsync(id);
            return NoContent();
        }

        // 🔐 Login con JWT
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var profesional = await _profesionalService.LoginAsync(request.Email, request.Password);
            if (profesional == null)
                return Unauthorized("Credenciales inválidas");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, profesional.Id),
                    new Claim(ClaimTypes.Email, profesional.Email),
                    new Claim(ClaimTypes.Name, profesional.Nombre),
                    new Claim(ClaimTypes.Role, "Profesional")
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                profesional.Id,
                profesional.Nombre,
                profesional.Email
            });
        }

        // 🔒 Endpoint protegido con rol Profesional
        [Authorize(Roles = "Profesional")]
        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var nombre = User.FindFirstValue(ClaimTypes.Name);
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Ok(new
            {
                mensaje = $"Bienvenido, Dr. {nombre}",
                id,
                email
            });
        }
    }
}
