

using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using NSubstitute;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.TestHelper
{
    public static class DataReaderHelper
    {
        public static void AssignCommonBetLimitField(this IDataReader dataReader, IEnumerable<PlayerBetLimit> playerBetLimits)
        {
            var firstItem = playerBetLimits.First();
            var remainItems = playerBetLimits.Skip(1);

            dataReader["minbet"].Returns(firstItem.MinBet, remainItems.Select(_ =>(object) _.MinBet).ToArray());
            dataReader["minbetmin"].Returns(firstItem.BetValueMin, remainItems.Select(_ => (object)_.BetValueMin).ToArray());
            dataReader["maxbet"].Returns(firstItem.MaxBet, remainItems.Select(_ => (object)_.MaxBet).ToArray());
            dataReader["maxbetmax"].Returns(firstItem.BetValueMax, remainItems.Select(_ => (object)_.BetValueMax).ToArray());
            dataReader["sporttype"].Returns(firstItem.SportType, remainItems.Select(_ => (object)_.SportType).ToArray());
            dataReader["bettype"].Returns(firstItem.BetType, remainItems.Select(_ => (object)_.BetType).ToArray());
        }

        public static void AssignDCSGameField(this IDataReader dataReader, IEnumerable<PlayerBetLimit> playerBetLimits)
        {
            var firstItem = playerBetLimits.First();
            var remainItems = playerBetLimits.Skip(1);

            dataReader["maxpermatch"].Returns(firstItem.MaxPerMatch, remainItems.Select(_ => (object)_.MaxPerMatch).ToArray());
            dataReader["maxpermatchmax"].Returns(firstItem.MaxPerMatchMax, remainItems.Select(_ => (object)_.MaxPerMatchMax).ToArray());
            dataReader["maxpayoutpermatch"].Returns(firstItem.MaxPayoutPerMatch, remainItems.Select(_ => (object)_.MaxPayoutPerMatch).ToArray());
            dataReader["maxpayoutpermatchmax"].Returns(firstItem.MaxPayoutPerMatchMax, remainItems.Select(_ => (object)_.MaxPayoutPerMatchMax).ToArray());
            dataReader["maxpernumber"].Returns(firstItem.MaxPerBall, remainItems.Select(_ => (object)_.MaxPerBall).ToArray());
            dataReader["maxpernumbermax"].Returns(firstItem.MaxPerBallMax, remainItems.Select(_ => (object)_.MaxPerBallMax).ToArray());
        }
    }
}
