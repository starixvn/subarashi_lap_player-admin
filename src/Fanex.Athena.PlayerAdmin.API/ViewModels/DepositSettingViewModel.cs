using Fanex.Athena.PlayerAdmin.Core.Models;
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.API.ViewModels
{
    public class DepositSettingViewModel
    {
        public IEnumerable<string> CustomerClass { get; set; }
        public IEnumerable<CommGroupViewModel> CommGroups { get; set; }
    }
}
