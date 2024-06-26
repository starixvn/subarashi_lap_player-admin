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

        public async Task<IEnumerable<PlayerCommissionViewModel>> GetAllCommGroups(BasePlayerCommissionUpdateQuery query)
        {
            var criteria = new GetAllCommGroupsCriteria(query);
            var (comms, commValues) = await _repository.GetAllCommGroups(criteria).ConfigureAwait(false);

            return MergeCommSetting(comms, commValues, criteria.Permissions);
        }

        private IList<PlayerCommissionViewModel> MergeCommSetting(IEnumerable<PlayerCommissionViewModel> comms, IEnumerable<PlayerCommissionSettingViewModel> comValues, IDictionary<string, bool> permissions)
        {
            comValues = MapCommProductSetting(comValues, permissions);

            return comms.GroupJoin(comValues, c => c.GroupCommId, d => d.GroupCommId,
                (c, ds) => new PlayerCommissionViewModel
                {
                    GroupCommId = c.GroupCommId,
                    GroupName = c.GroupName,
                    CommValues = ds.ToDictionary(_ => CommissionGroupHelper.GetProductCode(_.Sporttype, _.Bettype), _ => _)
                }).ToList();
        }

        private PlayerCommissionViewModel MergeCommSetting(PlayerCommissionViewModel comm, IEnumerable<PlayerCommissionSettingViewModel> commValues, IDictionary<string, bool> permissions)
        {
            commValues = MapCommProductSetting(commValues, permissions);
            comm.CommValues = commValues.ToDictionary(_ => CommissionGroupHelper.GetProductCode(_.Sporttype, _.Bettype), _ => _);

            return comm;
        }

        private IEnumerable<PlayerCommissionSettingViewModel> MapCommProductSetting(IEnumerable<PlayerCommissionSettingViewModel> commValues, IDictionary<string, bool> permissions)
        {
            return commValues.Where(_ => permissions.TryGetValue(CommissionGroupHelper.GetProductCode(_.Sporttype, _.Bettype), out bool productAllow) && productAllow);
        }

        public async Task<IResultInfo> MultipleMoveGroup(MultipleMoveGroupQuery query)
        {
            var (comm, commValues) = await _repository.GetCommissionGroupById(new GetCommissionGroupByIdCriteria(query));
            var groupInfo = MergeCommSetting(comm, commValues, query.Permissions);

            if (groupInfo?.CommValues == null)
            {
                return new ResultInfo { Code = 1, Message = ViewResources.GroupNotExists };
            }

            var criteria = new MultipleMoveGroupCriteria(query);
            var result = await _repository.MultipleMoveGroup(criteria);
            return result;
        }

        public async Task<IResultInfo> MultipleRemoveFromGroup(MultipleRemoveFromGroupQuery query)
        {
            var criteria = new MultipleRemoveFromGroupCriteria(query);
            var result = await _repository.MultipleRemoveFromGroup(criteria);
            return result;
        }

        public async Task<IResultInfo> MultipleUpdateGroup(MultipleUpdateGroupQuery query)
        {
            var criteria = new MultipleUpdateGroupCriteria(query);
            var result = await _repository.MultipleUpdateGroup(criteria);
            return result;
        }

        public async Task<PlayerCommissionViewModel> GetPlayerCommissionGroup(SingleMoveGroupQuery query)
        {
            var criteria = new GetPlayerCommissionGroupCriteria(query);

            var (comms, commValues) = await _repository.GetPlayerCommissionGroup(criteria).ConfigureAwait(false);

            return MergeCommSetting(comms, commValues, criteria.Permissions);
        }
    }
}