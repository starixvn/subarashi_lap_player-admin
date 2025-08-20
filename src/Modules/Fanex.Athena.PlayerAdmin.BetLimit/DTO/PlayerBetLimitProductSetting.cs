namespace Fanex.Athena.PlayerAdmin.BetLimit.DTO
{
    public class PlayerBetLimitProductSetting
    {
        public int ProductId { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }

        public bool? IsDisabled { get; set; }
    }
}