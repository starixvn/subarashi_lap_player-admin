using Fanex.Athena.PlayerAdmin.Core.Models;

namespace Fanex.Athena.PlayerAdmin.API.MetaModel
{
    public class PlayerUpdatingBaseMetaModel : ICustomerIdentityMetaModel, IClientIPMetaModel, IUserIdMetaModel, ILanguageMetaModel, ISiteNameMetaModel
    {
        public int UserId { get; set; }

        public int CustId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int CurrencyId { get; set; }

        public string SiteName { get; set; }

        public string OldValue { get; set; }

        public int LicenseeCustId { get; set; }
    }
}