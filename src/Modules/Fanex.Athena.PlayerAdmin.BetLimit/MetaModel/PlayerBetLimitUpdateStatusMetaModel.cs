using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Core.Utils;

namespace Fanex.Athena.PlayerAdmin.BetLimit.MetaModel
{
    public class PlayerBetLimitUpdateStatusMetaModel : ICustomerIdentityMetaModel, IProductMetaModel, IValidationMetaModel,
        IUserIdMetaModel, ISiteNameMetaModel, IClientIPMetaModel, ILanguageMetaModel
    {
        public int CustId { get; set; }

        public string SiteName { get; set; }

        public int UserId { get; set; }

        public int CurrencyId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ProductId { get; set; }

        public int LicenseeCustId { get; set; }

        public bool Status { get; set; }

        public bool IsValid => true;
    }
}