namespace Fanex.Athena.PlayerAdmin.Infrastructure.Configs
{
    public class IdentityServerSetting
    {
        public string Host { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }
        public string TokenEndpoint { get; set; }
        public string UserInfoEndpoint { get; set; }
    }
}