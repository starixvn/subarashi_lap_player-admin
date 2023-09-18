using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.Core.Models;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces
{
    public interface IExchangeChargeCommService
    {
        Task<IResultInfo<double?>> GetChargeCommission(int custId, string language, int groupMode);
        Task<IResultInfo> UpdateChargeCommission(PlayerUpdateChargeCommMetaModel metaModel);
    }
}
