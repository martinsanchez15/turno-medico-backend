using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TurnoMedicoBackend.Models
{
    public class Turno
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("pacienteId")]
        public string PacienteId { get; set; }

        [BsonElement("profesional")]
        public string Profesional { get; set; }

        [BsonElement("especialidad")]
        public string Especialidad { get; set; }

        [BsonElement("fechaHora")]
        public DateTime FechaHora { get; set; }

        [BsonElement("estado")]
        public string Estado { get; set; } = "pendiente"; // o "confirmado", "cancelado"
    }
}
