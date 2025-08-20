namespace Fanex.Athena.PlayerAdmin.BetLimit.Services
{
    using System.Threading.Tasks;
    using Core.Models;
    using DTO;
    using Helpers;
    using MediatR;
    using MetaModel;
    using Repositories;

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