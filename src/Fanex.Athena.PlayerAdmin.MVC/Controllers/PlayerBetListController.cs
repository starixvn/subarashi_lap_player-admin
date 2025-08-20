namespace Fanex.Athena.PlayerAdmin.MVC.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.ConsoleClient;
    using Core.Constants;
    using Core.Extensions;
    using Core.Services;
    using MetaModel;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

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
            metaModel.SpecialPlatform = _contextService.SpecialPlatform;

            var result = metaModel.IsExport
                ? await ExportRunningBetListDataAsync(betListApiClient, metaModel)
                : await GetRunningBetListDataAsync(betListApiClient, metaModel);
            return result;
        }

        private async Task<IActionResult> GetRunningBetListDataAsync(IBetListApiClient betListApiClient, PlayerRunningBetListMetaModel metaModel)
        {
            var json = await betListApiClient.PostStringAsync("/api/PlayerAdminBetList/GetBetList", metaModel, _contextService.Language);
            return this.ToJsonResult(json);
        }

        private async Task<FileResult> ExportRunningBetListDataAsync(IBetListApiClient betListApiClient, PlayerRunningBetListMetaModel metaModel)
        {
            metaModel.IsAllowTxIDColumn = _contextService.IsEnableSingleWallet;

            var httpResponse = await betListApiClient.PostAsync("/api/PlayerAdminBetList/Export", metaModel, _contextService.Language);

            return File(await httpResponse.Content.ReadAsStreamAsync(), "application/octet-stream");
        }
    }
}