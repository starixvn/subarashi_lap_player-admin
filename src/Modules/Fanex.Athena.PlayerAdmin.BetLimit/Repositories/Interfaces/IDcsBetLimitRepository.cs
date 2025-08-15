namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models;
    using DTO;
    using MetaModel;

    public interface IDcsBetLimitRepository
    {
        Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId, int productId);

        Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel);

        Task<PlayerBetLimitProductSetting> GetStatusAsync(int custId, int productId);

        Task<IResultInfo> UpdateStatusAsync(PlayerBetLimitUpdateStatusMetaModel metaModel);
    }
}