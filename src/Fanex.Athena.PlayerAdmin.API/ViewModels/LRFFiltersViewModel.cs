using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.API.ViewModels
{
    public class LRFFiltersViewModel
    {
        public IEnumerable<string> CustomerClass { get; set; }
        public IEnumerable<CommGroupViewModel> CommGroups { get; set; }
    }
}
