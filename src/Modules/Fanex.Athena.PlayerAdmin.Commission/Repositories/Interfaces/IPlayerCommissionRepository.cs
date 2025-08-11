namespace Fanex.Athena.PlayerAdmin.Commission.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models;
    using Criteria;
    using ViewModels;

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