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
                var sportId = getSportTypeFunc?.Invoke(item);
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

        private static PlayerBetLimitDetailedLog CreateLogDetailItemByAction(
            int logDetailId,
            string sportId,
            string status,
            PlayerBetLimitMetaModel item)
        {
            if (logDetailId == LogActionDetailId.UpdatingMinBet && item.MinBet != item.OldMinBet)
            {
                return CreateLogDetailItem(logDetailId, sportId, status, item.OldMinBet, item.MinBet);
            }

            if (logDetailId == LogActionDetailId.UpdatingMaxBet && item.MaxBet != item.OldMaxBet)
            {
                return CreateLogDetailItem(logDetailId, sportId, status, item.OldMaxBet, item.MaxBet);
            }

            if ((logDetailId == LogActionDetailId.UpdatingMaxPerMatch
                || logDetailId == LogActionDetailId.UpdatingMaxBetPerDraw)
                && item.MaxPerMatch != item.OldMaxPerMatch)
            {
                return CreateLogDetailItem(logDetailId, sportId, status, item.OldMaxPerMatch, item.MaxPerMatch);
            }

            if (logDetailId == LogActionDetailId.UpdatingMaxPerBall && item.MaxPerBall != item.OldMaxPerBall)
            {
                return CreateLogDetailItem(logDetailId, sportId, status, item.OldMaxPerBall, item.MaxPerBall);
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