using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using Fanex.Athena.PlayerAdmin.BetLimit.DataDomains;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Helpers;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Services
{
    public class RngKenoBetLimitService : IRngKenoBetLimitService
    {
        private readonly IRngKenoBetLimitRepository _repository;
        private readonly IPublisher _publisher;

        public RngKenoBetLimitService(IRngKenoBetLimitRepository repository, IPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId)
        {
            return await _repository.GetSettingAsync(custId);
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            var result = await _repository.UpdateSettingAsync(metaModel);

            var logEvent = BetLimitLogHelper.CreateLogEvent(
                metaModel,
                x => SportType.Keno.ToString(),
                result.Code == 0,
                new[] { LogActionDetailId.UpdatingMinBet, LogActionDetailId.UpdatingMaxBet, LogActionDetailId.UpdatingMaxBetPerDraw });

            if(logEvent is not null)
            {
                await _publisher.Publish(logEvent);
            }

            return result;
        }
    }
}