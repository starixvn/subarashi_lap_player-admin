namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Settings
{
    using Core.Settings;

    public class AppSettingTests
    {
        [Fact]
        public void Properties_AreSetAndRetrieved()
        {
            var appSetting = new AppSetting();
            var allowedCrossDomains = new List<string> { "domain1", "domain2" };
            var specialBAAdmins = new List<string> { "admin1", "admin2" };

            appSetting.IsProduction = true;
            appSetting.Env = "Production";
            appSetting.IsShowMixedSport = true;
            appSetting.AllowedCrossDomains = allowedCrossDomains;
            appSetting.SpecialBAAdmins = specialBAAdmins;
            appSetting.BetTicketEventBuilderUrl = "http://localhost:8080";
            appSetting.CustomerClassUrl = "http://localhost:8081";

            Assert.True(appSetting.IsProduction);
            Assert.Equal("Production", appSetting.Env);
            Assert.True(appSetting.IsShowMixedSport);
            Assert.Equal(allowedCrossDomains, appSetting.AllowedCrossDomains);
            Assert.Equal(specialBAAdmins, appSetting.SpecialBAAdmins);
            Assert.Equal("http://localhost:8080", appSetting.BetTicketEventBuilderUrl);
            Assert.Equal("http://localhost:8081", appSetting.CustomerClassUrl);
        }
    }
}
