using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TurnoMedicoBackend.Models;
using TurnoMedicoBackend.Configurations;

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

        public async Task<List<Turno>> GetAsync() =>
            await _turnos.Find(_ => true).ToListAsync();

        public async Task<List<Turno>> GetByPacienteIdAsync(string pacienteId) =>
            await _turnos.Find(t => t.PacienteId == pacienteId).ToListAsync();

        public async Task<Turno?> GetByIdAsync(string id) =>
            await _turnos.Find(t => t.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Turno turno) =>
            await _turnos.InsertOneAsync(turno);

        public async Task DeleteAsync(string id) =>
            await _turnos.DeleteOneAsync(t => t.Id == id);
    }
}
