namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Constants;
    using Core.Resources;
    using Core.Services;
    using MetaModel;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using ViewModels;

    [RequestValidation(validateCustId: true, validateClientIP: false, validateProductId: -1)]
    public class DcsBetLimitController : PlayerBetLimitBaseControler
    {
        private readonly IDcsBetLimitService _service;

        public DcsBetLimitController(
            IContextService contextService,
            IResourceService resourceService,
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
            var data = await _service.GetSettingAsync(metaModel.CustId, metaModel.ProductId);
            var status = await _service.GetStatusAsync(metaModel.CustId, metaModel.ProductId);

            var viewModel = new PlayerBetLimitViewModel
            {
                Data = AddSettingTypeName(data),
                Editable = isEditable && (!status.IsDisabled.HasValue || status.IsDisabled.Value),
                IsStatusEnable = status.IsDisabled.HasValue && isEditable,
                StatusEnable = status.IsDisabled.GetValueOrDefault(),
                StatusBetType = status.BetType,
                StatusSportType = status.SportType
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