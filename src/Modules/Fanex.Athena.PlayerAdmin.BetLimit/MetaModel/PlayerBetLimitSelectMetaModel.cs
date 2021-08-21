using Fanex.Athena.PlayerAdmin.Core.Models;

namespace Fanex.Athena.PlayerAdmin.BetLimit.MetaModel
{

    public class PlayerBetLimitSelectMetaModel : ICustomerIdentityMetaModel, IProductMetaModel
    {
        public int CustId { get; set; }

        public int ProductId { get; set; }

        public int CurrencyId { get; set; }
    }
}
