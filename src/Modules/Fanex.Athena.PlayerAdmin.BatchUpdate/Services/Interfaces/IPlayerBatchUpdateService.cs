using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Services
{
    public interface IPlayerBatchUpdateService
    {
        Task<IResultInfo> UpdateSpecialLevel(UpdateSpecialLevelApiMetaModel metaModel);

        Task<IResultInfo> UpdateOddsGroup(UpdateOddsGroupApiMetaModel metaModel);

        Task<IResultInfo> UpdateBAStatus(UpdateBAStatusApiMetaModel metaModel);
    }
}