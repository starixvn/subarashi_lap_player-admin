using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ReferrerValidation]
    [EnableCors(Policy.FromPortalSite)]
    [ApiController]
    public class PlayerBaStatusController : ControllerBase
    {
        private readonly IPlayerBaStatusService _playerBaStatusService;

        public PlayerBaStatusController(IPlayerBaStatusService playerBaStatusService)
        {
            _playerBaStatusService = playerBaStatusService;
        }

        [HttpPost]
        [BearerAuthorize(Roles.SuperAdmin, Roles.UpdateBaStatus)]
        [RequestValidation(validateCustId: true, validateProductId: ProductTypes.BA)]
        [AutoBindingMetaModel(bindUserId: true, bindSiteName: true, bindLanguage: true, bindClientIP: true)]
        public async Task<ActionResult<ResultViewModel>> Update(
            [FromServices] IContextService contextService, [FromServices] ILicenseeService licenseeService,
            [FromServices] IOptions<AppSetting> appSetting,
            BaStatusUpdatingMetaModel metaModel)
        {
            if (await licenseeService.IsAllowUpdateBA(contextService.LicenseeId, contextService.Username, appSetting.Value.SpecialBAAdmins))
            {
                var result = await _playerBaStatusService.Update(metaModel);
                return result;
            }

            return new ResultViewModel
            {
                Success = false,
                Message = ViewResources.MessageLimitTimeUpdateBA,
                ErrorCode = 1
            };
        }
    }
}