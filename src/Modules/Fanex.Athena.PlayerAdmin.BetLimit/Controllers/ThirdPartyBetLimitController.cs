namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Constants;
    using Core.Resources;
    using Core.Services;
    using MetaModel;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using ViewModels;

    [RequestValidation(
        validateCustId: true,
        validateProductId: -1,
        limitedProductIds: [ProductTypes.CashOut])]
    public class ThirdPartyBetLimitController : PlayerBetLimitBaseControler
    {
        private readonly IThirdPartyBetLimitService _service;

        public ThirdPartyBetLimitController(
            IContextService contextService,
            IResourceService resourceService,
            IThirdPartyBetLimitService service) : base(contextService, resourceService)
        {
            _service = service;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit)]
        public async Task<ActionResult<PlayerBetLimitViewModel>> GetSetting([FromBody] PlayerBetLimitSelectMetaModel metaModel)
        {
            var isEditable = HasEditPermission();
            var setting = await _service.GetSettingAsync(metaModel.CustId, metaModel.ProductId);

            var viewModel = new PlayerBetLimitViewModel
            {
                IsStatusEnable = isEditable,
                StatusEnable = setting.IsEnabled
            };

            return viewModel;
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