using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.Core.Models;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Services.Interfaces
{
    public interface IPlayerBaStatusService
    {
        Task<ResultViewModel> Update(BaStatusUpdatingMetaModel metaModel);
    }
}