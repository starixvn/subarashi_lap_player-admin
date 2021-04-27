namespace Fanex.Athena.PlayerAdmin.API.DTO
{
    public class PlayerBettingStatistics
    {
        public int WonTicketCount { get; set; }

        public int LoseTicketCount { get; set; }

        public int DrawTicketCount { get; set; }

        public int CancelledTicketCount { get; set; }

        public decimal Margin { get; set; }

        public decimal AverageBetSize { get; set; }

        public decimal Winnings { get; set; }

        public decimal Loss { get; set; }

        public decimal NetWinLoss => Winnings + Loss;

        public bool HasData => WonTicketCount > 0 || LoseTicketCount > 0 || DrawTicketCount > 0 || CancelledTicketCount > 0;
    }
}