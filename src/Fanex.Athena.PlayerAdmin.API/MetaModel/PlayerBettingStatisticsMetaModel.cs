using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using System;

namespace Fanex.Athena.PlayerAdmin.API.MetaModel
{
    public class PlayerBettingStatisticsMetaModel : IBalanceUpAndMoveBetMetaModel, ICustomerIdentityMetalModel
    {
        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public int CurrencyId { get; set; }
    }
}