using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    [RequestValidation(
    validateCustId: true,
    validateProductId: -1,
    limitedProductIds: new[] { ProductTypes.VirtualSports, ProductTypes.VirtualSports2, ProductTypes.Lottery, ProductTypes.TG, ProductTypes.I4D, ProductTypes.SLT, ProductTypes.I4D, ProductTypes.SLT,
            ProductTypes.AllBet, ProductTypes.Voidbridge, ProductTypes.CashOut, ProductTypes.PP, ProductTypes.SXG, ProductTypes.HB, ProductTypes.JK,
            ProductTypes.SAG, ProductTypes.PGS, ProductTypes.SBC, ProductTypes.WM, ProductTypes.SG, ProductTypes.VRG, ProductTypes.CC, ProductTypes.CG, ProductTypes.VGA, ProductTypes.AP,
            ProductTypes.APM, ProductTypes.SabaCasino, ProductTypes.BTC, ProductTypes.FGGL, ProductTypes.MXG, ProductTypes.Keno, ProductTypes.NumberGame, ProductTypes.JIG,
            ProductTypes.YB, ProductTypes.MGRNG, ProductTypes.LG, ProductTypes.SEA, ProductTypes.WE })]
    public class DcsBetLimitController : PlayerBetLimitBaseControler
    {
        private readonly IDcsBetLimitService _service;

        public DcsBetLimitController(IContextService contextService, IResourceService resourceService,
            IDcsBetLimitService dcsBetLimitService) : base(contextService, resourceService)
        {
            _service = dcsBetLimitService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit)]
        public async Task<JsonResult> GetSetting([FromBody] PlayerBetLimitSelectMetaModel metaModel)
        {
            var isEditable = HasEditPermission();
            var dataTask = _service.GetSettingAsync(metaModel.CustId, metaModel.ProductId);
            var statusTask = _service.GetStatusAsync(metaModel.CustId, metaModel.ProductId);

            var data = await dataTask;
            var status = await statusTask;

            var viewModel = new PlayerBetLimitViewModel
            {
                Data = AddSettingTypeName(data),
                Editable = isEditable && (!status.HasValue || status.Value),
                IsStatusEnable = status.HasValue && isEditable,
                StatusEnable = status.GetValueOrDefault()
            };

            return Json(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit_Write)]
        [AutoBindingMetaModel(bindClientIP: true, bindLanguage: true, bindSiteName: true, bindUserId: true)]
        public async Task<JsonResult> UpdateSetting([FromBody] PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            var result = await _service.UpdateSettingAsync(metaModel);
            result.Message = result.Code == 0 ? ViewResources.UpdateSuccess : result.Message;
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit_Write)]
        [AutoBindingMetaModel(bindClientIP: true, bindLanguage: true, bindSiteName: true, bindUserId: true)]
        public async Task<JsonResult> UpdateStatus([FromBody] PlayerBetLimitUpdateStatusMetaModel metaModel)
        {
            var result = await _service.UpdateStatusAsync(metaModel);
            result.Message = result.Code == 0 ? ViewResources.UpdateSuccess : result.Message;
            return Json(result);
        }
    }
}