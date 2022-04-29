using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Core.Utils;

namespace Fanex.Athena.PlayerAdmin.BetLimit.MetaModel
{
    public class PlayerUpdateChargeCommMetaModel : ICustomerIdentityMetaModel, IValidationMetaModel,
        IUserIdMetaModel, ISiteNameMetaModel, IClientIPMetaModel, ILanguageMetaModel
    {
        public int CustId { get; set; }
        public int LicenseeCustId { get; set; }

        public int CurrencyId { get; set; }

        public int UserId { get; set; }

        public string SiteName { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public double Comm { get; set; }

        public double OldComm { get; set; }

        public bool IsValid => true;
    }
}
