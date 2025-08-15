namespace Fanex.Athena.PlayerAdmin.Core.Helpers
{
    using System.Collections.Generic;
    using System.Text;

    public static class XmlParameterBuilder
    {
        public static string BuildRootXml(string innerXml)
        {
            return $"<ROOT>{innerXml}</ROOT>";
        }

        public static string BuildBetLimitElement(string sportType, int betType, decimal minBet, decimal maxBet, decimal maxPerMatch)
        {
            return string.Format(
                "<BetLimit SportType=\"{0}\" BetType=\"{1}\" MinBet=\"{2}\" MaxBet=\"{3}\" MaxPerMatch=\"{4}\" ></BetLimit>",
                sportType,
                betType,
                minBet,
                maxBet,
                maxPerMatch);
        }

        public static string BuildGameBetLimitElement(string sportType, int betType, decimal minBet, decimal maxBet, decimal maxPerMatch, decimal maxPayoutPerMatch)
        {
            return string.Format(
                "<BetLimit SportType=\"{0}\" BetType=\"{1}\" MinBet=\"{2}\" MaxBet=\"{3}\" MaxPerMatch=\"{4}\" MaxPayoutPerMatch=\"{5}\"></BetLimit>",
                sportType,
                betType,
                minBet,
                maxBet,
                maxPerMatch,
                maxPayoutPerMatch);
        }

        public static string BuildNumberGameBetLimitElement(string sportType, int betType, decimal minBet, decimal maxBet, decimal maxPerMatch, decimal maxPerNumber)
        {
            return string.Format(
                "<BetLimit SportType=\"{0}\" BetType=\"{1}\" MinBet=\"{2}\" MaxBet=\"{3}\" MaxPerMatch=\"{4}\" MaxPerNumber=\"{5}\"></BetLimit>",
                sportType,
                betType,
                minBet,
                maxBet,
                maxPerMatch,
                maxPerNumber);
        }

        public static string BuildNewBetLimitElement(string sportType, int betType, decimal minBet, decimal maxBet)
        {
            return string.Format(
                "<BetLimit SportType=\"{0}\" BetType=\"{1}\" MinBet=\"{2}\" MaxBet=\"{3}\" ></BetLimit>",
                sportType,
                betType,
                minBet,
                maxBet);
        }

        public static string CombineElements(IEnumerable<string> elements)
        {
            var builder = new StringBuilder();
            foreach (var element in elements)
            {
                builder.Append(element);
            }
            return builder.ToString();
        }
    }
}
