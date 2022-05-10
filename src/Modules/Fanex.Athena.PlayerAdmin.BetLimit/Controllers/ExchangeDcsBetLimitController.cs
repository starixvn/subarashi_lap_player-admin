using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    [RequestValidation(validateCustId: true, validateProductId: ProductTypes.Exchange)]
    public class ExchangeDcsBetLimitController : PlayerBetLimitBaseControler
    {
        private static readonly double[] ValidComms = new[] { 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7 };

        private readonly IDcsBetLimitService _dcsService;
        private readonly IExchangeChargeCommService _chargeCommService;

        public ExchangeDcsBetLimitController(IContextService contextService, IResourceService resourceService,
            IDcsBetLimitService dcsBetLimitService, IExchangeChargeCommService chargeCommService) : base(contextService, resourceService)
        {
            _dcsService = dcsBetLimitService;
            _chargeCommService = chargeCommService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit)]
        public async Task<JsonResult> GetSetting([FromBody] PlayerBetLimitSelectMetaModel metaModel)
        {
            var commResult = _chargeCommService.GetChargeCommission(metaModel.CustId, _contextService.Language);
            var status = _dcsService.GetStatusAsync(metaModel.CustId, ProductTypes.Exchange);

            var viewModel = new PlayerChargeCommViewModel
            {
                ValidComms = ValidComms,
                Commission = (await commResult).IsSuccessful ? (await commResult).SpecificExtendedData : null,
                StatusEnable = (await status).GetValueOrDefault(),
                Editable = HasEditPermission()
            };

            return Json(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit_Write)]
        [AutoBindingMetaModel(bindClientIP: true, bindLanguage: true, bindSiteName: true, bindUserId: true)]
        public async Task<JsonResult> UpdateStatus([FromBody] PlayerBetLimitUpdateStatusMetaModel metaModel)
        {
            metaModel.ProductId = ProductTypes.Exchange;
            var result = await _dcsService.UpdateStatusAsync(metaModel);
            result.Message = result.IsSuccessful ? ViewResources.UpdateSuccess : result.Message;
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit_Write)]
        [AutoBindingMetaModel(bindClientIP: true, bindLanguage: true, bindSiteName: true, bindUserId: true)]
        public async Task<JsonResult> UpdateChargeComm([FromBody] PlayerUpdateChargeCommMetaModel metaModel)
        {
            if (!ValidComms.Contains(metaModel.Comm))
            {
                const int InvalidChargeCommissionError = 100;
                return Json(new ResultInfo { Code = InvalidChargeCommissionError, Message = ViewResources.InvalidChargeCommission });
            }

            if (!(await _dcsService.GetStatusAsync(metaModel.CustId, ProductTypes.Exchange)).GetValueOrDefault())
            {
                return Json(new ResultInfo { Code = 1, Message = ViewResources.FunctionDeniedMsg });
            }

            var result = await _chargeCommService.UpdateChargeCommission(metaModel);
            return Json(result);
        }
    }
}
