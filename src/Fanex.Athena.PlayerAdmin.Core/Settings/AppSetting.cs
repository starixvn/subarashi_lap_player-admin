using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.Core.Settings
{
    public class AppSetting
    {
        public string Environment { get; set; }
        public IEnumerable<string> FakeLicensees { get; set; }
        public bool IsShowMixedSport { get; set; }
        public IEnumerable<string> AllowedCrossDomains { get; set; }
        public IEnumerable<string> SpecialBAAdmins { get; set; }
        public string BetTicketEventBuilderUrl { get; set; }
        public string CustomerClassUrl { get; set; }
    }
}