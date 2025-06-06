using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TurnoMedicoBackend.Models;
using TurnoMedicoBackend.Settings;
using BCrypt.Net;

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

        public async Task CreateAsync(Paciente paciente)
        {
            // Encriptar contraseña antes de guardar
            paciente.Password = BCrypt.Net.BCrypt.HashPassword(paciente.Password);
            await _pacientes.InsertOneAsync(paciente);
        }

        public async Task UpdateAsync(string id, Paciente paciente) =>
            await _pacientes.ReplaceOneAsync(p => p.Id == id, paciente);

        public async Task DeleteAsync(string id) =>
            await _pacientes.DeleteOneAsync(p => p.Id == id);

        // ✅ Método para login
        public async Task<Paciente?> LoginAsync(string email, string password)
        {
            var paciente = await _pacientes.Find(p => p.Email == email).FirstOrDefaultAsync();
            if (paciente == null) return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(password, paciente.Password);
            return isValid ? paciente : null;
        }

        // ✅ Método para búsqueda por email (usado por AuthController)
        public async Task<Paciente?> BuscarPorEmailAsync(string email) =>
            await _pacientes.Find(p => p.Email == email).FirstOrDefaultAsync();
    }
}
