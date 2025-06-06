using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TurnoMedicoBackend.Models
{
    public class Profesional
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
