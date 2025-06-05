using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TurnoMedicoBackend.Models;
using TurnoMedicoBackend.Settings;

namespace TurnoMedicoBackend.Services
{
    public class TurnoService
    {
        private readonly IMongoCollection<Turno> _turnos;

        public TurnoService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var db = client.GetDatabase(settings.Value.DatabaseName);
            _turnos = db.GetCollection<Turno>(settings.Value.TurnoCollectionName);
        }

        // Obtener todos los turnos
        public async Task<List<Turno>> GetAsync() =>
            await _turnos.Find(_ => true).ToListAsync();

        // Obtener turnos por paciente
        public async Task<List<Turno>> GetByPacienteIdAsync(string pacienteId) =>
            await _turnos.Find(t => t.PacienteId == pacienteId).ToListAsync();

        // Obtener turnos por profesional
        public async Task<List<Turno>> GetByProfesionalAsync(string profesional) =>
            await _turnos.Find(t => t.Profesional == profesional).ToListAsync();

        // Obtener turno por ID
        public async Task<Turno?> GetByIdAsync(string id) =>
            await _turnos.Find(t => t.Id == id).FirstOrDefaultAsync();

        // Verificar si un turno está disponible
        public async Task<bool> IsDisponibleAsync(string profesional, DateTime fechaHora)
        {
            var existente = await _turnos.Find(t =>
                t.Profesional == profesional && t.FechaHora == fechaHora
            ).FirstOrDefaultAsync();

            return existente == null;
        }

        // Crear turno si está disponible
        public async Task<bool> CreateIfDisponibleAsync(Turno turno)
        {
            if (!await IsDisponibleAsync(turno.Profesional, turno.FechaHora))
                return false;

            await _turnos.InsertOneAsync(turno);
            return true;
        }

        // Crear turno sin validación (por compatibilidad)
        public async Task CreateAsync(Turno turno) =>
            await _turnos.InsertOneAsync(turno);

        // Actualizar estado del turno (confirmado, cancelado, etc.)
        public async Task<bool> UpdateEstadoAsync(string id, string nuevoEstado)
        {
            var update = Builders<Turno>.Update.Set(t => t.Estado, nuevoEstado);
            var result = await _turnos.UpdateOneAsync(t => t.Id == id, update);
            return result.ModifiedCount > 0;
        }

        // Eliminar turno
        public async Task DeleteAsync(string id) =>
            await _turnos.DeleteOneAsync(t => t.Id == id);
    }
}
