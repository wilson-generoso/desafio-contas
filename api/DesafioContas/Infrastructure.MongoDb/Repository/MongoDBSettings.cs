namespace Desafio.Contas.Infrastructure.MongoDb.Repository
{
    public class MongoDBSettings
    {
        public const string CONFIG_KEY = "MongoDBSettings";

        public string DatabaseName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AuthenticationMechanism { get; set; } = "SCRAM-SHA-1";
    }
}
