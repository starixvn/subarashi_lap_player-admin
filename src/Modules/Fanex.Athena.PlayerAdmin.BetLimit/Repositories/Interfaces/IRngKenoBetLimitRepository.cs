using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces
{
    public interface IRngKenoBetLimitRepository
    {
        Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId);

        Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel);
    }
}