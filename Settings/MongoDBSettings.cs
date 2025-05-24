namespace TurnoMedicoBackend.Configurations
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string PacienteCollectionName { get; set; } = null!;
        public string TurnoCollectionName { get; set; } = null!; 
    }
}
