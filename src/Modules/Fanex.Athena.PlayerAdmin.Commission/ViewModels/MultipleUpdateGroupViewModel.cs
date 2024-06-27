using Fanex.Athena.PlayerAdmin.Commission.Helpers;
using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.Commission.ViewModels
{
    public class MultipleUpdateGroupViewModel : BaseUpdateGroupViewModel
    {
        public MultipleUpdateGroupViewModel(MultipleUpdateGroupQuery query) : base(query)
        {
        }

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