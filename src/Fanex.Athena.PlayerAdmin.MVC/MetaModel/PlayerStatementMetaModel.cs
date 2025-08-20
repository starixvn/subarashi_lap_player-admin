namespace Fanex.Athena.PlayerAdmin.MVC.MetaModel
{
    using System;
    using Core.Helpers;
    using Core.Models;
    using Fanex.Core.Utils;

    public class PlayerStatementMetaModel : ICustomerIdentityMetaModel, IValidationMetaModel, ISiteNameMetaModel, ILanguageMetaModel, IShowBetFromIPMetaModel
    {
        private const int MaximumYear = 2;

        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SiteName { get; set; }

        public string Language { get; set; }

        public int CurrencyId { get; set; }

        public bool IsShowIp { get; set; }

        public bool IsShowIpVN { get; set; }

        public byte ViewType { get; set; }

        public bool IsValid => ToDate >= FromDate
                && FromDate.Year >= (DateTimeHelper.DatabaseServerToDay.Year - MaximumYear);
    }
}