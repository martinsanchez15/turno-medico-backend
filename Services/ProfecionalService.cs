using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TurnoMedicoBackend.Models;
using TurnoMedicoBackend.Settings;
using BCrypt.Net;

namespace TurnoMedicoBackend.Services
{
    public class ProfesionalService
    {
        private readonly IMongoCollection<Profesional> _profesionales;

        public ProfesionalService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _profesionales = database.GetCollection<Profesional>("Profesionales");
        }

        public async Task<List<Profesional>> GetAsync() =>
            await _profesionales.Find(_ => true).ToListAsync();

        public async Task<Profesional?> GetByIdAsync(string id) =>
            await _profesionales.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task<Profesional?> GetByEmailAsync(string email) =>
            await _profesionales.Find(p => p.Email == email).FirstOrDefaultAsync();

        public async Task CreateAsync(Profesional profesional)
        {
            // Evitar duplicados
            var existing = await GetByEmailAsync(profesional.Email);
            if (existing != null)
                throw new Exception("Ya existe un profesional con este email.");

            // Encriptar contraseña
            profesional.Password = BCrypt.Net.BCrypt.HashPassword(profesional.Password);
            await _profesionales.InsertOneAsync(profesional);
        }

        public async Task UpdateAsync(string id, Profesional profesional) =>
            await _profesionales.ReplaceOneAsync(p => p.Id == id, profesional);

        public async Task DeleteAsync(string id) =>
            await _profesionales.DeleteOneAsync(p => p.Id == id);

        public async Task<Profesional?> LoginAsync(string email, string password)
        {
            var profesional = await GetByEmailAsync(email);
            if (profesional is null) return null;

            var valid = BCrypt.Net.BCrypt.Verify(password, profesional.Password);
            return valid ? profesional : null;
        }
    }
}
