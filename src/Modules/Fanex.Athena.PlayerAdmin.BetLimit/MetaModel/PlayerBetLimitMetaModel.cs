namespace Fanex.Athena.PlayerAdmin.BetLimit.MetaModel
{
    public class PlayerBetLimitMetaModel
    {
        public string Id { get; set; }

        public int BetType { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal MaxPerBall { get; set; }

        public decimal OldMinBet { get; set; }

        public decimal OldMaxBet { get; set; }

        public decimal OldMaxPerMatch { get; set; }

        public decimal OldMaxPerBall { get; set; }

        public bool IsUpdated { get; set; }
    }
}