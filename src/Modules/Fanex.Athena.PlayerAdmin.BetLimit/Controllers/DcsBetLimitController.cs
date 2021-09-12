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
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    [RequestValidation(
    validateCustId: true,
    validateProductId: -1,
    limitedProductIds: new[] { ProductTypes.VirtualSports, ProductTypes.VirtualSports2, ProductTypes.Lottery, ProductTypes.TG, ProductTypes.I4D, ProductTypes.SLT, ProductTypes.I4D, ProductTypes.SLT,
            ProductTypes.AllBet, ProductTypes.Voidbridge, ProductTypes.CashOut, ProductTypes.Exchange, ProductTypes.PP, ProductTypes.SXG, ProductTypes.GDG, ProductTypes.HB, ProductTypes.JK,
            ProductTypes.SAG, ProductTypes.PGS, ProductTypes.SBC, ProductTypes.WM, ProductTypes.SG, ProductTypes.VRG, ProductTypes.CC, ProductTypes.CG, ProductTypes.VGA })]
    public class DcsBetLimitController : PlayerBetLimitBaseControler
    {
        private static readonly IEnumerable<int> ProductsHasStatus = new []
        {
            ProductTypes.I4D, ProductTypes.SLT, ProductTypes.AllBet, ProductTypes.Voidbridge, ProductTypes.CashOut, ProductTypes.Exchange, ProductTypes.PP, ProductTypes.SXG, ProductTypes.GDG,
            ProductTypes.HB, ProductTypes.JK, ProductTypes.SAG, ProductTypes.PGS, ProductTypes.SBC, ProductTypes.WM, ProductTypes.SG, ProductTypes.VRG, ProductTypes.CC, ProductTypes.CG,
            ProductTypes.VGA, ProductTypes.Lottery, ProductTypes.TG
        };

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
            var data = _service.GetSettingAsync(metaModel.CustId, metaModel.ProductId);
            var status = _service.GetStatusAsync(metaModel.CustId, metaModel.ProductId);

            await Task.WhenAll(data, status);

            var viewModel = new PlayerBetLimitViewModel
            {
                Data = AddSettingTypeName(await data),
                Editable = isEditable && await status,
                IsStatusEnable = isEditable && ProductsHasStatus.Contains(metaModel.ProductId),
                StatusEnable = await status
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