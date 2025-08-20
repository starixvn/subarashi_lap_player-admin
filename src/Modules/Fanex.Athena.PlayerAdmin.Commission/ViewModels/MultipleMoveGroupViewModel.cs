namespace Fanex.Athena.PlayerAdmin.Commission.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using MetaModels;

    public class MultipleMoveGroupViewModel : BaseUpdateGroupViewModel
    {
        public MultipleMoveGroupViewModel(MultipleMoveGroupQuery query, IEnumerable<PlayerCommissionViewModel> groups) : base(query)
        {
            Groups = groups.ToList();
        }

        public IList<PlayerCommissionViewModel> Groups { get; set; }
    }
}
