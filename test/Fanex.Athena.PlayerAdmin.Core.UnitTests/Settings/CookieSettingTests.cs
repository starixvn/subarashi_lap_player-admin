namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Settings
{
    using Core.Settings;
    using Microsoft.AspNetCore.Http;

    public class CookieSettingTests
    {
        [Fact]
        public void Properties_ShouldBeSettableAndGettable()
        {
            var setting = new CookieSetting
            {
                Name = "AuthCookie",
                ExpireTime = 45,
                SameSite = SameSiteMode.None,
                Secure = CookieSecurePolicy.Always,
                SlidingExpiration = true,
                LoginPath = "/login",
                DeniedPath = "/denied"
            };

            Assert.Equal("AuthCookie", setting.Name);
            Assert.Equal(45, setting.ExpireTime);
            Assert.Equal(SameSiteMode.None, setting.SameSite);
            Assert.Equal(CookieSecurePolicy.Always, setting.Secure);
            Assert.True(setting.SlidingExpiration);
            Assert.Equal("/login", setting.LoginPath);
            Assert.Equal("/denied", setting.DeniedPath);
        }
    }
}

