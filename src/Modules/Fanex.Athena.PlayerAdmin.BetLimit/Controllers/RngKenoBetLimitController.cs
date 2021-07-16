using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.ViewModels;
using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Filters;
using Fanex.Athena.PlayerAdmin.Infrastructure.Resources;
using Fanex.Athena.PlayerAdmin.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    public class RngKenoBetLimitController : PlayerBetLimitBaseControler
    {
        private readonly IRngKenoBetLimitService _service;

        public RngKenoBetLimitController(IContextService contextService, IResourceService resourceService,
            IRngKenoBetLimitService rngKenoBetLimitService) : base(contextService, resourceService)
        {
            _service = rngKenoBetLimitService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit)]
        [DataValidation(validateCustId: true, validateProductId: ProductType.Keno)]
        public async Task<ActionResult<PlayerBetLimitViewModel>> GetSetting([FromBody] PlayerBetLimitSelectMetaModel metaModel)
        {
            var isEditable = HasEditPermission();
            var data = await _service.GetSettingAsync(metaModel.CustId);

            var viewModel = new PlayerBetLimitViewModel
            {
                Data = AddProductName(data),
                Editable = isEditable
            };

            return viewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit_Write)]
        [DataValidation(validateCustId: true, validateProductId: ProductType.Keno)]
        [AutoBindingMetaModel(bindClientIP: true, bindLanguage: true, bindSiteName: true, bindUserId: true)]
        public async Task<JsonResult> UpdateSetting([FromBody] PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            var result = await _service.UpdateSettingAsync(metaModel);

            result.Message = result.Code == 0 ? ViewResources.UpdateSuccess : result.Message;

            return Json(result);
        }

        private static IEnumerable<PlayerBetLimit> AddProductName(IEnumerable<PlayerBetLimit> data)
        {
            foreach (var item in data)
            {
                item.BetSettingType = ViewResources.ProductKN;
            }

            return data;
        }
    }
}