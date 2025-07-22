using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.Commission.ViewModels
{
    public class PlayerCommissionViewModel
    {
        public int LicenseeId { get; set; }

        public int GroupCommId { get; set; }

        public string GroupName { get; set; }

        public IEnumerable<PlayerCommissionSettingViewModel> CommValues { get; set; }
    }
}