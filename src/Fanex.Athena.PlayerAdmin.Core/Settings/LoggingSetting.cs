namespace Fanex.Athena.PlayerAdmin.Core.Settings
{
    public class LoggingSetting
    {
        public string Category { get; set; }
        public SentryEngine SentryEngine { get; set; }
        public RabbitEngine RabbitEngine { get; set; }
    }

    public class SentryEngine
    {
        public string BackendDsn { get; set; }
    }

    public class RabbitEngine
    {
        public string ConnectionString { get; set; }
    }
}