namespace Fanex.Athena.PlayerAdmin.MVC.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.ConsoleClient;
    using Core.Constants;
    using Core.Extensions;
    using Core.Helpers;
    using Core.Services;
    using MetaModel;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    [CookieAuthorize(Roles.SuperAdmin, Roles.AccountInfo)]
    public class PlayerBtcSingleCustomerController : Controller
    {
        private const string FunctionKey = "BettingTicketCenter";
        private const string AllPlatform = "All";

        private static readonly int[] _statusList = new[]
        {
            StatusTypes.Won, StatusTypes.Lose, StatusTypes.Draw, StatusTypes.Void,
            StatusTypes.AbnormalBet, StatusTypes.Reject, StatusTypes.Refund,  StatusTypes.DelayedPayout
        };

        private static readonly int[] _liveStatusList = new[] { LiveStatusTypes.NonLive, LiveStatusTypes.Live, LiveStatusTypes.Mix };

        private readonly ILicenseeService _licenseeService;
        private readonly IContextService _contextService;

        public PlayerBtcSingleCustomerController(ILicenseeService licenseeService, IContextService contextService)
        {
            _licenseeService = licenseeService;
            _contextService = contextService;
        }

        [HttpGet]
        [ReferrerValidation]
        public async Task<ViewResult> Index([FromQuery] string userName, [FromQuery] string currencyName)
        {
            var productAccess = await _licenseeService.GetSingleWalletProductAccess(_contextService.LicenseeId, FunctionKey);
            var viewModel = new PlayerBtcSingleCustomerViewModel
            {
                UserName = userName?.Trim(),
                CurrencyName = currencyName?.Trim(),
                IsAllowedCustomerComm = _contextService.IsAllowedCustomerComm,
                ProductCodeListString = productAccess.ProductCodeListString
            };

            ViewBag.IsAllowTxIDColumn = _contextService.IsEnableSingleWallet;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestValidation]
        [AutoBindingMetaModel(bindAgent: true, bindProductList: true)]
        public async Task<IActionResult> Index([FromServices] IBetListApiClient betListApiClient, [Bind("Username", "PageSize", "PageIndex", "IsExport", "SortBy", "SortOrder")]
        [FromBody] PlayerBtcSingleCustomerMetaModel metaModel)
        {
            AssignMetaModelValue(metaModel);

            if (metaModel.IsExport)
            {
                var httpResponse = await betListApiClient.PostAsync("/api/BettingTicketCenter/GetBySingleCustomer", metaModel, _contextService.Language);

                if (httpResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    return new StatusCodeResult((int)HttpStatusCode.NoContent);
                }
                return File(await httpResponse.Content.ReadAsStreamAsync(), "application/octet-stream");
            }

            var json = await betListApiClient.PostStringAsync("/api/BettingTicketCenter/GetBySingleCustomer", metaModel, _contextService.Language);
            return this.ToJsonResult(json);
        }

        private void AssignMetaModelValue(PlayerBtcSingleCustomerMetaModel metaModel)
        {
            var now = DateTimeHelper.DatabaseServerNow;

            metaModel.IsAllowTxIDColumn = _contextService.IsEnableSingleWallet;
            metaModel.Status = GetStatus();
            metaModel.Platform = AllPlatform;
            metaModel.LiveStatus = GetLiveStatus();
            metaModel.FromDate = DateTimeHelper.CreateDateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;
            metaModel.SportIds = "All";
            metaModel.BetTypeIds = "All";
            metaModel.CashOutMode = 0;
        }

        private static string GetStatus()
            => string.Join(",", _statusList);

        private static string GetLiveStatus()
            => string.Join(",", _liveStatusList);
    }
}