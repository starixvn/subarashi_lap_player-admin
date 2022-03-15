using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.API.ViewModels
{
    public class DepositSettingViewModel
    {
        public IEnumerable<string> CustomerClass { get; set; }
        public IEnumerable<string> OddsGroups { get; set; }
        public IEnumerable<CommGroupViewModel> CommGroups { get; set; }
    }
}
