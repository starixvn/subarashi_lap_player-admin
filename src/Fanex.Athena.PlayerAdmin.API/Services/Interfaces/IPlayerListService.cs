using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.ViewModels;

namespace Fanex.Athena.PlayerAdmin.API.Services.Interfaces
{
    public interface IPlayerListService
    {
        Task<PlayerListResultViewModel> Search(PlayerSearchingMetaModel metaModel);

        Task<PlayerListResultViewModel> GetPlayerListById(PlayerListByIdMetaModel metaModel);

        Task<IEnumerable<string>> CustomerClass(string licenseeName);

        Task<IEnumerable<CommGroupViewModel>> GetAllCommGroups(int licenseeId);
    }
}
