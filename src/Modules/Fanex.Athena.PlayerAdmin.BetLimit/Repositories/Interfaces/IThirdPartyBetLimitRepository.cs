namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    using System.Threading.Tasks;
    using Core.Models;
    using DTO;
    using MetaModel;

    public interface IThirdPartyBetLimitRepository
    {
        Task<ThirdPartyBetSetting> GetSettingAsync(int custId, int productId);

        Task<IResultInfo> UpdateStatusAsync(PlayerBetLimitUpdateStatusMetaModel metaModel);
    }
}