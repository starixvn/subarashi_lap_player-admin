namespace Fanex.Athena.PlayerAdmin.Infrastructure.Settings
{
    public class ConsoleClient
    {
        public ApiInfo BetList { get; set; }
        public ApiInfo Report { get; set; }
        public ApiInfo Lg { get; set; }
    }

    public class ApiInfo
    {
        public string Host { get; set; }
        public string AuthKey { get; set; }
    }
}