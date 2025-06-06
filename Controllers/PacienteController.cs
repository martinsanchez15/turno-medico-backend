using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

        // 🔒 Endpoint protegido para ver perfil autenticado
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
