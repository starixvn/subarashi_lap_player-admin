using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using System;

namespace Fanex.Athena.PlayerAdmin.API.MetaModel
{
    public class PlayerBettingStatisticsMetaModel : IBalanceUpAndMoveBetMetaModel, ICustomerIdentityMetaModel, IProductCodeMetailModel
    {
        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public int CurrencyId { get; set; }

        public string ProductCodeList { get; set; } = ProductCode.SB;
    }
}