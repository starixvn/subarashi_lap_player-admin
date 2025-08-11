namespace Fanex.Athena.PlayerAdmin.Core.Settings
{
    using System.Collections.Generic;

    public class AppSetting
    {
        public bool IsProduction { get; set; }
        public string Env { get; set; }
        public bool IsShowMixedSport { get; set; }
        public IEnumerable<string> AllowedCrossDomains { get; set; }
        public IEnumerable<string> SpecialBAAdmins { get; set; }
        public string BetTicketEventBuilderUrl { get; set; }
        public string CustomerClassUrl { get; set; }
    }
}