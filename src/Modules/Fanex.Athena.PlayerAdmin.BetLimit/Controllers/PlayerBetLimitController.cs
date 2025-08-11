namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Constants;
    using Core.Services;
    using Core.Settings;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Repositories.Interfaces;
    using ViewModels;

    public class PlayerBetLimitController : Controller
    {
        private readonly IPlayerBetLimitRepository _playerBetLimitRepository;
        private readonly ILicenseeService _licenseeService;

        public PlayerBetLimitController(IPlayerBetLimitRepository playerBetLimitRepository, ILicenseeService licenseeService)
        {
            _playerBetLimitRepository = playerBetLimitRepository;
            _licenseeService = licenseeService;
        }

        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit)]
        public async Task<ViewResult> Index([FromServices] IContextService contextService, [FromServices] IOptions<AppSetting> appSetting, int custId, int currencyId, int licenseeCustId)
        {
            var productList = await GetProducts(contextService.SiteName, contextService.LicenseeId, appSetting.Value.Env);
            var viewModel = new PlayerBetLimitProductViewModel
            {
                CustId = custId,
                ProductList = productList,
                CurrencyId = currencyId,
                LicenseeCustId = licenseeCustId,
                ProductId = productList.FirstOrDefault().Key
            };

            return View(viewModel);
        }

        private async Task<IDictionary<int, string>> GetProducts(string siteName, int licenseeId, string environment)
        {
            var products = await _playerBetLimitRepository.GetEnableBetLimitProducts(siteName, environment);
            var productAccess = await _licenseeService.GetProductAccess(licenseeId);

            return products.ToDictionary(x => x.ProductId, x => productAccess.GetProductName(x.ProductCode));
        }
    }
}