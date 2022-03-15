using Fanex.Athena.PlayerAdmin.BetLimit.DataDomains;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Helpers;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Services
{
    public class DcsBetLimitService : IDcsBetLimitService
    {
        private static readonly int[] ProductsHasStatus = { ProductTypes.Lottery, ProductTypes.I4D, ProductTypes.SLT, ProductTypes.AllBet, ProductTypes.Voidbridge, ProductTypes.CashOut,
            ProductTypes.Exchange, ProductTypes.PP, ProductTypes.SXG, ProductTypes.GDG, ProductTypes.HB, ProductTypes.JK, ProductTypes.SAG, ProductTypes.PGS, ProductTypes.SBC, ProductTypes.WM,
            ProductTypes.SG, ProductTypes.VRG, ProductTypes.CC, ProductTypes.CG, ProductTypes.VGA, ProductTypes.TG, ProductTypes.AP, ProductTypes.APM };

        private readonly IDcsBetLimitRepository _repository;
        private readonly INewDcsBetLimitRepository _newRepository;
        private readonly IPublisher _publisher;

        public DcsBetLimitService(IDcsBetLimitRepository repository, INewDcsBetLimitRepository newRepository, IPublisher publisher)
        {
            _repository = repository;
            _newRepository = newRepository;
            _publisher = publisher;
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId, int productId)
        {
            return productId == ProductTypes.VS3 ? await _newRepository.GetSettingAsync(custId, productId) : await _repository.GetSettingAsync(custId, productId);
        }

        public async Task<bool> GetStatusAsync(int custId, int productId)
        {
            if (!ProductsHasStatus.Contains(productId))
            {
                return true;
            }

            return await _repository.GetStatusAsync(custId, productId);
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            var result = metaModel.ProductId == ProductTypes.VS3 ? await _newRepository.UpdateSettingAsync(metaModel) : await _repository.UpdateSettingAsync(metaModel);

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
                ProductTypes.Lottery => new[] { LogActionDetailId.UpdatingMinBet, LogActionDetailId.UpdatingMaxBet, LogActionDetailId.UpdatingMaxBetPerDraw },
                ProductTypes.VS3 => new[] { LogActionDetailId.UpdatingMinBet, LogActionDetailId.UpdatingMaxBet },
                _ => new[] { LogActionDetailId.UpdatingMinBet, LogActionDetailId.UpdatingMaxBet, LogActionDetailId.UpdatingMaxPerMatch },
            };
        }
    }
}