using Microsoft.AspNetCore.Mvc;
using TurnoMedicoBackend.Models;
using TurnoMedicoBackend.Services;

namespace TurnoMedicoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly PacienteService _pacienteService;

        public PacienteController(PacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Paciente>>> Get() =>
            await _pacienteService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Paciente>> Get(string id)
        {
            var paciente = await _pacienteService.GetByIdAsync(id);

            if (paciente is null)
                return NotFound();

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

            if (existing is null)
                return NotFound();

            paciente.Id = id;
            await _pacienteService.UpdateAsync(id, paciente);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var paciente = await _pacienteService.GetByIdAsync(id);

            if (paciente is null)
                return NotFound();

            await _pacienteService.DeleteAsync(id);
            return NoContent();
        }

        // ✅ Nuevo endpoint: login de paciente
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var paciente = await _pacienteService.LoginAsync(request.Email, request.Password);
            if (paciente == null)
                return Unauthorized("Credenciales inválidas");

            return Ok(new
            {
                paciente.Id,
                paciente.Nombre,
                paciente.Email
                // Más adelante: acá se devolvería el JWT
            });
        }
    }
}
