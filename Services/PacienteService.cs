using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TurnoMedicoBackend.Models;
using TurnoMedicoBackend.Configurations;

namespace TurnoMedicoBackend.Services
{
    public class PacienteService
    {
        private readonly IMongoCollection<Paciente> _pacientes;

        public PacienteService(IOptions<MongoDBSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _pacientes = database.GetCollection<Paciente>(settings.Value.PacienteCollectionName);
        }

        public async Task<List<Paciente>> GetAsync() =>
            await _pacientes.Find(_ => true).ToListAsync();

        public async Task<Paciente?> GetByIdAsync(string id) =>
            await _pacientes.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Paciente paciente) =>
            await _pacientes.InsertOneAsync(paciente);

        public async Task UpdateAsync(string id, Paciente paciente) =>
            await _pacientes.ReplaceOneAsync(p => p.Id == id, paciente);

        public async Task DeleteAsync(string id) =>
            await _pacientes.DeleteOneAsync(p => p.Id == id);
    }
}
