using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.Commission.MetaModels
{
    public class MultipleUpdateGroupQuery : BasePlayerCommissionUpdateQuery
    {
        public string[] Products { get; set; }

        public decimal[] Comms { get; set; }

        public Dictionary<string, PlayerCommissionSettingViewModel> CommValues { get; set; }

        public override bool IsValid
            => base.IsValid && CommValidator.IsValid(CommValues.Select(_ => _.Value.Discount).ToArray());
    }
}