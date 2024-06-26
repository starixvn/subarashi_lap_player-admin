using Fanex.Athena.PlayerAdmin.Commission.Helpers;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Athena.PlayerAdmin.Commission.ViewModels
{
    public class PlayerCommissionViewModel
    {
        public int LicenseeId { get; set; }

        public int GroupCommId { get; set; }

        public string GroupName { get; set; }

        public Dictionary<string, decimal> CommValues2 { get; set; }

        public Dictionary<string, PlayerCommissionSettingViewModel> CommValues { get; set; }

        public void InitDefaultValue(IDictionary<string, bool> permissions)
        {
            CommValues = new Dictionary<string, PlayerCommissionSettingViewModel>();

            foreach (var permission in permissions)
            {
                var setting = CommissionGroupHelper.GetSetting(permission.Key);

                if (setting != null)
                {
                    CommValues.Add(
                        permission.Key,
                        new PlayerCommissionSettingViewModel
                        {
                            Sporttype = setting.Item1,
                            Bettype = setting.Item2
                        }
                    );
                }
            }
        }
    }
}