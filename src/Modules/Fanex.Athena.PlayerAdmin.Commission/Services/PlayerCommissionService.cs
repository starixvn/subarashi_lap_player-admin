using Fanex.Athena.PlayerAdmin.Commission.Criteria;
using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Athena.PlayerAdmin.Commission.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.Commission.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using System.Collections.Generic;
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

        public Task<IEnumerable<PlayerCommissionViewModel>> GetAllCommGroups(BasePlayerCommissionUpdateQuery query)
        {
            var criteria = new GetAllCommGroupsCriteria(query);
            return _repository.GetAllCommGroups(criteria);
        }

        public async Task<IResultInfo> MultipleMoveGroup(MultipleMoveGroupQuery query)
        {
            var groupInfo = await _repository.GetCommissionGroupById(new GetCommissionGroupByIdCriteria(query));
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

        public Task<PlayerCommissionViewModel> GetPlayerCommissionGroup(SingleMoveGroupQuery query)
        {
            var criteria = new GetPlayerCommissionGroupCriteria(query);

            return _repository.GetPlayerCommissionGroup(criteria);
        }
    }
}