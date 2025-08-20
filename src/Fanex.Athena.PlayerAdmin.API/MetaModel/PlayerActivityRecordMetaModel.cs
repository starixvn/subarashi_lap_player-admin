namespace Fanex.Athena.PlayerAdmin.API.MetaModel
{
    using Core.Models;

    public class PlayerActivityRecordMetaModel : ISiteNameMetaModel, ICustomerIdentityMetaModel
    {
        public int CustId { get; set; }
        public int CurrencyId { get; set; }
        public string SiteName { get; set; }
    }
}