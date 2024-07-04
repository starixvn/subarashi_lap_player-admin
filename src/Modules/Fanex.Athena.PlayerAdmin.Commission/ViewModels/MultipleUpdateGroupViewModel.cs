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

        public IEnumerable<PlayerCommissionSettingViewModel> CommValues { get; set; }
    }
}