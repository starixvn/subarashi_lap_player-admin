using System;

namespace Fanex.Athena.PlayerAdmin.Core.Models
{
    public interface IBalanceUpAndMoveBetMetaModel
    {
        DateTime MoveBetsDay { get; set; }

        DateTime BalanceUpDay { get; set; }
    }
}