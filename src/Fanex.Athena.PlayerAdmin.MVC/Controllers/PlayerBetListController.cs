using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.MVC.Controllers
{
    [CookieAuthorize(Roles.SuperAdmin, Roles.BetList)]
    public class PlayerBetListController : Controller
    {
        private readonly IContextService _contextService;

        public PlayerBetListController(IContextService contextService)
        {
            _contextService = contextService;
        }

        [HttpGet]
        [ReferrerValidation]
        public ViewResult Running(int custId, int currencyId)
        {
            var currencyList = _contextService.CurrencyList;
            var viewModel = new PlayerRunningBetListViewModel
            {
                CustId = custId,
                Currency = currencyList.FirstOrDefault(x => x.Key == currencyId).Value
            };

            ViewBag.IsAllowTxIDColumn = _contextService.IsEnableSingleWallet;

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

            var result = metaModel.IsExport
                ? await ExportRunningBetListDataAsync(betListApiClient, metaModel)
                : await GetRunningBetListDataAsync(betListApiClient, metaModel);
            return result;
        }

        private async Task<IActionResult> GetRunningBetListDataAsync(IBetListApiClient betListApiClient, PlayerRunningBetListMetaModel metaModel)
        {
            var json = await betListApiClient.PostStringAsync("/PlayerAdminBetList/GetBetList", metaModel, _contextService.Language);
            return this.ToJsonResult(json);
        }

        private async Task<FileResult> ExportRunningBetListDataAsync(IBetListApiClient betListApiClient, PlayerRunningBetListMetaModel metaModel)
        {
            metaModel.IsAllowTxIDColumn = _contextService.IsEnableSingleWallet;

            var httpResponse = await betListApiClient.PostAsync("/PlayerAdminBetList/Export", metaModel, _contextService.Language);

            return File(await httpResponse.Content.ReadAsStreamAsync(), "application/octet-stream");
        }
    }
}