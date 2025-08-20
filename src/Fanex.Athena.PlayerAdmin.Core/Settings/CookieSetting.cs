namespace Fanex.Athena.PlayerAdmin.Core.Settings
{
    using Microsoft.AspNetCore.Http;

    public class CookieSetting
    {
        public string Name { get; set; }
        public int ExpireTime { get; set; }
        public SameSiteMode SameSite { get; set; }
        public CookieSecurePolicy Secure { get; set; }
        public bool SlidingExpiration { get; set; }
        public string LoginPath { get; set; }
        public string DeniedPath { get; set; }
    }
}