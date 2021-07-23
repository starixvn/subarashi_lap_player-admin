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
        [RequestValidation(
            validateCustId: true,
            validateProductId: -1,
            limitedProductIds: new[] { ProductType.VirtualSports, ProductType.VirtualSports2, ProductType.Lottery, ProductType.TG, ProductType.I4D })]
        public async Task<JsonResult> GetSetting([FromBody] PlayerBetLimitSelectMetaModel metaModel)
        {
            var isEditable = HasEditPermission();
            var data = _service.GetSettingAsync(metaModel.CustId, metaModel.ProductId);
            var status = _service.GetStatusAsync(metaModel.CustId, metaModel.ProductId);

            await Task.WhenAll(data, status);

            var viewModel = new PlayerBetLimitViewModel
            {
                Data = AddSettingTypeName(await data),
                Editable = isEditable && await status
            };

            return Json(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit_Write)]
        [RequestValidation(
            validateCustId: true,
            validateProductId: -1,
            limitedProductIds: new[] { ProductType.VirtualSports, ProductType.VirtualSports2, ProductType.Lottery, ProductType.TG, ProductType.I4D })]
        [AutoBindingMetaModel(bindClientIP: true, bindLanguage: true, bindSiteName: true, bindUserId: true)]
        public async Task<JsonResult> UpdateSetting([FromBody] PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            var result = await _service.UpdateSettingAsync(metaModel);

            result.Message = result.Code == 0 ? ViewResources.UpdateSuccess : result.Message;

            return Json(result);
        }
    }
}