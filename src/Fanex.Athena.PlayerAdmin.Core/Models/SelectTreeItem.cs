namespace Fanex.Athena.PlayerAdmin.Core.Models
{
    using System.Collections.Generic;

    public class SelectTreeItem
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool Checked { get; set; }

        public IEnumerable<SelectTreeItem> Children { get; set; }
    }
}