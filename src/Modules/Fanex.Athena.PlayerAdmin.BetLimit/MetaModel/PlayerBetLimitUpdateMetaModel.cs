namespace Fanex.Athena.PlayerAdmin.BetLimit.MetaModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Models;
    using Fanex.Core.Utils;

    public class PlayerBetLimitUpdateMetaModel<T> : ICustomerIdentityMetaModel, IProductMetaModel, IValidationMetaModel,
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

        public T Setting { get; set; }

        public bool IsValid
        {
            get
            {
                var result = !Equals(Setting, default(T));
                if (result && Setting is IEnumerable<object> enumerable)
                {
                    return enumerable.Any();
                }

                return result;
            }
        }
    }
}