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
            ProductTypes.SG, ProductTypes.VRG, ProductTypes.CC, ProductTypes.CG, ProductTypes.VGA, ProductTypes.TG };

        private readonly IDcsBetLimitRepository _repository;
        private readonly IPublisher _publisher;

        public DcsBetLimitService(IDcsBetLimitRepository repository, IPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId, int productId)
        {
            return await _repository.GetSettingAsync(custId, productId);
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
            var result = await _repository.UpdateSettingAsync(metaModel);

            var logEvent = BetLimitLogHelper.CreateLogEvent(
                metaModel,
                x => x.Id,
                result.Code == 0,
                metaModel.ProductId == ProductTypes.Lottery
                    ? new[] { LogActionDetailId.UpdatingMinBet, LogActionDetailId.UpdatingMaxBet, LogActionDetailId.UpdatingMaxBetPerDraw }
                    : new[] { LogActionDetailId.UpdatingMinBet, LogActionDetailId.UpdatingMaxBet, LogActionDetailId.UpdatingMaxPerMatch });

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
    }
}