using Fanex.Athena.PlayerAdmin.Infrastructure.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Filters;
using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using Fanex.Athena.PlayerAdmin.Infrastructure.Services;
using Fanex.Athena.PlayerAdmin.MVC.DTO;
using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.MVC.Controllers
{
    [DisableCors]
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
        [CookieAuthorize(Roles.SuperAdmin, Roles.AccountInfo)]
        public async Task<ViewResult> Index(string userName, string currencyName)
        {
            var productAccess = await GetProductAccess();

            var viewModel = new PlayerFastCrossLinkViewModel
            {
                UserName = userName?.Trim(),
                CurrencyName = currencyName?.Trim(),
                ProductCodeListString = string.Join(",", productAccess.ListProductCode),
                IsAllowedCustomerCommission = _contextService.IsAllowedCustomerComm,
                IsAllowedBA = productAccess.IsAllowBA
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.AccountInfo)]
        [DataValidation]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<IActionResult> Index([FromServices] IReportApiClient reportAPIClient, [Bind("Username")][FromBody] PlayerWinLossMetaModel metaModel)
        {
            await SetDefaultProductOptionValue(metaModel);
            var result = await reportAPIClient.PostAsync<PlayerWinLoss>("api/CustomerWinLoss/Index", metaModel, _contextService.Language);
            return Json(result, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        private Task<ProductAccess> GetProductAccess()
        {
            return _licenseeService.GetSingleWalletProductAccess(
                _contextService.LicenseeId,
                _contextService.Language,
                FunctionName);
        }

        private async Task SetDefaultProductOptionValue(PlayerWinLossMetaModel metaModel)
        {
            var now = DateTime.Now;
            var productAccess = await GetProductAccess();

            metaModel.ProductOptions = productAccess.ProductList.Select(p => new ProductOption { Code = p.ProductCode, Name = p.ProductName, Status = true });
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;
        }
    }
}