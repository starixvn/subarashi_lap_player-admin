namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Controllers
{
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Constants;
    using MetaModels;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    public partial class PlayerBatchUpdateController
    {
        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.UpdateSpecialLevel)]
        [DecompressModelStateAndMetaModel("metaModel", "CustIds", "CustNames")]
        public ViewResult UpdateSpecialLevel(PlayerBatchUpdateMetaModel metaModel)
        {
            var viewModel = new BasePlayerBatchUpdateViewModel(metaModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.UpdateSpecialLevel)]
        [RequestValidation]
        [AutoBindingMetaModel(bindLicenseeId: true, bindSiteName: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public async Task<JsonResult> UpdateSpecialLevel(UpdateSpecialLevelApiMetaModel metaModel)
        {
            var result = await _service.UpdateSpecialLevel(metaModel);
            return Json(result);
        }
    }
}