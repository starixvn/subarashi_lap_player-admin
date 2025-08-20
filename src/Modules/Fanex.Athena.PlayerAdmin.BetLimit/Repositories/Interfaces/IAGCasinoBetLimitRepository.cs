namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces
{
    using System.Threading.Tasks;
    using Core.Models;
    using DTO;
    using MetaModel;

    public interface IAGCasinoBetLimitRepository
    {
        Task<AgCasinoBetSetting> GetBetSettingAsync(int custId);

        Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel> metaModel);

        Task<IResultInfo> UpdateStatusAsync(PlayerBetLimitUpdateStatusMetaModel metaModel);
    }
}