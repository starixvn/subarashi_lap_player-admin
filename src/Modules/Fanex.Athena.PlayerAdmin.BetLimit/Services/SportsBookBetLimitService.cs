using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.BetLimit.DataDomains;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Helpers;
using Fanex.Athena.PlayerAdmin.BetLimit.Mediator;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Services
{
    public class SportsBookBetLimitService : ISportsBookBetLimitService
    {
        private const int EsportIndex = 2;

        private readonly ISportsBookBetLimitRepository _betSetting;
        private readonly IDcsBetLimitRepository _dcsBetSetting;
        private readonly IPublisher _publisher;

        public SportsBookBetLimitService(
            ISportsBookBetLimitRepository betSettingRepository,
            IDcsBetLimitRepository dcsBetSettingRepository,
            IPublisher publisher)
        {
            _betSetting = betSettingRepository;
            _dcsBetSetting = dcsBetSettingRepository;
            _publisher = publisher;
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId, bool isShowMixedSport)
        {
            var eSportSetting = await _dcsBetSetting.GetSettingAsync(custId, ProductType.Sportsbooks2);

            if (!eSportSetting.Any())
            {
                throw new InvalidOperationException("There is no E-sport setting data.");
            }

            var sbSettings = await _betSetting.GetSettingAsync(custId, isShowMixedSport);

            var result = sbSettings.ToList();
            var sbSettingTotal = sbSettings.Count();
            result.Insert(sbSettingTotal > EsportIndex ? EsportIndex : sbSettingTotal, eSportSetting.First());

            return result;
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel, bool isShowMixedSport)
        {
            var esportsTypeId = SportType.Esports.ToString();
            if (!metaModel.Setting.Any(x => x.Id == esportsTypeId))
            {
                return new ResultInfo { Code = 1, Message = "Invalid parameters. Setting is null." };
            }

            var result = await _betSetting.UpdateSettingAsync(metaModel, isShowMixedSport);

            var logEvent = BetLimitLogHelper.CreateLogEvent(
                metaModel,
                GetDetailedLog(metaModel.Setting.Where(x => x.Id != esportsTypeId), result.Code == 0));

            if (result.Code > 0)
            {
                await PublishLogEvent(logEvent);
                return result;
            }

            var esportsSetting = metaModel.Setting.Where(x => x.IsUpdated && x.Id == esportsTypeId).ToList();
            if (esportsSetting.Any())
            {
                var dcsMetaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
                {
                    CustId = metaModel.CustId,
                    UserId = metaModel.UserId,
                    SiteName = metaModel.SiteName,
                    Ip = metaModel.Ip,
                    Setting = esportsSetting,
                    ProductId = ProductType.Sportsbooks2,
                    LicenseeCustId = metaModel.LicenseeCustId
                };
                result = await _dcsBetSetting.UpdateSettingAsync(dcsMetaModel);

                if (logEvent is null)
                {
                    logEvent = BetLimitLogHelper.CreateLogEvent(
                        metaModel,
                        GetDetailedLog(esportsSetting, result.Code == 0));
                }
                else
                {
                    logEvent.Detail = logEvent.Detail.Concat(GetDetailedLog(esportsSetting, result.Code == 0));
                }
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
                 new[] { LogActionDetailId.UpdatingMinBet, LogActionDetailId.UpdatingMaxBet, LogActionDetailId.UpdatingMaxPerMatch });
        }

        private async Task PublishLogEvent(PlayerUpdatedBetLimitEvent logEvent)
        {
            if(logEvent is not null)
            {
                await _publisher.Publish(logEvent);
            }
        }
    }
}