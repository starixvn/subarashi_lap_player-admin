using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Athena.PlayerAdmin.BatchUpdate.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Controllers
{
    public partial class PlayerBatchUpdateController
    {
        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.UpdateBaStatus)]
        public ViewResult UpdateBAStatus(PlayerBatchUpdateMetaModel metaModel)
        {
            var viewModel = new BasePlayerBatchUpdateViewModel(metaModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.UpdateBaStatus)]
        [RequestValidation(validateProductId: ProductType.BA)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindSiteName: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public async Task<JsonResult> UpdateBAStatus([FromServices] IContextService contextService, [FromServices]ILicenseeService licenseeService,
            IOptions<AppSetting> appSetting,
            UpdateBAStatusApiMetaModel metaModel)
        {
            if (await IsAllowUpdateBA(licenseeService, contextService.LicenseeId, contextService.Username, appSetting.Value.SpecialBAAdmins))
            {
                var result = await _service.UpdateBAStatus(metaModel);
                return Json(result);
            }

            return Json(new
            {
                IsSuccessful = true,
                Code = 2,
                Message = ViewResources.MessageLimitTimeUpdateBA
            });
        }

        private static async Task<bool> IsAllowUpdateBA(ILicenseeService licenseeService, int licenseeId, string userName, IEnumerable<string> specicalBAAdmins)
        {
            var specialBaPermission = specicalBAAdmins.IsPresent() &&
                specicalBAAdmins.Any(s => string.Equals(s, userName, StringComparison.OrdinalIgnoreCase));
            if (specialBaPermission)
            {
                return true;
            }

            var isAllow = await licenseeService.IsAllowUpdateMemberBAStatus(licenseeId);
            return isAllow;
        }
    }
}