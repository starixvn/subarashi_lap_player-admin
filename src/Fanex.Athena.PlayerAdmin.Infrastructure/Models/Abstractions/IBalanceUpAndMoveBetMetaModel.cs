using System;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Models
{
    public interface IBalanceUpAndMoveBetMetaModel
    {
        DateTime MoveBetsDay { get; set; }

        DateTime BalanceUpDay { get; set; }
    }
}