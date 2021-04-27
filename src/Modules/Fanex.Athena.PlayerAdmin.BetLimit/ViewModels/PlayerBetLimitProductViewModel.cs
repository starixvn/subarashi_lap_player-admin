namespace Fanex.Athena.PlayerAdmin.BetLimit.ViewModels
{
    using System.Collections.Generic;

    public class PlayerBetLimitProductViewModel
    {
        public int CustId { get; set; }

        public int ProductId { get; set; }

        public int CurrencyId { get; set; }

        public int LicenseeCustId { get; set; }

        public IDictionary<int, string> ProductList { get; set; }
    }
}
