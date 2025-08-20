namespace Fanex.Athena.PlayerAdmin.Core.Models
{
    using System;

    public interface IBalanceUpAndMoveBetMetaModel
    {
        DateTime MoveBetsDay { get; set; }

        DateTime BalanceUpDay { get; set; }
    }
}