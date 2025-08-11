namespace Fanex.Athena.PlayerAdmin.BetLimit.Services
{
    using System.Threading.Tasks;
    using Core.Models;
    using DTO;
    using MetaModel;

    public interface IThirdPartyBetLimitService
    {
        Task<ThirdPartyBetSetting> GetSettingAsync(int custId, int productId);

        Task<IResultInfo> UpdateStatusAsync(PlayerBetLimitUpdateStatusMetaModel metaModel);
    }
}