using Fanex.Athena.PlayerAdmin.Commission.Criteria;
using Fanex.Athena.PlayerAdmin.Commission.Helpers;
using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Athena.PlayerAdmin.Commission.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.Commission.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Commission.Services
{
    public class PlayerCommissionService : IPlayerCommissionService
    {
        private readonly IPlayerCommissionRepository _repository;

        public PlayerCommissionService(IPlayerCommissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PlayerCommissionSettingViewModel>> InitCommValues(ProductAccess productAccess)
        {
            var commSettings = await _repository.GetAllCommSettingAsync(productAccess).ConfigureAwait(false);
            commSettings = commSettings.Where(_ => CommissionGroupHelper.GetProductPermissions(productAccess, _.ProductCode))
                .OrderBy(_ => CommissionGroupHelper.ProductListComparer(_.ProductCode)).ThenBy(_ => _.ProductCode); ;

            return commSettings;
        }

        public async Task<IEnumerable<PlayerCommissionViewModel>> GetAllCommGroups(BasePlayerCommissionUpdateQuery query, ProductAccess productAccess)
        {
            var criteria = new GetAllCommGroupsCriteria(query);
            var (comms, commValues) = await _repository.GetAllCommGroups(criteria, productAccess).ConfigureAwait(false);

            return MergeCommSetting(comms, commValues, productAccess);
        }

        private static IList<PlayerCommissionViewModel> MergeCommSetting(IEnumerable<PlayerCommissionViewModel> comms, IEnumerable<PlayerCommissionSettingViewModel> comValues, ProductAccess productAccess)
        {
            return comms.GroupJoin(comValues, c => c.GroupCommId, d => d.GroupCommId,
                (c, ds) => new PlayerCommissionViewModel
                {
                    GroupCommId = c.GroupCommId,
                    GroupName = c.GroupName,
                    CommValues = ds.Where(_ => CommissionGroupHelper.GetProductPermissions(productAccess, _.ProductCode)).OrderBy(_ => _, new GroupCommSettingComparer()).ThenBy(_ => _.ProductCode),
                }).ToList();
        }

        private static PlayerCommissionViewModel MergeCommSetting(PlayerCommissionViewModel comm, IEnumerable<PlayerCommissionSettingViewModel> commValues, ProductAccess productAccess)
        {
            comm.CommValues = commValues.Where(_ => CommissionGroupHelper.GetProductPermissions(productAccess, _.ProductCode)).OrderBy(_ => _, new GroupCommSettingComparer()).ThenBy(_ => _.ProductCode);

            return comm;
        }

        public async Task<IResultInfo> MultipleMoveGroup(MultipleMoveGroupQuery query, ProductAccess productAccess)
        {
            var (comm, commValues) = await _repository.GetCommissionGroupById(new GetCommissionGroupByIdCriteria(query), productAccess);
            var groupInfo = MergeCommSetting(comm, commValues, productAccess);

            if (groupInfo?.CommValues == null)
            {
                return new ResultInfo { Code = 1, Message = ViewResources.GroupNotExists };
            }

            var criteria = new MultipleMoveGroupCriteria(query);

            return await _repository.MultipleMoveGroup(criteria);
        }

        public async Task<IResultInfo> MultipleRemoveFromGroup(MultipleRemoveFromGroupQuery query)
        {
            var criteria = new MultipleRemoveFromGroupCriteria(query);

            return await _repository.MultipleRemoveFromGroup(criteria);
        }

        public async Task<IResultInfo> MultipleUpdateGroup(MultipleUpdateGroupQuery query)
        {
            var criteria = new MultipleUpdateGroupCriteria(query);

            return await _repository.MultipleUpdateGroup(criteria);
        }

        public async Task<PlayerCommissionViewModel> GetPlayerCommissionGroup(SingleMoveGroupQuery query, ProductAccess productAccess)
        {
            var criteria = new GetPlayerCommissionGroupCriteria(query);

            var (comms, commValues) = await _repository.GetPlayerCommissionGroup(criteria, productAccess).ConfigureAwait(false);

            return MergeCommSetting(comms, commValues, productAccess);
        }
    }
}