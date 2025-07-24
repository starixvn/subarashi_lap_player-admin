using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Helpers;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Services
{
    public class ExchangeChargeCommService : IExchangeChargeCommService
    {
        private const int ApiProcessingErrorCode = 102;

        private const int SuccessStatus = 1;

        private readonly ILgApiClient _lgApiClient;
        private readonly IPublisher _publisher;
        private readonly AppSetting _appSetting;

        public ExchangeChargeCommService(ILgApiClient lgApiClient, IPublisher publisher, IOptions<AppSetting> appSetting)
        {
            _lgApiClient = lgApiClient;
            _publisher = publisher;
            _appSetting = appSetting.Value;
        }

        public async Task<IResultInfo<double?>> GetChargeCommission(int custId, string language, int groupMode)
        {
            const string apiPath = "/Exchange/GetCommission";
            var result = await _lgApiClient.PostAsync<ApiData<ExchangeCommissionData>>(apiPath, new
            {
                custId,
                environment = _appSetting.Env,
                groupMode
            }, language);
            return result.Data == null ? CreateApiErrorResult() : CreateApiResult(result.Data.Status, result.Data.Commission);
        }

        public async Task<IResultInfo> UpdateChargeCommission(PlayerUpdateChargeCommMetaModel metaModel)
        {
            const string apiPath = "/Exchange/UpdateCommission";
            var apiResult = await _lgApiClient.PostAsync<ApiData<ExchangeCommissionData>>(apiPath, new
            {
                custId = metaModel.CustId,
                commission = metaModel.Comm,
                environment = _appSetting.Env,
                groupMode = ExchangeProductHelper.GetGroupMode(metaModel.SiteName),
            }, metaModel.Language);

            var result = apiResult.Data == null ? CreateApiErrorResult() : CreateApiResult(apiResult.Data.Status, apiResult.Data.Commission);
            if (result.IsSuccessful)
            {
                var @event = ChargeCommLogHelper.CreateLogEvent(metaModel, ProductTypes.Exchange);
                await _publisher.Publish(@event);
            }

            return result;
        }

        private static IResultInfo<double?> CreateApiResult(int status, double commission)
            => status == SuccessStatus ? new ResultInfo<double?>
            {
                Code = 0,
                Message = ViewResources.UpdateSuccess,
                SpecificExtendedData = commission
            } : CreateApiErrorResult(status);

        private static IResultInfo<double?> CreateApiErrorResult(int status = ApiProcessingErrorCode)
            => new ResultInfo<double?>
            {
                Code = status == 0 ? ApiProcessingErrorCode : status,
                Message = ViewResources.ErrorProcessingMsg
            };
    }
}
