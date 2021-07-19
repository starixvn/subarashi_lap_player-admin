using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.ViewModels;
using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Attributes;
using Fanex.Athena.PlayerAdmin.Infrastructure.Helpers;
using Fanex.Athena.PlayerAdmin.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ViewResult> Index([FromServices] IContextService contextService, int custId, int currencyId, int licenseeCustId)
        {
            var productList = await GetProducts(contextService.SiteName);
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

        private async Task<IDictionary<int, string>> GetProducts(string siteName)
        {
            var products = await _playerBetLimitRepository.GetEnableBetLimitProducts(siteName);
            return products.ToDictionary(x => x.ProductId, x => ProductHelper.GetProductName(x.ProductCode));
        }
    }
}