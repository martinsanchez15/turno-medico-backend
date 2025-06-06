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
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly PacienteService _pacienteService;
        private readonly ProfesionalService _profesionalService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(PacienteService pacienteService, ProfesionalService profesionalService, IOptions<JwtSettings> jwtSettings)
        {
            _pacienteService = pacienteService;
            _profesionalService = profesionalService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            object usuario = null;
            string rol = null;

            // Intentar login como paciente
            var paciente = await _pacienteService.LoginAsync(request.Email, request.Password);
            if (paciente != null)
            {
                usuario = new
                {
                    paciente.Id,
                    paciente.Nombre,
                    paciente.Email,
                    Rol = "paciente"
                };
                rol = "paciente";
            }

            // Intentar login como profesional
            var profesional = await _profesionalService.LoginAsync(request.Email, request.Password);
            if (profesional != null)
            {
                usuario = new
                {
                    profesional.Id,
                    profesional.Nombre,
                    profesional.Email,
                    Rol = "profesional"
                };
                rol = "profesional";
            }

            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Email o contraseña incorrectos" });
            }

            var token = GenerarToken(request.Email, rol);
            return Ok(new { token, usuario });
        }

        private string GenerarToken(string email, string rol)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, rol)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
