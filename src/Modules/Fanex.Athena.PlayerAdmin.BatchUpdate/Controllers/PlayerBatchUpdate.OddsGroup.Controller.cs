using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Athena.PlayerAdmin.BatchUpdate.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.Services;
using System.Linq;
using System;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Controllers
{
    public partial class PlayerBatchUpdateController
    {
        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.UpdateOddsGroup)]
        public ViewResult UpdateOddsGroup(PlayerBatchUpdateMetaModel metaModel, 
            [FromServices] IContextService contextService,
            [FromServices] ILicenseeService licenseeService)
        {
            var viewModel = new BasePlayerBatchUpdateViewModel(metaModel);
            ViewBag.OddsGroups = licenseeService.GetOddsGroups(contextService.SiteName);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.UpdateOddsGroup)]
        [RequestValidation]
        [AutoBindingMetaModel(bindLicenseeId: true, bindSiteName: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public async Task<JsonResult> UpdateOddsGroup(UpdateOddsGroupApiMetaModel metaModel, [FromServices] ILicenseeService licenseeService)
        {
            var oddsGroups = licenseeService.GetOddsGroups(metaModel.SiteName);
            var isValid = oddsGroups.Any(_ => string.Equals(_, metaModel.OddsGroupId, StringComparison.InvariantCultureIgnoreCase));
            if(!isValid)
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