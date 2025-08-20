namespace Fanex.Athena.PlayerAdmin.API.ViewModels
{
    using System.Collections.Generic;
    using DTO;

    public class DepositSettingViewModel
    {
        public IEnumerable<RiskLevel> RiskLevels { get; set; }

        public IEnumerable<string> OddsGroups { get; set; }

        public IEnumerable<CommGroupViewModel> CommGroups { get; set; }
    }
}