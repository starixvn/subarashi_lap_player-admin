namespace Fanex.Athena.PlayerAdmin.Infrastructure.Settings
{
    public class IdentityServerSetting
    {
        public string Host { get; set; }
        public int MyProperty { get; set; }
        public CredentialsSetting CredentialsClient { get; set; }
        public CredentialsSetting CredentialsAPI { get; set; }
        public string GrantType { get; set; }
        public string TokenEndpoint { get; set; }
        public string UserInfoEndpoint { get; set; }
        public int CacheDuration { get; set; }
    }

    public class CredentialsSetting
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}