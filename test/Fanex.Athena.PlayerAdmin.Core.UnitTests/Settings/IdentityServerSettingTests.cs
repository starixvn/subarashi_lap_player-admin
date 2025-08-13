// src/Fanex.Athena.PlayerAdmin.Core/Settings/IdentityServerSetting.cs
namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Settings
{
    using AutoFixture;
    using Core.Settings;

    public class IdentityServerSettingTests
    {
        private readonly Fixture _fixture;

        public IdentityServerSettingTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void IdentityServerSetting_CanBeInstantiated()
        {
            // Act
            var setting = new IdentityServerSetting();

            // Assert
            Assert.NotNull(setting);
        }

        [Fact]
        public void IdentityServerSetting_AllPropertiesCanBeSetAndGet()
        {
            // Arrange
            var host = _fixture.Create<string>();
            var myProperty = _fixture.Create<int>();
            var internalApiScope = _fixture.Create<string>();
            var externalApiScope = _fixture.Create<string>();
            var grantType = _fixture.Create<string>();
            var tokenEndpoint = _fixture.Create<string>();
            var userInfoEndpoint = _fixture.Create<string>();
            var cacheDuration = _fixture.Create<int>();
            var credentialClient = _fixture.Create<CredentialsSetting>();
            var credentialIntrospection = _fixture.Create<CredentialsSetting>();

            // Act
            var setting = new IdentityServerSetting
            {
                Host = host,
                MyProperty = myProperty,
                CredentialClient = credentialClient,
                CredentialIntrospection = credentialIntrospection,
                InternalApiScope = internalApiScope,
                ExternalApiScope = externalApiScope,
                GrantType = grantType,
                TokenEndpoint = tokenEndpoint,
                UserInfoEndpoint = userInfoEndpoint,
                CacheDuration = cacheDuration
            };

            // Assert
            Assert.Equal(host, setting.Host);
            Assert.Equal(myProperty, setting.MyProperty);
            Assert.Equal(credentialClient, setting.CredentialClient);
            Assert.Equal(credentialIntrospection, setting.CredentialIntrospection);
            Assert.Equal(internalApiScope, setting.InternalApiScope);
            Assert.Equal(externalApiScope, setting.ExternalApiScope);
            Assert.Equal(grantType, setting.GrantType);
            Assert.Equal(tokenEndpoint, setting.TokenEndpoint);
            Assert.Equal(userInfoEndpoint, setting.UserInfoEndpoint);
            Assert.Equal(cacheDuration, setting.CacheDuration);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("https://identity.server.com")]
        [InlineData("http://localhost:5000")]
        public void IdentityServerSetting_Host_CanBeSetToVariousValues(string hostValue)
        {
            // Arrange & Act
            var setting = new IdentityServerSetting { Host = hostValue };

            // Assert
            Assert.Equal(hostValue, setting.Host);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(42)]
        public void IdentityServerSetting_MyProperty_CanBeSetToVariousValues(int value)
        {
            // Arrange & Act
            var setting = new IdentityServerSetting { MyProperty = value };

            // Assert
            Assert.Equal(value, setting.MyProperty);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("internal.scope")]
        [InlineData("api://internal")]
        public void IdentityServerSetting_InternalApiScope_CanBeSetToVariousValues(string scopeValue)
        {
            // Arrange & Act
            var setting = new IdentityServerSetting { InternalApiScope = scopeValue };

            // Assert
            Assert.Equal(scopeValue, setting.InternalApiScope);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("external.scope")]
        [InlineData("api://external")]
        public void IdentityServerSetting_ExternalApiScope_CanBeSetToVariousValues(string scopeValue)
        {
            // Arrange & Act
            var setting = new IdentityServerSetting { ExternalApiScope = scopeValue };

            // Assert
            Assert.Equal(scopeValue, setting.ExternalApiScope);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("authorization_code")]
        [InlineData("client_credentials")]
        [InlineData("custom_grant_type")]
        public void IdentityServerSetting_GrantType_CanBeSetToVariousValues(string grantTypeValue)
        {
            // Arrange & Act
            var setting = new IdentityServerSetting { GrantType = grantTypeValue };

            // Assert
            Assert.Equal(grantTypeValue, setting.GrantType);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("/oauth/token")]
        [InlineData("/connect/token")]
        [InlineData("token")]
        public void IdentityServerSetting_TokenEndpoint_CanBeSetToVariousValues(string endpointValue)
        {
            // Arrange & Act
            var setting = new IdentityServerSetting { TokenEndpoint = endpointValue };

            // Assert
            Assert.Equal(endpointValue, setting.TokenEndpoint);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("/oauth/userinfo")]
        [InlineData("/connect/userinfo")]
        [InlineData("userinfo")]
        public void IdentityServerSetting_UserInfoEndpoint_CanBeSetToVariousValues(string endpointValue)
        {
            // Arrange & Act
            var setting = new IdentityServerSetting { UserInfoEndpoint = endpointValue };

            // Assert
            Assert.Equal(endpointValue, setting.UserInfoEndpoint);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(3600)]
        [InlineData(86400)]
        [InlineData(int.MaxValue)]
        public void IdentityServerSetting_CacheDuration_CanBeSetToVariousValues(int durationValue)
        {
            // Arrange & Act
            var setting = new IdentityServerSetting { CacheDuration = durationValue };

            // Assert
            Assert.Equal(durationValue, setting.CacheDuration);
        }

        [Fact]
        public void IdentityServerSetting_CredentialClient_CanBeSetToNull()
        {
            // Arrange & Act
            var setting = new IdentityServerSetting { CredentialClient = null };

            // Assert
            Assert.Null(setting.CredentialClient);
        }

        [Fact]
        public void IdentityServerSetting_CredentialIntrospection_CanBeSetToNull()
        {
            // Arrange & Act
            var setting = new IdentityServerSetting { CredentialIntrospection = null };

            // Assert
            Assert.Null(setting.CredentialIntrospection);
        }

        [Fact]
        public void IdentityServerSetting_CredentialClient_CanBeSetToValidCredentials()
        {
            // Arrange
            var credentials = new CredentialsSetting
            {
                ClientId = "test-client-id",
                ClientSecret = "test-client-secret"
            };

            // Act
            var setting = new IdentityServerSetting { CredentialClient = credentials };

            // Assert
            Assert.Equal(credentials, setting.CredentialClient);
            Assert.Equal("test-client-id", setting.CredentialClient.ClientId);
            Assert.Equal("test-client-secret", setting.CredentialClient.ClientSecret);
        }

        [Fact]
        public void IdentityServerSetting_CredentialIntrospection_CanBeSetToValidCredentials()
        {
            // Arrange
            var credentials = new CredentialsSetting
            {
                ClientId = "introspection-client-id",
                ClientSecret = "introspection-client-secret"
            };

            // Act
            var setting = new IdentityServerSetting { CredentialIntrospection = credentials };

            // Assert
            Assert.Equal(credentials, setting.CredentialIntrospection);
            Assert.Equal("introspection-client-id", setting.CredentialIntrospection.ClientId);
            Assert.Equal("introspection-client-secret", setting.CredentialIntrospection.ClientSecret);
        }

        [Fact]
        public void IdentityServerSetting_DefaultValues_AreSetCorrectly()
        {
            // Arrange & Act
            var setting = new IdentityServerSetting();

            // Assert
            Assert.Null(setting.Host);
            Assert.Equal(0, setting.MyProperty);
            Assert.Null(setting.CredentialClient);
            Assert.Null(setting.CredentialIntrospection);
            Assert.Null(setting.InternalApiScope);
            Assert.Null(setting.ExternalApiScope);
            Assert.Null(setting.GrantType);
            Assert.Null(setting.TokenEndpoint);
            Assert.Null(setting.UserInfoEndpoint);
            Assert.Equal(0, setting.CacheDuration);
        }

        [Fact]
        public void IdentityServerSetting_MultipleInstancesAreIndependent()
        {
            // Arrange
            var setting1 = new IdentityServerSetting { Host = "host1" };
            var setting2 = new IdentityServerSetting { Host = "host2" };

            // Act & Assert
            Assert.Equal("host1", setting1.Host);
            Assert.Equal("host2", setting2.Host);
            Assert.NotEqual(setting1.Host, setting2.Host);
        }

        [Fact]
        public void IdentityServerSetting_CanBeConfiguredCompletelyFromFixture()
        {
            // Arrange & Act
            var setting = _fixture.Create<IdentityServerSetting>();

            // Assert
            Assert.NotNull(setting);
            Assert.NotNull(setting.Host);
            Assert.NotNull(setting.InternalApiScope);
            Assert.NotNull(setting.ExternalApiScope);
            Assert.NotNull(setting.GrantType);
            Assert.NotNull(setting.TokenEndpoint);
            Assert.NotNull(setting.UserInfoEndpoint);
            Assert.NotNull(setting.CredentialClient);
            Assert.NotNull(setting.CredentialIntrospection);
        }

        [Fact]
        public void IdentityServerSetting_PropertiesCanBeModifiedAfterCreation()
        {
            // Arrange
            var setting = new IdentityServerSetting { Host = "initial-host" };
            var newHost = "updated-host";

            // Act
            setting.Host = newHost;

            // Assert
            Assert.Equal(newHost, setting.Host);
        }

        [Fact]
        public void IdentityServerSetting_CanHandleExtremeStringValues()
        {
            // Arrange
            var veryLongString = new string('x', 10000);
            var emptyString = string.Empty;

            // Act
            var setting = new IdentityServerSetting
            {
                Host = veryLongString,
                InternalApiScope = emptyString,
                ExternalApiScope = veryLongString,
                GrantType = emptyString
            };

            // Assert
            Assert.Equal(veryLongString, setting.Host);
            Assert.Equal(emptyString, setting.InternalApiScope);
            Assert.Equal(veryLongString, setting.ExternalApiScope);
            Assert.Equal(emptyString, setting.GrantType);
        }

        [Fact]
        public void IdentityServerSetting_SpecialCharactersInProperties_AreHandledCorrectly()
        {
            // Arrange
            var specialChars = "!@#$%^&*()_+-=[]{}|;':\",./<>?`~";
            var unicodeString = "测试中文字符串";
            var urlWithSpecialChars = "https://example.com/path?param1=value1&param2=value2";

            // Act
            var setting = new IdentityServerSetting
            {
                Host = urlWithSpecialChars,
                InternalApiScope = specialChars,
                ExternalApiScope = unicodeString,
                GrantType = specialChars
            };

            // Assert
            Assert.Equal(urlWithSpecialChars, setting.Host);
            Assert.Equal(specialChars, setting.InternalApiScope);
            Assert.Equal(unicodeString, setting.ExternalApiScope);
            Assert.Equal(specialChars, setting.GrantType);
        }
    }
}