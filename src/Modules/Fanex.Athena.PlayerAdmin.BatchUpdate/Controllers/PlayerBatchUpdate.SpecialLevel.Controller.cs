using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Athena.PlayerAdmin.BatchUpdate.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Controllers
{
    public partial class PlayerBatchUpdateController
    {
        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.UpdateSpecialLevel)]
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