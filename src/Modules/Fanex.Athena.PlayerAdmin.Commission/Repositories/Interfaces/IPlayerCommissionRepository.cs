using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Commission.Criteria;
using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Commission.Repositories.Interfaces
{
    public interface IPlayerCommissionRepository
    {
        Task<IEnumerable<PlayerCommissionViewModel>> GetAllCommGroups(GetAllCommGroupsCriteria criteria);

        Task<PlayerCommissionViewModel> GetCommissionGroupById(GetCommissionGroupByIdCriteria criteria);

        Task<IResultInfo> MultipleMoveGroup(MultipleMoveGroupCriteria criteria);

        Task<IResultInfo> MultipleRemoveFromGroup(MultipleRemoveFromGroupCriteria criteria);

        Task<IResultInfo> MultipleUpdateGroup(MultipleUpdateGroupCriteria criteria);

        Task<PlayerCommissionViewModel> GetPlayerCommissionGroup(GetPlayerCommissionGroupCriteria criteria);
    }
}