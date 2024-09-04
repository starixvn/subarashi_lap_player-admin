using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.Commission.MetaModels
{
    public class MultipleUpdateGroupQuery : BasePlayerCommissionUpdateQuery
    {
        public IEnumerable<PlayerCommissionSettingViewModel> CommValues { get; set; }

        public override bool IsValid
            => base.IsValid && CommValues != null && CommValidator.IsValid(CommValues.Select(_ => _.Discount).ToArray());
    }
}