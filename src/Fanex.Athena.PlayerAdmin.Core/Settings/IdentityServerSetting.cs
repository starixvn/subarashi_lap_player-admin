namespace Fanex.Athena.PlayerAdmin.Core.Settings
{
    public class IdentityServerSetting
    {
        public string Host { get; set; }
        public int MyProperty { get; set; }
        public CredentialsSetting CredentialClient { get; set; }
        public CredentialsSetting CredentialIntrospection { get; set; }
        public string InternalApiScope { get; set; }
        public string ExternalApiScope { get; set; }
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