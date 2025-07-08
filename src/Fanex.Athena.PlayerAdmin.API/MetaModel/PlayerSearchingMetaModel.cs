using Fanex.Athena.PlayerAdmin.Core.Models;

namespace Fanex.Athena.PlayerAdmin.API.MetaModel
{
    public class PlayerSearchingMetaModel : PlayerListBaseMetaModel, ISubsidiarySearchMetaModel
    {
        public byte SearchOption { get; set; }

        public string UserName { get; set; }

        public byte BaFilter { get; set; }

        public string SubsidiaryList { get; set; }

        public bool SubsidiarySearch { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}