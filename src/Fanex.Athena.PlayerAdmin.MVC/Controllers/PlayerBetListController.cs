using Fanex.Athena.PlayerAdmin.Infrastructure.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Attributes;
using Fanex.Athena.PlayerAdmin.Infrastructure.Services;
using Fanex.Athena.PlayerAdmin.Infrastructure.Settings;
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
    [CookieAuthorize(Roles.SuperAdmin, Roles.BetList)]
    public class PlayerBetListController : Controller
    {
        private readonly IContextService _contextService;
        private readonly AppSetting _appSetting;

        public PlayerBetListController(IContextService contextService, IOptions<AppSetting> appSetting)
        {
            _appSetting = appSetting.Value;
            _contextService = contextService;
        }

        [HttpGet]
        [ReferrerValidation]
        public ViewResult Running(int custId, int currencyId, string lapToken)
        {
            var currencyList = _contextService.CurrencyList;
            var viewModel = new PlayerRunningBetListViewModel
            {
                CustId = custId,
                Currency = currencyList.FirstOrDefault(x => x.Key == currencyId).Value,
                LapToken = lapToken
            };

            ViewBag.IsAllowTxIDColumn = _appSetting.LicenseeOfferTxIDColumn.Contains(_contextService.SiteName, StringComparer.InvariantCultureIgnoreCase);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestValidation(validateCustId: true)]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<IActionResult> GetRunningBetList([FromServices] IBetListApiClient betListApiClient, [FromBody] PlayerRunningBetListMetaModel metaModel)
        {
            metaModel.IsShowIp = _contextService.IsShowIP;
            metaModel.IsShowIpVN = _contextService.IsShowIpVN;

            IActionResult result = metaModel.IsExport
                ? await ExportRunningBetListDataAsync(betListApiClient, metaModel)
                : await GetRunningBetListDataAsync(betListApiClient, metaModel);
            return result;
        }

        private async Task<JsonResult> GetRunningBetListDataAsync(IBetListApiClient betListApiClient, PlayerRunningBetListMetaModel metaModel)
        {
            var result = await betListApiClient.PostAsync<BetList>("/PlayerAdminBetList/GetBetList", metaModel, _contextService.Language);
            return Json(result);
        }

        private async Task<FileResult> ExportRunningBetListDataAsync(IBetListApiClient betListApiClient, PlayerRunningBetListMetaModel metaModel)
        {
            metaModel.IsAllowTxIDColumn = _appSetting.LicenseeOfferTxIDColumn.Contains(
                _contextService.SiteName, StringComparer.InvariantCultureIgnoreCase);

            var httpResponse = await betListApiClient.PostAsync("/PlayerAdminBetList/Export", metaModel, _contextService.Language);

            return File(await httpResponse.Content.ReadAsStreamAsync(), "application/octet-stream");
        }
    }
}