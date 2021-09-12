using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
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
using Fanex.Athena.PlayerAdmin.BetLimit.Services;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    [RequestValidation(validateCustId: true, validateProductId: ProductTypes.Keno)]
    public class RngKenoBetLimitController : PlayerBetLimitBaseControler
    {
        private readonly IRngKenoBetLimitService _service;
        private readonly IThirdPartyBetLimitService _thirdPartyService;

        public RngKenoBetLimitController(IContextService contextService, IResourceService resourceService,
            IRngKenoBetLimitService rngKenoBetLimitService,
            IThirdPartyBetLimitService thirdPartyService) : base(contextService, resourceService)
        {
            _service = rngKenoBetLimitService;
            _thirdPartyService = thirdPartyService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit)]
        public async Task<ActionResult<PlayerBetLimitViewModel>> GetSetting([FromBody] PlayerBetLimitSelectMetaModel metaModel)
        {
            var isEditable = HasEditPermission();
            var data = await _service.GetSettingAsync(metaModel.CustId);

            var viewModel = new PlayerBetLimitViewModel
            {
                Data = AddProductName(data),
                Editable = isEditable,
                IsStatusEnable = false,
                StatusEnable = isEditable && data.Any(_ => _.IsEnable.HasValue && _.IsEnable.Value)
            };

            return viewModel;
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