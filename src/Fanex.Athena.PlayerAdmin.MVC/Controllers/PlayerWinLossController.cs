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
using Fanex.Athena.PlayerAdmin.Core.DataDomains;

namespace Fanex.Athena.PlayerAdmin.MVC.Controllers
{
    [CookieAuthorize(Roles.SuperAdmin, Roles.AccountInfo)]
    public class PlayerWinLossController : Controller
    {
        private const string FunctionName = "WinLossAnalysis";

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
                IsAllowedBA = _contextService.IsEnableBA && productAccess.IsAllowBA
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromServices] IReportApiClient reportAPIClient, [Bind("Username", "BaseCurrency")][FromBody] PlayerWinLossMetaModel metaModel)
        {
            await AssignMetaModelValue(metaModel);
            var json = await reportAPIClient.PostStringAsync("api/WinlossAnalysis/GetData", metaModel, _contextService.Language);
            return this.ToJsonResult(json);
        }

        private async Task AssignMetaModelValue(PlayerWinLossMetaModel metaModel)
        {
            const string AllOption = "All";
            var now = DateTimeHelper.DatabaseServerNow;
            var productAccess = await _licenseeService.GetSingleWalletProductAccess(_contextService.LicenseeId, FunctionName);
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now.Date;
            metaModel.CurrencyIdList = string.Join(",", _contextService.CurrencyList.Keys);
            metaModel.ProductCodes = productAccess.ProductCodeListString;
            metaModel.SportIds = AllOption;
            metaModel.BetTypeIds = AllOption;
            metaModel.StatusIds = AllOption;
            metaModel.PlatformIds = AllOption;
            metaModel.LiveStatusIds = AllOption;
            metaModel.IsViewCashOutOnly = false;
            metaModel.GroupMode = GroupByMode.Player;
            metaModel.BreakdownMode = BreakdownMode.Product;
            metaModel.IsShowInOrginalCurrency = true;
            metaModel.IsExcludedTestData = false;
            metaModel.IsVipOnly = false;
            metaModel.OddsGroup = AllOption;
            metaModel.CustomerClass = AllOption;
            metaModel.SearchMode = SearchMode.MatchPrecise;
            metaModel.SearchUserName = metaModel.Username;
            metaModel.SubsidiarySearch = false;
            metaModel.SortBy = 0;
            metaModel.SortOrder = 0;
        }
    }
}