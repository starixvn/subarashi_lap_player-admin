namespace Fanex.Athena.PlayerAdmin.BetLimit.DTO
{
    public class PlayerBetLimit
    {
        public string Id { get; set; }

        public int BetType { get; set; }

        public string BetSettingType { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal MaxPerMatchMax { get; set; }

        public decimal MaxPerMatchMin { get; set; }

        public decimal BetValueMax { get; set; }

        public decimal BetValueMin { get; set; }

        public decimal MaxPerBall { get; set; }

        public decimal MaxPerBallMax { get; set; }

        public bool? IsEnable { get; set; }
    }
}
