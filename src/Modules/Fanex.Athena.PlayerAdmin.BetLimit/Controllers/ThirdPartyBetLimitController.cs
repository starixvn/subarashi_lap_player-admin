using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services;
using Fanex.Athena.PlayerAdmin.BetLimit.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    [RequestValidation(
        validateCustId: true,
        validateProductId: -1,
        limitedProductIds: new[] { ProductTypes.ColossusBets, ProductTypes.CashOut })]
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