using Microsoft.AspNetCore.Mvc;
using TurnoMedicoBackend.Models;
using TurnoMedicoBackend.Services;

namespace TurnoMedicoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnoController : ControllerBase
    {
        private readonly TurnoService _turnoService;

        public TurnoController(TurnoService turnoService)
        {
            _turnoService = turnoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Turno>>> Get() =>
            await _turnoService.GetAsync();

        [HttpGet("paciente/{pacienteId}")]
        public async Task<ActionResult<List<Turno>>> GetByPaciente(string pacienteId) =>
            await _turnoService.GetByPacienteIdAsync(pacienteId);

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Turno>> Get(string id)
        {
            var turno = await _turnoService.GetByIdAsync(id);
            if (turno is null) return NotFound();
            return turno;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Turno turno)
        {
            await _turnoService.CreateAsync(turno);
            return CreatedAtAction(nameof(Get), new { id = turno.Id }, turno);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var turno = await _turnoService.GetByIdAsync(id);
            if (turno is null) return NotFound();

            await _turnoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
