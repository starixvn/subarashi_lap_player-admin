namespace Fanex.Athena.PlayerAdmin.Commission.ViewModels
{
    using System.Collections.Generic;
    using MetaModels;

    public class MultipleUpdateGroupViewModel : BaseUpdateGroupViewModel
    {
        public MultipleUpdateGroupViewModel(MultipleUpdateGroupQuery query) : base(query)
        {
        }

        public IEnumerable<PlayerCommissionSettingViewModel> CommValues { get; set; }
    }
}