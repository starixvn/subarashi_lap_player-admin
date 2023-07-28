namespace Fanex.Athena.PlayerAdmin.BetLimit.Helpers
{
    public static class ExchangeProductHelper
    {
        private const string SiteName_12Bet = "12Bet";

        private const int GroupMode_12B = 0;
        private const int GroupMode_Other = 1;

        public static int GetGroupMode(string siteName)
            => siteName.Equals(SiteName_12Bet, System.StringComparison.InvariantCultureIgnoreCase) ? GroupMode_12B : GroupMode_Other;
    }
}
