using Fanex.Athena.PlayerAdmin.BetLimit.DataDomains;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Helpers;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Services
{
    public class NumberGameBetLimitService : INumberGameBetLimitService
    {
        private readonly INumberGameBetLimitRepository _repository;
        private readonly IPublisher _publisher;

        public NumberGameBetLimitService(INumberGameBetLimitRepository repository, IPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId) => _repository.GetSettingAsync(custId);

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            var result = await _repository.UpdateSettingAsync(metaModel).ConfigureAwait(false);

            var logEvent = BetLimitLogHelper.CreateLogEvent(
                metaModel,
                x => x.Id,
                result.Code == 0,
                new[] { LogActionDetailId.UpdatingMinBet, LogActionDetailId.UpdatingMaxBet, LogActionDetailId.UpdatingMaxPerMatch, LogActionDetailId.UpdatingMaxPerBall });

            if (logEvent is not null)
            {
                await _publisher.Publish(logEvent);
            }
            return result;
        }
    }
}