using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.Settings;

namespace Fanex.Athena.PlayerAdmin.Commission.Controllers
{
    public partial class PlayerCommissionController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [FunctionUM(FunctionUMType.Commission)]
        [RequestValidation]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public async Task<JsonResult> PostMultipleMoveGroup([FromForm] MultipleMoveGroupQuery query)
        {
            await AssignSessionToQuery(query);
            var result = await _playerCommService.MultipleMoveGroup(query);
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [FunctionUM(FunctionUMType.Commission)]
        [RequestValidation]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public async Task<JsonResult> PostMultipleUpdateGroup([FromForm] MultipleUpdateGroupQuery query)
        {
            await AssignSessionToQuery(query);
            var result = await _playerCommService.MultipleUpdateGroup(query);

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [FunctionUM(FunctionUMType.Commission)]
        [RequestValidation]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public async Task<JsonResult> PostMultipleRemoveFromGroup([FromForm] MultipleRemoveFromGroupQuery query)
        {
            await AssignSessionToQuery(query);
            var result = await _playerCommService.MultipleRemoveFromGroup(query);

            return Json(result);
        }
    }
}