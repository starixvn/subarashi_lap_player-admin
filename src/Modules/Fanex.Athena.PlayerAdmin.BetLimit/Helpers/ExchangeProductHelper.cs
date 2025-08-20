namespace Fanex.Athena.PlayerAdmin.BetLimit.Helpers
{
    using System;

    public static class ExchangeProductHelper
    {
        private const string SiteName_12Bet = "12Bet";

        private const int GroupMode_12B = 0;
        private const int GroupMode_Other = 1;

        public static int GetGroupMode(string siteName)
            => string.Equals(siteName, SiteName_12Bet, StringComparison.InvariantCultureIgnoreCase) ? GroupMode_12B : GroupMode_Other;
    }
}
