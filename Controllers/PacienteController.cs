using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class PacienteController : ControllerBase
    {
        private readonly PacienteService _pacienteService;
        private readonly JwtSettings _jwtSettings;

        public PacienteController(PacienteService pacienteService, IOptions<JwtSettings> jwtSettings)
        {
            _pacienteService = pacienteService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpGet]
        public async Task<ActionResult<List<Paciente>>> Get() =>
                    await _pacienteService.GetAsync();

        [Authorize(Roles = "Paciente")]
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Paciente>> Get(string id)
        {
            var paciente = await _pacienteService.GetByIdAsync(id);
            if (paciente is null) return NotFound();
            return paciente;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Paciente paciente)
        {
            await _pacienteService.CreateAsync(paciente);
            return CreatedAtAction(nameof(Get), new { id = paciente.Id }, paciente);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Paciente paciente)
        {
            var existing = await _pacienteService.GetByIdAsync(id);
            if (existing is null) return NotFound();

            paciente.Id = id;
            await _pacienteService.UpdateAsync(id, paciente);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var paciente = await _pacienteService.GetByIdAsync(id);
            if (paciente is null) return NotFound();

            await _pacienteService.DeleteAsync(id);
            return NoContent();
        }

        // 🔐 Login y generación de token JWT
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var paciente = await _pacienteService.LoginAsync(request.Email, request.Password);
            if (paciente == null)
                return Unauthorized("Credenciales inválidas");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, paciente.Id),
                    new Claim(ClaimTypes.Email, paciente.Email),
                    new Claim(ClaimTypes.Name, paciente.Nombre),
                    new Claim(ClaimTypes.Role, "Paciente")
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                paciente.Id,
                paciente.Nombre,
                paciente.Email
            });
        }

        // 🔒 Endpoint protegido
        [Authorize(Roles = "Paciente")]
        [HttpGet("perfil")]
        public IActionResult Perfil()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var nombre = User.FindFirstValue(ClaimTypes.Name);
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Ok(new
            {
                mensaje = $"Bienvenido, {nombre}",
                id,
                email
            });
        }
    }
}
