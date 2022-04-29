using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.Core.Settings
{
    public class AppSetting
    {
        public bool IsProduction { get; set; }
        public string Env { get; set; }
        public IEnumerable<string> FakeLicensees { get; set; }
        public bool IsShowMixedSport { get; set; }
        public IEnumerable<string> AllowedCrossDomains { get; set; }
        public IEnumerable<string> SpecialBAAdmins { get; set; }
        public string BetTicketEventBuilderUrl { get; set; }
        public string CustomerClassUrl { get; set; }
        public IEnumerable<OddsGroup> OddsGroups { get; set; }
    }

    public class OddsGroup
    {
        public string Value { get; set; }
        public IEnumerable<string> OfferedBy { get; set; }
    }
}