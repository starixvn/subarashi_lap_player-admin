namespace Fanex.Athena.PlayerAdmin.Commission.ViewModels
{
    using System.Collections.Generic;

    public class PlayerCommissionViewModel
    {
        public int LicenseeId { get; set; }

        public int GroupCommId { get; set; }

        public string GroupName { get; set; }

        public IEnumerable<PlayerCommissionSettingViewModel> CommValues { get; set; }
    }
}