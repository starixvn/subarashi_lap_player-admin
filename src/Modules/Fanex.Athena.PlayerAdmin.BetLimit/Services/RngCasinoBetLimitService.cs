using Fanex.Athena.PlayerAdmin.BetLimit.Helpers;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories;
using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using MediatR;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Services
{
    public class RngCasinoBetLimitService : IRngCasinoBetLimitService
    {
        private readonly IRngCasinoBetLimitRepository _repository;
        private readonly IPublisher _publisher;
        private readonly ILgApiClient _lgApiClient;
        private readonly ICustomerService _customerService;
        private const int RngCasinoSportId = 251;

        public RngCasinoBetLimitService(
            IRngCasinoBetLimitRepository repository,
            IPublisher publisher,
            ILgApiClient lgApiClient,
            ICustomerService customerService)
        {
            _repository = repository;
            _publisher = publisher;
            _lgApiClient = lgApiClient;
            _customerService = customerService;
        }

        public async Task<(bool isSync, bool? isEnabled)> GetSettingAsync(int custId)
        {
            var customerSetting = await _customerService.GetCustomerSetting(custId);
            var isSync = customerSetting?.SyncCasino ?? false;
            bool? isEnabled = isSync ? !customerSetting.DisableCasino : null;
            return (isSync, isEnabled);
        }

        public async Task<IResultInfo> UpdateStatusAsync(PlayerBetLimitUpdateStatusMetaModel metaModel)
        {
            const string apiPath = "/RngCasino/Status";
            var param = new
            {
                CustId = metaModel.CustId,
                Status = metaModel.Status ? 1 : 0,
                Ip = metaModel.Ip,
                UserId = metaModel.UserId,
                UserName = metaModel.Username
            };

            var updatingCasino = await _lgApiClient.PostAsync<ApiResult<bool>>(apiPath, param, "en-US");
            if (updatingCasino.Data)
            {
                var result = await _repository.UpdateStatusAsync(metaModel, RngCasinoSportId);
                var @event = ProductStatusLogHelper.CreateLogEvent(metaModel, result.Code == 0);
                await _publisher.Publish(@event);
                return result;
            }

            return new ResultInfo
            {
                Code = 1,
                Message = ViewResources.UpdateUnSuccess
            };
        }
    }
}