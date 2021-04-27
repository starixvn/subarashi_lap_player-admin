using System.Collections.Generic;
using System.Linq;
using Fanex.Athena.PlayerAdmin.Commission.MetaModels;

namespace Fanex.Athena.PlayerAdmin.Commission.ViewModels
{
    public class MultipleMoveGroupViewModel : BaseUpdateGroupViewModel
    {
        public MultipleMoveGroupViewModel(MultipleMoveGroupQuery query, IEnumerable<PlayerCommissionViewModel> groups) : base(query)
        {
            Groups = groups.ToList();
        }

        public IList<PlayerCommissionViewModel> Groups { get; set; }
    }
}
