using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.MVC.DTO;
using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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

        private readonly AppSetting _appSettings;

        public PlayerBtcSingleCustomerController(
            ILicenseeService licenseeService,
            IContextService contextService,
            IOptions<AppSetting> appSettings)
        {
            _licenseeService = licenseeService;
            _contextService = contextService;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        [ReferrerValidation]
        public async Task<ViewResult> Index([FromQuery]string userName, [FromQuery]string currencyName)
        {
            var viewModel = new PlayerBtcSingleCustomerViewModel
            {
                UserName = userName?.Trim(),
                CurrencyName = currencyName?.Trim(),
                IsAllowedCustomerComm = _contextService.IsAllowedCustomerComm,
                ProductCodeListString = await GetProductCode()
            };

            ViewBag.IsAllowTxIDColumn = _appSettings.LicenseeOfferTxIDColumn.Contains(
               _contextService.SiteName, StringComparer.InvariantCultureIgnoreCase);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestValidation]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<IActionResult> Index([FromServices] IBetListApiClient betListApiClient, [Bind("Username", "PageSize", "PageIndex", "IsExport", "SortBy", "SortOrder")]
        [FromBody] PlayerBtcSingleCustomerMetaModel metaModel)
        {
            metaModel.HasPlayerCom = _contextService.IsAllowedCustomerComm;
            metaModel.IsAllowTxIDColumn = _appSettings.LicenseeOfferTxIDColumn.Contains(
               _contextService.SiteName, StringComparer.InvariantCultureIgnoreCase);
            metaModel.IsShowIp = _contextService.IsShowIP;
            metaModel.IsShowIpVN = _contextService.IsShowIpVN;

            await AssignMetaModelValue(metaModel);

            if (metaModel.IsExport)
            {
                var httpResponse = await betListApiClient.PostAsync("/BettingTicketCenter/GetBySingleCustomer",
                metaModel,
                _contextService.Language);

                if (httpResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    return new StatusCodeResult((int)HttpStatusCode.NoContent);
                }
                return File(await httpResponse.Content.ReadAsStreamAsync(), "application/octet-stream");
            }

            var result = await betListApiClient.PostAsync<BetList>("/BettingTicketCenter/GetBySingleCustomer", metaModel, _contextService.Language);

            return Json(result);
        }

        private async Task AssignMetaModelValue(PlayerBtcSingleCustomerMetaModel metaModel)
        {
            var now = DateTime.Now;

            metaModel.Status = GetStatus();
            metaModel.Platform = AllPlatform;
            metaModel.ProductCodeList = await GetProductCode();
            metaModel.LiveStatus = GetLiveStatus();
            metaModel.BaseCurrency = null;
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;
        }

        private static string GetStatus()
        {
            return string.Join(",", _statusList);
        }

        private static string GetLiveStatus()
        {
            return string.Join(",", _liveStatusList);
        }

        private async Task<string> GetProductCode()
        {
            var productAccess = await _licenseeService.GetSingleWalletProductAccess(
                _contextService.LicenseeId,
                _contextService.Language,
                FunctionKey);

            return string.Join(",", productAccess.ProductList.Select(i => i.ProductCode));
        }
    }
}