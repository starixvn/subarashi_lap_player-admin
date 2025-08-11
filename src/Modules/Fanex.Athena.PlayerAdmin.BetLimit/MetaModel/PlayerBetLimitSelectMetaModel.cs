namespace Fanex.Athena.PlayerAdmin.BetLimit.MetaModel
{
    using Core.Models;

    public class PlayerBetLimitSelectMetaModel : ICustomerIdentityMetaModel, IProductMetaModel, ISiteNameMetaModel
    {
        public int CustId { get; set; }

        public int ProductId { get; set; }

        public int CurrencyId { get; set; }

        public string SiteName { get; set; }
    }
}
