namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Settings
{
    using Core.Settings;

    public class CustomerClassApiSettingTests
    {
        [Fact]
        public void Properties_ShouldBeSettableAndGettable()
        {
            var endpoints = new CustomerClassEndpoint
            {
                AddLicBA = "/api/add",
                RemoveLicBA = "/api/remove",
                GetCustomerClassCategory = "/api/categories"
            };

            var setting = new CustomerClassApiSetting
            {
                BaseAddress = "https://example.com",
                EndPoints = endpoints,
                Enabled = true
            };

            Assert.Equal("https://example.com", setting.BaseAddress);
            Assert.True(setting.Enabled);
            Assert.NotNull(setting.EndPoints);
            Assert.Equal("/api/add", setting.EndPoints.AddLicBA);
            Assert.Equal("/api/remove", setting.EndPoints.RemoveLicBA);
            Assert.Equal("/api/categories", setting.EndPoints.GetCustomerClassCategory);
        }
    }
}

