namespace Fanex.Athena.PlayerAdmin.BetLimit.DataDomains
{
    public class PlayerBetLimitDetailedLog
    {
        public int DetailedId { get; set; }

        public string SportType { get; set; }

        public string Status { get; set; }

        public string Old { get; set; }

        public string New { get; set; }
    }
}
