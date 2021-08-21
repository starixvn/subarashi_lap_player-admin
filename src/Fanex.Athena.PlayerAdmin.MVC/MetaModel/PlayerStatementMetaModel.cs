using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Core.Utils;
using System;

namespace Fanex.Athena.PlayerAdmin.MVC.MetaModel
{
    public class PlayerStatementMetaModel : ICustomerIdentityMetaModel, IValidationMetaModel ,ISiteNameMetaModel, ILanguageMetaModel
    {
        private const int MaximumYear = 2;

        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SiteName { get; set; }

        public string Language { get; set; }

        public int CurrencyId { get; set; }

        public bool IsValid => ToDate >= FromDate
                && FromDate.Month == ToDate.Month
                && FromDate.Year >= (DateTime.Today.Year - MaximumYear);
    }
}