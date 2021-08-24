using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Helpers;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Models;
using MediatR;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Services
{
    public class ThirdPartyBetLimitService : IThirdPartyBetLimitService
    {
        private readonly IThirdPartyBetLimitRepository _repository;
        private readonly IPublisher _publisher;

        public ThirdPartyBetLimitService(
            IThirdPartyBetLimitRepository repository,
            IPublisher publisher
            )
        {
            _repository = repository;
            _publisher = publisher;
        }

        public Task<ThirdPartyBetSetting> GetSettingAsync(int custId, int productId)
            => _repository.GetSettingAsync(custId, productId);

        public async Task<IResultInfo> UpdateStatusAsync(PlayerBetLimitUpdateStatusMetaModel metaModel)
        {
            var result = await _repository.UpdateStatusAsync(metaModel);
            var @event = ProductStatusLogHelper.CreateLogEvent(metaModel, result.Code == 0);
            await _publisher.Publish(@event);
            return result;
        }
    }
}