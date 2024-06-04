using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.BetLimit.DataDomains;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Helpers;
using Fanex.Athena.PlayerAdmin.BetLimit.Mediator;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using MediatR;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Services
{
    public class SportsBookBetLimitService : ISportsBookBetLimitService
    {
        private readonly IDcsGameBetLimitRepository _betSetting;
        private readonly IPublisher _publisher;
        private (string SportType, int BetType) MixParlaySports = (SportType.OtherSports.ToString(), BetType.Parlay);

        public SportsBookBetLimitService(
            IDcsGameBetLimitRepository betSettingRepository,
            IPublisher publisher)
        {
            _betSetting = betSettingRepository;
            _publisher = publisher;
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId, bool isShowMixedSport)
        {
            var sbSettings = await _betSetting.GetSettingAsync(custId, ProductTypes.Sportsbook);
            var result = isShowMixedSport
                ? sbSettings.ToList()
                : sbSettings.Where(_ => _.SportType != MixParlaySports.SportType && _.BetType != MixParlaySports.BetType);

            result = result.OrderBy(_ => _, new SBBetLimitComparer());

            return result;
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel, bool isShowMixedSport)
        {
            var result = await _betSetting.UpdateSettingAsync(metaModel);

            var logEvent = BetLimitLogHelper.CreateLogEvent(
                metaModel,
                GetDetailedLog(metaModel.Setting, result.Code == 0));

            if (result.Code > 0)
            {
                await PublishLogEvent(logEvent);
                return result;
            }

            await PublishLogEvent(logEvent);

            return result;
        }

        private static IEnumerable<PlayerBetLimitDetailedLog> GetDetailedLog(IEnumerable<PlayerBetLimitMetaModel> setting, bool isSuccessful)
        {
            return BetLimitLogHelper.BuildBetLimitLogDetail(
                 setting,
                 x => x.Id,
                 isSuccessful,
                 new[] {
                     LogActionDetailId.UpdatingMinBet,
                     LogActionDetailId.UpdatingMaxBet,
                     LogActionDetailId.UpdatingMaxPerMatch,
                     LogActionDetailId.UpdatingMaxPayoutPerMatch,
                 });
        }

        private async Task PublishLogEvent(PlayerUpdatedBetLimitEvent logEvent)
        {
            if (logEvent is not null)
            {
                await _publisher.Publish(logEvent);
            }
        }
    }

    internal class SBBetLimitComparer : IComparer<PlayerBetLimit>
    {
        private readonly IDictionary<string, int> BetLimitOrderer = new Dictionary<string, int>
        {
            {$"{SportType.Soccer}_{BetType.DefaultBetType}", 0 },
            {$"{SportType.SabaSoccer}_{BetType.DefaultBetType}", 1 },
            {$"{SportType.Soccer}_{BetType.Parlay}", 2 },
            {$"{SportType.Esports}_{BetType.DefaultBetType}", 3 },
            {$"{SportType.BasketBall}_{BetType.DefaultBetType}", 4 },
            {$"{SportType.SabaBasketBall}_{BetType.DefaultBetType}", 5 },
            {$"{SportType.FootBall}_{BetType.DefaultBetType}", 6 },
            {$"{SportType.Tennis}_{BetType.DefaultBetType}", 7 },
            {$"{SportType.BaseBall}_{BetType.DefaultBetType}", 8 },
            {$"{SportType.Golf}_{BetType.DefaultBetType}", 9 },
            {$"{SportType.Cricket}_{BetType.DefaultBetType}", 10 },
            {$"{SportType.SabaCricket}_{BetType.DefaultBetType}", 11 },
            {$"{SportType.Motorsports}_{BetType.DefaultBetType}", 12 },
            {$"{SportType.OtherSports}_{BetType.DefaultBetType}", 13 },
            {$"{SportType.OtherSports}_{BetType.Parlay}", 14 },
        };

        public int Compare(PlayerBetLimit x, PlayerBetLimit y)
        {
            if (x == null || y == null)
            {
                return 1;
            }

            if (!BetLimitOrderer.ContainsKey(x.Id) || !BetLimitOrderer.ContainsKey(y.Id))
            {
                return 1;
            }

            return BetLimitOrderer[x.Id] - BetLimitOrderer[y.Id];
        }
    }
}