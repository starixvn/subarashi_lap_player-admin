using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.BetLimit.DataDomains;
using Fanex.Athena.PlayerAdmin.BetLimit.Mediator;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Helpers
{
    public static class BetLimitLogHelper
    {
        public const string Success = "Success";
        public const string False = "False";

        public static PlayerUpdatedBetLimitEvent CreateLogEvent(
            PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel,
            Func<PlayerBetLimitMetaModel, string> getSportTypeFunc,
            bool isSuccessful,
            int[] logDetailIds)
        {
            var detail = BuildBetLimitLogDetail(metaModel.Setting, getSportTypeFunc, isSuccessful, logDetailIds);

            return CreateLogEvent(metaModel, detail);
        }

        public static PlayerUpdatedBetLimitEvent CreateLogEvent(PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel> metaModel, bool isSuccessful)
        {
            return CreateLogEvent(metaModel, new[] { BuildGroupLogDetail(metaModel.Setting, isSuccessful) });
        }

        public static PlayerUpdatedBetLimitEvent CreateLogEvent<T>(PlayerBetLimitUpdateMetaModel<T> metaModel, IEnumerable<PlayerBetLimitDetailedLog> detail)
        {
            return detail.Any()
                ? new PlayerUpdatedBetLimitEvent
                {
                    CustID = metaModel.CustId,
                    UserId = metaModel.UserId,
                    SiteName = metaModel.SiteName,
                    LicenseeCustId = metaModel.LicenseeCustId,
                    Detail = detail
                }
                : null;
        }

        public static IEnumerable<PlayerBetLimitDetailedLog> BuildBetLimitLogDetail(
            IEnumerable<PlayerBetLimitMetaModel> setting,
            Func<PlayerBetLimitMetaModel, string> getSportTypeFunc,
            bool isSuccessful,
            int[] logDetailIds)
        {
            var status = isSuccessful ? Success : False;
            var updatedItems = setting.Where(x => x.IsUpdated);
            foreach (var item in updatedItems)
            {
                var uSportId = getSportTypeFunc?.Invoke(item);
                var sportId = GenSportIdForLog(uSportId);
                for (var i = 0; i < logDetailIds.Length; i++)
                {
                    var logDetailId = logDetailIds[i];
                    var betLimitLogDetailItem = CreateLogDetailItemByAction(logDetailId, sportId, status, item);

                    if (betLimitLogDetailItem != null)
                    {
                        yield return betLimitLogDetailItem;
                    }
                }
            }
        }

        private static string GenSportIdForLog(string uSportId)
        {
            var splitParam = uSportId.Split('_');
            const int minParam = 2;
            var sportId = splitParam[0];
            if (splitParam.Count() >= minParam)
            {
                var sportType = Convert.ToInt32(splitParam[0]);
                var betType = Convert.ToInt32(splitParam[1]);
                sportId = sportType.ToString();
                if (betType == BetType.Parlay)
                {
                    sportId = sportType switch
                    {
                        SportType.Soccer => SportTypeExtend.SoccerParlay,
                        SportType.OtherSports => SportTypeExtend.MixSportsParlay,
                        _ => sportType.ToString()
                    };
                }
            }
            return sportId;
        }

        private static bool IsMinBetActionDetail(int logDetailId, PlayerBetLimitMetaModel item)
        {
            return logDetailId == LogActionDetailId.UpdatingMinBet && item.MinBet != item.OldMinBet;
        }

        private static bool IsMaxBetActionDetail(int logDetailId, PlayerBetLimitMetaModel item)
        {
            return logDetailId == LogActionDetailId.UpdatingMaxBet && item.MaxBet != item.OldMaxBet;
        }

        private static bool IsMaxPerMatchActionDetail(int logDetailId, PlayerBetLimitMetaModel item)
        {
            return (logDetailId == LogActionDetailId.UpdatingMaxPerMatch
                || logDetailId == LogActionDetailId.UpdatingMaxBetPerDraw)
                && item.MaxPerMatch != item.OldMaxPerMatch;
        }

        private static bool IsMaxPerBallActionDetail(int logDetailId, PlayerBetLimitMetaModel item)
        {
            return logDetailId == LogActionDetailId.UpdatingMaxPerBall && item.MaxPerBall != item.OldMaxPerBall;
        }

        public static bool IsMaxPayoutPerMatch(int logDetailId, PlayerBetLimitMetaModel item)
        {
            return logDetailId == LogActionDetailId.UpdatingMaxPayoutPerMatch && item.MaxPayoutPerMatch != item.OldMaxPayoutPerMatch;
        }

        private static PlayerBetLimitDetailedLog CreateLogDetailItemByAction(
            int logDetailId,
            string sportId,
            string status,
            PlayerBetLimitMetaModel item)
        {
            if (IsMinBetActionDetail(logDetailId, item))
            {
                return CreateLogDetailItem(logDetailId, sportId, status, item.OldMinBet, item.MinBet);
            }

            if (IsMaxBetActionDetail(logDetailId, item))
            {
                return CreateLogDetailItem(logDetailId, sportId, status, item.OldMaxBet, item.MaxBet);
            }

            if (IsMaxPerMatchActionDetail(logDetailId, item))
            {
                return CreateLogDetailItem(logDetailId, sportId, status, item.OldMaxPerMatch, item.MaxPerMatch);
            }

            if (IsMaxPerBallActionDetail(logDetailId, item))
            {
                return CreateLogDetailItem(logDetailId, sportId, status, item.OldMaxPerBall, item.MaxPerBall);
            }

            if (IsMaxPayoutPerMatch(logDetailId, item))
            {
                return CreateLogDetailItem(logDetailId, sportId, status, item.OldMaxPayoutPerMatch, item.MaxPayoutPerMatch);
            }

            return null;
        }

        private static PlayerBetLimitDetailedLog BuildGroupLogDetail(AgCasinoBetLimitMetaModel setting, bool isSuccessful)
        {
            return new PlayerBetLimitDetailedLog
            {
                DetailedId = LogActionDetailId.UpdatingAGGroup,
                SportType = SportType.AgCasino.ToString(),
                Status = isSuccessful ? Success : False,
                Old = setting.OldGroup,
                New = setting.Group
            };
        }

        private static PlayerBetLimitDetailedLog CreateLogDetailItem(
            int logDetailId,
            string id,
            string status,
            decimal oldValue,
            decimal newValue)
        {
            return new PlayerBetLimitDetailedLog
            {
                DetailedId = logDetailId,
                SportType = id,
                Status = status,
                Old = oldValue.ToString(CultureInfo.InvariantCulture),
                New = newValue.ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}