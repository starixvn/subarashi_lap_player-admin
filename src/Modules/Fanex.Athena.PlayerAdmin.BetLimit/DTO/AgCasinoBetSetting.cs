namespace Fanex.Athena.PlayerAdmin.BetLimit.DTO
{
    public class AgCasinoBetSetting
    {
        public int CurrencyId { get; set; }

        public string AgGroup { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsLineEnabled { get; set; }

        public string[] CheckedGroups { get; set; }
    }
}
