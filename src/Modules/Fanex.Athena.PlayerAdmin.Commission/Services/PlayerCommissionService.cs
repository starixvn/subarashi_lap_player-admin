using Fanex.Athena.PlayerAdmin.Commission.Criteria;
using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Athena.PlayerAdmin.Commission.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.Commission.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using Fanex.Athena.PlayerAdmin.Infrastructure.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using Fanex.Athena.PlayerAdmin.Infrastructure.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Commission.Services
{
    public class PlayerCommissionService : IPlayerCommissionService
    {
        private readonly IPlayerCommissionRepository _repository;
        private readonly ILgApiClient _lgApiClient;

        public PlayerCommissionService(IPlayerCommissionRepository repository, ILgApiClient lgApiClient)
        {
            _repository = repository;
            _lgApiClient = lgApiClient;
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

            if (!IsUpdateRNGCasino(result, query.Permissions))
            {
                return result;
            }

            var callApiSuccess = await UpdateCommToAPI(new UpdateCommToAPICriteria
            {
                CustomerIdList = result.ExtendedData,
                Comm = groupInfo.CommValues[ProductCode.CS],
                IP = query.Ip,
                UserId = query.UserId,
                Username = query.Username
            });

            return callApiSuccess ? result : new ResultInfo { Code = 1, Message = ViewResources.CasinoAPI_UpdateCommError };
        }

        public async Task<IResultInfo> MultipleRemoveFromGroup(MultipleRemoveFromGroupQuery query)
        {
            var criteria = new MultipleRemoveFromGroupCriteria(query);
            var result = await _repository.MultipleRemoveFromGroup(criteria);

            if (!IsUpdateRNGCasino(result, query.Permissions) || !query.IsRemovePlayerComm)
            {
                return result;
            }

            var callApiSuccess = await UpdateCommToAPI(new UpdateCommToAPICriteria
            {
                CustomerIdList = result.ExtendedData,
                Comm = 0,
                IP = query.Ip,
                UserId = query.UserId,
                Username = query.Username
            });

            return callApiSuccess ? result : new ResultInfo { Code = 1, Message = ViewResources.CasinoAPI_UpdateCommError };
        }

        public async Task<IResultInfo> MultipleUpdateGroup(MultipleUpdateGroupQuery query)
        {
            var criteria = new MultipleUpdateGroupCriteria(query);
            var result = await _repository.MultipleUpdateGroup(criteria);

            if (!IsUpdateRNGCasino(result, query.Permissions))
            {
                return result;
            }

            decimal rngComm = 0;
            for (var i = 0; i < query.Products.Length; i++)
            {
                if (string.Equals(query.Products[i], ProductCode.CS))
                {
                    rngComm = query.Comms[i];
                }
            }

            var callApiSuccess = await UpdateCommToAPI(new UpdateCommToAPICriteria
            {
                CustomerIdList = result.ExtendedData,
                Comm = rngComm,
                IP = query.Ip,
                UserId = query.UserId,
                Username = query.Username
            });

            return callApiSuccess ? result : new ResultInfo { Code = 1, Message = ViewResources.CasinoAPI_UpdateCommError };
        }

        public Task<PlayerCommissionViewModel> GetPlayerCommissionGroup(SingleMoveGroupQuery query)
        {
            var criteria = new GetPlayerCommissionGroupCriteria(query);

            return _repository.GetPlayerCommissionGroup(criteria);
        }

        private async Task<bool> UpdateCommToAPI(UpdateCommToAPICriteria criteria)
        {
            var customerIds = criteria.CustomerIdList.Split(',');
            if (customerIds.Length == 0)
            {
                return true;
            }

            var metalModel = new RNGCasinoUpdateMemberComCriteria
            {
                Comm = criteria.Comm,
                CustomerId = customerIds,
                FromIp = criteria.IP,
                ModifyBy = criteria.UserId,
                ModifyUserName = criteria.Username
            };

            const string apiPath = "/RNGCasinoCustomer/UpdateCommission";
            var result = await _lgApiClient.PostAsync<ApiResult<bool>>(apiPath, metalModel, "en-US");

            return result != null && result.Data;
        }

        private static bool IsUpdateRNGCasino(IResultInfo result, IDictionary<string, bool> permissions)
            => result.IsSuccessful
            && permissions.ContainsKey(ProductCode.CS)
            && permissions[ProductCode.CS]
            && !string.IsNullOrWhiteSpace(result.ExtendedData);
    }
}