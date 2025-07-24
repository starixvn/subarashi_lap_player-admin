using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Commission.Criteria;
using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Commission.Repositories.Interfaces
{
    public interface IPlayerCommissionRepository
    {
        Task<IEnumerable<PlayerCommissionSettingViewModel>> GetAllCommSettingAsync(ProductAccess productAccess);

        Task<(IEnumerable<PlayerCommissionViewModel>, IEnumerable<PlayerCommissionSettingViewModel>)> GetAllCommGroups(GetAllCommGroupsCriteria criteria, ProductAccess productAccess);

        Task<(PlayerCommissionViewModel, IEnumerable<PlayerCommissionSettingViewModel>)> GetCommissionGroupById(GetCommissionGroupByIdCriteria criteria, ProductAccess productAccess);

        Task<IResultInfo> MultipleMoveGroup(MultipleMoveGroupCriteria criteria);

        Task<IResultInfo> MultipleRemoveFromGroup(MultipleRemoveFromGroupCriteria criteria);

        Task<IResultInfo> MultipleUpdateGroup(MultipleUpdateGroupCriteria criteria);

        Task<(PlayerCommissionViewModel, IEnumerable<PlayerCommissionSettingViewModel>)> GetPlayerCommissionGroup(GetPlayerCommissionGroupCriteria criteria, ProductAccess productAccess);
    }
}