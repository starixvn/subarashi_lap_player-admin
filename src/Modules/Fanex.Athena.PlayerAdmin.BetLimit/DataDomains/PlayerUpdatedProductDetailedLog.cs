namespace Fanex.Athena.PlayerAdmin.BetLimit.DataDomains
{
    public class PlayerUpdatedProductDetailedLog
    {
        public int DetailedId { get; set; }

        public string ProductId { get; set; }

        public string Status { get; set; }

        public string Old { get; set; }

        public string New { get; set; }
    }
}
