namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Constants;
    using Core.Models;
    using Core.Resources;
    using Core.Services;
    using MetaModels;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    public partial class PlayerBatchUpdateController
    {
        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.UpdateOddsGroup)]
        [DecompressModelStateAndMetaModel("metaModel", "CustIds", "CustNames")]
        public ViewResult UpdateOddsGroup(PlayerBatchUpdateMetaModel metaModel,
            [FromServices] IContextService contextService)
        {
            var viewModel = new BasePlayerBatchUpdateViewModel(metaModel);
            ViewBag.OddsGroups = contextService.OfferedOddsGroupList;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.UpdateOddsGroup)]
        [RequestValidation]
        [AutoBindingMetaModel(bindLicenseeId: true, bindSiteName: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public async Task<JsonResult> UpdateOddsGroup(UpdateOddsGroupApiMetaModel metaModel, [FromServices] IContextService contextService)
        {
            var isValid = contextService.OfferedOddsGroupList
                .Any(_ => string.Equals(_, metaModel.OddsGroupId, StringComparison.InvariantCultureIgnoreCase));

            if (!isValid)
            {
                return Json(new ResultInfo
                {
                    Code = 1,
                    Message = ViewResources.FunctionDeniedMsg
                });
            }

            var result = await _service.UpdateOddsGroup(metaModel);
            return Json(result);
        }
    }
}