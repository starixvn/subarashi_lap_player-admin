using System;
using System.Net;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Helpers;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.PlayerAdmin.MVC.Controllers
{
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
                var httpResponse = await betListApiClient.PostAsync("/BettingTicketCenter/GetBySingleCustomer", metaModel, _contextService.Language);

                if (httpResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    return new StatusCodeResult((int)HttpStatusCode.NoContent);
                }
                return File(await httpResponse.Content.ReadAsStreamAsync(), "application/octet-stream");
            }

            var json = await betListApiClient.PostStringAsync("/BettingTicketCenter/GetBySingleCustomer", metaModel, _contextService.Language);
            return this.ToJsonResult(json);
        }

        private void AssignMetaModelValue(PlayerBtcSingleCustomerMetaModel metaModel)
        {
            var now = DateTimeHelper.DatabaseServerNow;

            metaModel.HasPlayerCom = _contextService.IsAllowedCustomerComm;
            metaModel.IsAllowTxIDColumn = _contextService.IsEnableSingleWallet;
            metaModel.IsShowIp = _contextService.IsShowIP;
            metaModel.IsShowIpVN = _contextService.IsShowIpVN;
            metaModel.Status = GetStatus();
            metaModel.Platform = AllPlatform;
            metaModel.LiveStatus = GetLiveStatus();
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;
            metaModel.SpecialPlatform = _contextService.SpecialPlatform;
        }

        private static string GetStatus()
            => string.Join(",", _statusList);

        private static string GetLiveStatus()
            => string.Join(",", _liveStatusList);
    }
}