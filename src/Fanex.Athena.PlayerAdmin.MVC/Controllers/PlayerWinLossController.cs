using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.Helpers;
using Fanex.Athena.PlayerAdmin.Core.Extensions;

namespace Fanex.Athena.PlayerAdmin.MVC.Controllers
{
    [CookieAuthorize(Roles.SuperAdmin, Roles.AccountInfo)]
    public class PlayerWinLossController : Controller
    {
        private const string FunctionName = "CustomerWinLoss";

        private readonly ILicenseeService _licenseeService;
        private readonly IContextService _contextService;

        public PlayerWinLossController(
            ILicenseeService licenseeService,
            IContextService contextService
            )
        {
            _licenseeService = licenseeService;
            _contextService = contextService;
        }

        [HttpGet]
        [ReferrerValidation]
        public async Task<ViewResult> Index(string userName, string currencyName)
        {
            var productAccess = await _licenseeService.GetSingleWalletProductAccess(_contextService.LicenseeId, FunctionName);
            var viewModel = new PlayerFastCrossLinkViewModel
            {
                UserName = userName?.Trim(),
                CurrencyName = currencyName?.Trim(),
                ProductCodeListString = productAccess.ProductCodeListString,
                IsAllowedCustomerCommission = _contextService.IsAllowedCustomerComm,
                IsAllowedBA = productAccess.IsAllowBA
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestValidation]
        [AutoBindingMetaModel(bindAgent: true, bindProductList: true)]
        public async Task<IActionResult> Index([FromServices] IReportApiClient reportAPIClient, [Bind("Username")][FromBody] PlayerWinLossMetaModel metaModel)
        {
            await AssignMetaModelValue(metaModel);
            var json = await reportAPIClient.PostStringAsync("api/CustomerWinLoss/Index", metaModel, _contextService.Language);
            return this.ToJsonResult(json);
        }

        private async Task AssignMetaModelValue(PlayerWinLossMetaModel metaModel)
        {
            const string AllOption = "All";
            var now = DateTimeHelper.DatabaseServerNow;
            var productAccess = await _licenseeService.GetSingleWalletProductAccess(_contextService.LicenseeId, FunctionName);
            metaModel.HasComm = _contextService.IsAllowedCustomerComm;
            metaModel.IsAllowedBA = productAccess.IsAllowBA;
            metaModel.SystemIdVisible = !_licenseeService.IsFakeLicensee(_contextService.SiteName);
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;
            metaModel.OddsGroup = AllOption;
            metaModel.CustomerClass = AllOption;
        }
    }
}