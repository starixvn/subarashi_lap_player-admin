using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Commission.Services.Interfaces
{
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