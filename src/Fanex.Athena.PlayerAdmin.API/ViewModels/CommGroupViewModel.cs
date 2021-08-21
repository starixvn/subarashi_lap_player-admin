using Fanex.Athena.PlayerAdmin.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.ViewModels
{
    public class CommGroupViewModel
    {
        public CommGroupViewModel()
        {
            Id = "0";
            Name = ViewResources.None;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}
