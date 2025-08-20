namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models;
    using DTO;
    using MetaModel;

    public interface ISportsBookBetLimitRepository
    {
        Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId, bool isShowMixedSport);

        Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel, bool isShowMixedSport);
    }
}