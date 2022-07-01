using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.Helpers;
using Fanex.Athena.PlayerAdmin.Core.Services;

namespace Fanex.Athena.PlayerAdmin.MVC.Controllers
{
    [CookieAuthorize(Roles.SuperAdmin, Roles.Statement)]
    public class PlayerStatementController : Controller
    {
        private readonly IPlayerStatementService _playerStatementService;

        public PlayerStatementController(IPlayerStatementService playerStatementService)
        {
            _playerStatementService = playerStatementService;
        }

        [HttpGet]
        [ReferrerValidation]
        public ViewResult Index(int custId, int currencyId)
        {
            const int MinYearRange = 2;
            const int MaxYearRange = 5;

            var today = DateTimeHelper.DatabaseServerToDay;
            var viewModel = new PlayerStatementViewModel
            {
                CustId = custId,
                CurrencyId = currencyId,
                FromDate = new DateTime(today.Year, today.Month, 1),
                ToDate = today,
                MinDate = new DateTime(today.Year - MinYearRange, 1, 1),
                MaxDate = new DateTime(today.Year + MaxYearRange, 12, 31)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestValidation(validateCustId: true)]
        [AutoBindingMetaModel(bindSiteName: true, bindLanguage: true)]
        public async Task<ActionResult> GetStatementUrl([FromBody] PlayerStatementMetaModel metaModel, [FromServices] IContextService contextService)
        {
            metaModel.IsShowIpVN = contextService.IsShowIpVN;
            return Content(await _playerStatementService.GetUrl(metaModel));
        }
    }
}