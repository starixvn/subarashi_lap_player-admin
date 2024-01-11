using Fanex.Athena.PlayerAdmin.Core.Helpers;
using System;
using System.Runtime.Serialization;

namespace Fanex.Athena.PlayerAdmin.MVC.ViewModels
{
    public class PlayerDetailTransactionViewModel
    {
        public string TransactionId { get; set; }
        public DateTime TransactionTime { get; set; }
        public int BetType { get; set; }
        public string TransactionType { get; set; }
        public decimal BalanceChange { get; set; }
        public decimal AfterBalance { get; set; }
        public string Remark { get; set; }
        public bool IsResettled { get; set; }

        public string TransactionTimeStr => TransactionTime.ToString("MM/dd/yyyy HH:mm:ss");

        public string BalanceChangeStr =>
            (BalanceChange > 0 ? "+" : string.Empty) + CutOffValue(BalanceChange);

        public string AfterBalanceStr => CutOffValue(AfterBalance);

        private static string CutOffValue(decimal value) => string.Format(DataFormatHelper.DECIMALFORMAT20, DataFormatHelper.AsDecimal(value));
    }
}