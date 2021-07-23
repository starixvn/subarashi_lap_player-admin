using Fanex.Athena.PlayerAdmin.Core.Models;

namespace Fanex.Athena.PlayerAdmin.API.MetaModel
{
    public class PlayerActivityRecordMetaModel : ISiteNameMetaModel
    {
        public int IbcCustId { get; set; }

        public string SiteName { get; set; }
    }
}