namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Settings
{
    using Core.Settings;

    public class RedisSettingTests
    {
        [Fact]
        public void CanCreateInstanceAndSetProperties()
        {
            var server = "localhost";
            var port = 6379;
            var pass = "password";
            var appName = "TestApp";
            var database = 1;
            var keepAlive = 180;
            var protectionDataKey = "TestKey";

            var setting = new RedisSetting
            {
                Server = server,
                Port = port,
                Pass = pass,
                AppName = appName,
                Database = database,
                KeepAlive = keepAlive,
                ProtectionDataKey = protectionDataKey
            };

            Assert.Equal(server, setting.Server);
            Assert.Equal(port, setting.Port);
            Assert.Equal(pass, setting.Pass);
            Assert.Equal(appName, setting.AppName);
            Assert.Equal(database, setting.Database);
            Assert.Equal(keepAlive, setting.KeepAlive);
            Assert.Equal(protectionDataKey, setting.ProtectionDataKey);
        }
    }
}
