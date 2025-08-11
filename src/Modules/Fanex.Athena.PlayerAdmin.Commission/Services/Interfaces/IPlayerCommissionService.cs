namespace Fanex.Athena.PlayerAdmin.Commission.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Models;
    using MetaModels;
    using ViewModels;

    public interface IPlayerCommissionService
    {
        Task<IEnumerable<PlayerCommissionSettingViewModel>> InitCommValues(ProductAccess productAccess);

        Task<IEnumerable<PlayerCommissionViewModel>> GetAllCommGroups(BasePlayerCommissionUpdateQuery query, ProductAccess productAccess);

        Task<IResultInfo> MultipleMoveGroup(MultipleMoveGroupQuery query, ProductAccess productAccess);

        Task<IResultInfo> MultipleRemoveFromGroup(MultipleRemoveFromGroupQuery query);

        Task<IResultInfo> MultipleUpdateGroup(MultipleUpdateGroupQuery query);

        Task<PlayerCommissionViewModel> GetPlayerCommissionGroup(SingleMoveGroupQuery query, ProductAccess productAccess);
    }
}