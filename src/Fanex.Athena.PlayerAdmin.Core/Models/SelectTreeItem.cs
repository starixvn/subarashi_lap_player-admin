using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.Core.Models
{
    public class SelectTreeItem
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool Checked { get; set; }

        public IEnumerable<SelectTreeItem> Children { get; set; }
    }
}