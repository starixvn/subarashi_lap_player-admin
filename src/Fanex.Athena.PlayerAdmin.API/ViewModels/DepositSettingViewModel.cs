using Fanex.Athena.PlayerAdmin.API.DTO;
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.API.ViewModels
{
    public class DepositSettingViewModel
    {
        public IEnumerable<RiskLevel> RiskLevels { get; set; }

        public IEnumerable<string> OddsGroups { get; set; }

        public IEnumerable<CommGroupViewModel> CommGroups { get; set; }
    }
}