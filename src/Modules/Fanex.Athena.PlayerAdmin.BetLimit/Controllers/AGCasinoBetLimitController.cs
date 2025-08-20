namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Constants;
    using Core.Models;
    using Core.Resources;
    using Core.Services;
    using MetaModel;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

    [RequestValidation(validateProductId: ProductTypes.AgCasino, validateCustId: true)]
    public class AGCasinoBetLimitController : PlayerBetLimitBaseControler
    {
        private readonly IAGCasinoBetLimitService _service;

        public AGCasinoBetLimitController(IContextService contextService, IResourceService resourceService,
            IAGCasinoBetLimitService agCasinoBetLimitService) : base(contextService, resourceService)
        {
            _service = agCasinoBetLimitService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit)]
        public async Task<JsonResult> GetSetting([FromBody] PlayerBetLimitSelectMetaModel metaModel)
        {
            var isEditable = HasEditPermission();
            var getGroupBettingsTask = _service.GetGroupBettingsAsync(_contextService.SiteName, metaModel.CurrencyId);
            var getSettingTask = _service.GetBetSettingAsync(metaModel.CustId);

            await Task.WhenAll(getGroupBettingsTask, getSettingTask);

            var setting = await getSettingTask;
            if (!setting.CheckedGroups.Contains(setting.AgGroup))
            {
                setting.CheckedGroups = setting.CheckedGroups.Concat(new[] { setting.AgGroup }).OrderBy(x => x).ToArray();
            }

            return Json(new
            {
                Data = await getGroupBettingsTask,
                BetSetting = setting,
                Editable = isEditable && setting.IsEnabled && setting.IsLineEnabled,
                StatusEnable = setting.IsEnabled,
                IsStatusEnable = isEditable,
                setting.IsLineEnabled
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit_Write)]
        [AutoBindingMetaModel(bindClientIP: true, bindLanguage: true, bindSiteName: true, bindUserId: true)]
        public async Task<JsonResult> UpdateSetting([FromBody] PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel> metaModel)
        {
            var setting = await _service.GetBetSettingAsync(metaModel.CustId);

            if (!setting.IsEnabled)
            {
                return Json(new ResultInfo { Code = 1, Message = ViewResources.FunctionDeniedMsg });
            }
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