namespace Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models;
    using DTO;
    using MetaModel;

    public interface IAGCasinoBetLimitService
    {
        Task<IList<AgCasinoGame>> GetGroupBettingsAsync(string siteName, int currency);

        Task<AgCasinoBetSetting> GetBetSettingAsync(int custId);

        Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel> metaModel);

        Task<IResultInfo> UpdateStatusAsync(PlayerBetLimitUpdateStatusMetaModel metaModel);
    }
}