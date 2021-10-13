using System;

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
            (BalanceChange > 0 ? "+" : string.Empty) + CutOffValue(BalanceChange).ToString("#,##0.##");

        public string AfterBalanceStr => CutOffValue(AfterBalance).ToString("N2");

        private static decimal CutOffValue(decimal value, int digits = 2)
        {
            var power = (decimal)Math.Pow(10, digits);
            var result = Math.Floor(Math.Abs(value) * power) / 100;
            return value >= 0 ? result : -result;
        }
    }
}