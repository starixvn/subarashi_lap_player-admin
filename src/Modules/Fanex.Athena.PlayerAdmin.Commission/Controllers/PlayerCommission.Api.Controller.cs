namespace Fanex.Athena.PlayerAdmin.Commission.Controllers
{
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Constants;
    using Core.Settings;
    using MetaModels;
    using Microsoft.AspNetCore.Mvc;

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
            var productAccess = await _licenseeService.GetProductAccess(_contextService.LicenseeId);
            var result = await _playerCommService.MultipleMoveGroup(query, productAccess);
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
            var result = await _playerCommService.MultipleRemoveFromGroup(query);

            return Json(result);
        }
    }
}