namespace Fanex.Athena.PlayerAdmin.Commission.MetaModels
{
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels;

    public class MultipleUpdateGroupQuery : BasePlayerCommissionUpdateQuery
    {
        public IEnumerable<PlayerCommissionSettingViewModel> CommValues { get; set; }

        public override bool IsValid
            => base.IsValid && CommValues != null && CommValidator.IsValid(CommValues.Select(_ => _.Discount).ToArray());
    }
}