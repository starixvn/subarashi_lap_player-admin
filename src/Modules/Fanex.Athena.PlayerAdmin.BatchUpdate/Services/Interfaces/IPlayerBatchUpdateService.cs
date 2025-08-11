namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Services
{
    using System.Threading.Tasks;
    using Core.Models;
    using MetaModels;

    public interface IPlayerBatchUpdateService
    {
        Task<IResultInfo> UpdateSpecialLevel(UpdateSpecialLevelApiMetaModel metaModel);

        Task<IResultInfo> UpdateOddsGroup(UpdateOddsGroupApiMetaModel metaModel);
    }
}