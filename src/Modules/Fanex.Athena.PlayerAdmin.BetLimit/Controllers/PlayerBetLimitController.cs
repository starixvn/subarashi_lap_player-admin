using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Helpers;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Microsoft.Extensions.Options;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    public class PlayerBetLimitController : Controller
    {
        private readonly IPlayerBetLimitRepository _playerBetLimitRepository;

        public PlayerBetLimitController(IPlayerBetLimitRepository playerBetLimitRepository)
        {
            _playerBetLimitRepository = playerBetLimitRepository;
        }

        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit)]
        public async Task<ViewResult> Index([FromServices] IContextService contextService, [FromServices] IOptions<AppSetting> appSetting, int custId, int currencyId, int licenseeCustId)
        {
            var productList = await GetProducts(contextService.SiteName, appSetting.Value.Env);
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

        private async Task<IDictionary<int, string>> GetProducts(string siteName, string environment)
        {
            var products = await _playerBetLimitRepository.GetEnableBetLimitProducts(siteName, environment);
            return products.ToDictionary(x => x.ProductId, x => ProductHelper.GetProductName(x.ProductCode));
        }
    }
}