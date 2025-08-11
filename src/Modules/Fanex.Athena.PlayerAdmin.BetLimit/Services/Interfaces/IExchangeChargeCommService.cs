namespace Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces
{
    using System.Threading.Tasks;
    using Core.Models;
    using MetaModel;

    public interface IExchangeChargeCommService
    {
        Task<IResultInfo<double?>> GetChargeCommission(int custId, string language, int groupMode);
        Task<IResultInfo> UpdateChargeCommission(PlayerUpdateChargeCommMetaModel metaModel);
    }
}
