using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces
{
    public interface IAGCasinoBetLimitService
    {
        Task<IList<AgCasinoGame>> GetGroupBettingsAsync(string siteName, int currency);

        Task<AgCasinoBetSetting> GetBetSettingAsync(int custId);

        Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel> metaModel);
    }
}