using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Filters;
using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.MVC.Controllers
{
    [DisableCors]
    public class PlayerStatementController : Controller
    {
        private readonly IPlayerStatementService _playerStatementService;
        private const int MaximumYear = 2;

        public PlayerStatementController(IPlayerStatementService playerStatementService)
        {
            _playerStatementService = playerStatementService;
        }

        [HttpGet]
        [CookieAuthorize(Roles.SuperAdmin, Roles.Statement)]
        public ViewResult Index(int custId, int currencyId)
        {
            var today = DateTime.Today;
            var viewModel = new PlayerStatementViewModel
            {
                CustId = custId,
                CurrencyId = currencyId,
                FromDate = new DateTime(today.Year, today.Month, 1),
                ToDate = today,
                MinDate = new DateTime(today.Year - MaximumYear, 1, 1)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.Statement)]
        [DataValidation(validateCustId: true)]
        [AutoBindingMetaModel(bindSiteName: true, bindLanguage: true)]
        public async Task<ActionResult> GetStatementUrl([FromBody]PlayerStatementMetaModel metaModel)
        {
            return Content(await _playerStatementService.GetUrl(metaModel).ConfigureAwait(false));
        }
    }
}