namespace Fanex.Athena.PlayerAdmin.BetLimit.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Constants;
    using Core.Models;
    using DataDomains;
    using DTO;
    using Helpers;
    using Interfaces;
    using MediatR;
    using MetaModel;
    using Repositories.Interfaces;

    public class DcsBetLimitService : IDcsBetLimitService
    {
        public static readonly IEnumerable<int> ProductsHasControlStatus = new[]
        {
            ProductTypes.Lottery,
            ProductTypes.I4D,
            ProductTypes.SLT,
            ProductTypes.AllBet,
            ProductTypes.Voidbridge,
            ProductTypes.CashOut,
            ProductTypes.Exchange,
            ProductTypes.PP,
            ProductTypes.SXG,
            ProductTypes.HB,
            ProductTypes.JK,
            ProductTypes.SAG,
            ProductTypes.PGS,
            ProductTypes.SBC,
            ProductTypes.WM,
            ProductTypes.SG,
            ProductTypes.VRG,
            ProductTypes.CC,
            ProductTypes.CG,
            ProductTypes.VGA,
            ProductTypes.TG,
            ProductTypes.AP,
            ProductTypes.APM,
            ProductTypes.SabaCasino,
            ProductTypes.BTC,
            ProductTypes.FGGL,
            ProductTypes.MXG,
            ProductTypes.JIG,
            ProductTypes.YB,
            ProductTypes.MGRNG,
            ProductTypes.SEA,
            ProductTypes.WE,
            ProductTypes.L22,
            ProductTypes.UUSL,
            ProductTypes.FS,
            ProductTypes.FAC,
            ProductTypes.NS,
            ProductTypes.ON,
            ProductTypes.PTRNG,
            ProductTypes.GPIRNG,
            ProductTypes.GPILC,
            ProductTypes.PS,
            ProductTypes.ASK,
            ProductTypes.FG,
            ProductTypes.HOT,
            ProductTypes.MGLC,
            ProductTypes.PTLC,
            ProductTypes.RGRNG,
            ProductTypes.WLC,
            ProductTypes.AIG,
            ProductTypes.CPRNG
        };

        private readonly IDcsGameBetLimitRepository _gameBetLimitRepository;
        private readonly INewDcsBetLimitRepository _newRepository;
        private readonly IPublisher _publisher;

        private readonly IDcsBetLimitRepository _repository;

        public DcsBetLimitService(
            IDcsBetLimitRepository repository,
            INewDcsBetLimitRepository newRepository,
            IDcsGameBetLimitRepository dcsGameBetLimitRepository,
            IPublisher publisher)
        {
            _repository = repository;
            _newRepository = newRepository;
            _publisher = publisher;
            _gameBetLimitRepository = dcsGameBetLimitRepository;
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId, int productId)
        {
            return productId switch
            {
                ProductTypes.Sportsbook or ProductTypes.NumberGame => await _gameBetLimitRepository.GetSettingAsync(custId, productId),
                ProductTypes.VS3 => await _newRepository.GetSettingAsync(custId, productId),
                _ => await _repository.GetSettingAsync(custId, productId)
            };
        }

        public async Task<bool?> GetStatusAsync(int custId, int productId)
        {
            if (!ProductsHasControlStatus.Contains(productId))
            {
                return null;
            }

            return await _repository.GetStatusAsync(custId, productId);
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            var result = metaModel.ProductId switch
            {
                ProductTypes.Sportsbook or ProductTypes.NumberGame => await _gameBetLimitRepository.UpdateSettingAsync(metaModel),
                ProductTypes.VS3 => await _newRepository.UpdateSettingAsync(metaModel),
                _ => await _repository.UpdateSettingAsync(metaModel)
            };

            var logEvent = BetLimitLogHelper.CreateLogEvent(
                metaModel,
                x => x.Id,
                result.Code == 0,
                GetBetSettingAction(metaModel.ProductId));

            if (logEvent is not null)
            {
                await _publisher.Publish(logEvent);
            }

            return result;
        }

        public async Task<IResultInfo> UpdateStatusAsync(PlayerBetLimitUpdateStatusMetaModel metaModel)
        {
            var result = await _repository.UpdateStatusAsync(metaModel);

            var @event = ProductStatusLogHelper.CreateLogEvent(metaModel, result.Code == 0);
            await _publisher.Publish(@event);

            return result;
        }

        public static int[] GetBetSettingAction(int productId)
        {
            return productId switch
            {
                ProductTypes.Lottery or ProductTypes.Keno => new[]
                {
                    LogActionDetailId.UpdatingMinBet,
                    LogActionDetailId.UpdatingMaxBet,
                    LogActionDetailId.UpdatingMaxBetPerDraw
                },
                ProductTypes.NumberGame => new[]
                {
                    LogActionDetailId.UpdatingMinBet,
                    LogActionDetailId.UpdatingMaxBet,
                    LogActionDetailId.UpdatingMaxPerMatch,
                    LogActionDetailId.UpdatingMaxPerBall
                },
                ProductTypes.Sportsbook => new[]
                {
                    LogActionDetailId.UpdatingMinBet,
                    LogActionDetailId.UpdatingMaxBet,
                    LogActionDetailId.UpdatingMaxPerMatch,
                    LogActionDetailId.UpdatingMaxPayoutPerMatch
                },
                ProductTypes.VS3 => new[]
                {
                    LogActionDetailId.UpdatingMinBet,
                    LogActionDetailId.UpdatingMaxBet
                },
                _ => new[]
                {
                    LogActionDetailId.UpdatingMinBet,
                    LogActionDetailId.UpdatingMaxBet,
                    LogActionDetailId.UpdatingMaxPerMatch
                }
            };
        }
    }
}