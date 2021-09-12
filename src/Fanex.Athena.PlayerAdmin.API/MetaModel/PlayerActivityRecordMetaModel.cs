using Fanex.Athena.PlayerAdmin.Core.Models;

namespace Fanex.Athena.PlayerAdmin.API.MetaModel
{
    public class PlayerActivityRecordMetaModel : ISiteNameMetaModel, ICustomerIdentityMetaModel
    {
        public int CustId { get; set; }
        public int CurrencyId { get; set; }
        public string SiteName { get; set; }
    }
}