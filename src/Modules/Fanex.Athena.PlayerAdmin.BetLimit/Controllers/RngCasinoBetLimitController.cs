using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    [RequestValidation(validateCustId: true, validateProductId: ProductTypes.RngCasino)]
    public class RngCasinoBetLimitController : PlayerBetLimitBaseControler
    {
        private readonly IRngCasinoBetLimitService _service;

        public RngCasinoBetLimitController(
            IContextService contextService,
            IResourceService resourceService,
            IRngCasinoBetLimitService service) : base(contextService, resourceService)
        {
            _service = service;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit)]
        public async Task<JsonResult> GetSetting([FromBody] PlayerBetLimitSelectMetaModel metaModel)
        {
            var isEditable = HasEditPermission();
            var (_, isEnabled) = await _service.GetSettingAsync(metaModel.CustId);

            var data = new
            {
                StatusEnable = isEnabled,
                IsStatusEnable = isEditable
            };

            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit_Write)]
        [FunctionUM(FunctionUMType.BetSetting)]
        [AutoBindingMetaModel(bindClientIP: true, bindLanguage: true, bindSiteName: true, bindUserId: true)]
        public async Task<JsonResult> UpdateStatus([FromBody] PlayerBetLimitUpdateStatusMetaModel metaModel)
        {
            var (isSync, _) = await _service.GetSettingAsync(metaModel.CustId);
            if (!isSync)
            {
                return Json(new ResultInfo { Code = -1, Message = ViewResources.AccessDeniedMsg });
            }

            metaModel.Username = _contextService.Username;
            var result = await _service.UpdateStatusAsync(metaModel);
            result.Message = result.Code == 0 ? ViewResources.UpdateSuccess : result.Message;
            return Json(result);
        }
    }
}