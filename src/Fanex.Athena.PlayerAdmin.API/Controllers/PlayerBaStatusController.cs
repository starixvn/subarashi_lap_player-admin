using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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
        [RequestValidation(validateCustId: true, validateProductId: ProductType.BA)]
        [AutoBindingMetaModel(bindUserId: true, bindSiteName: true, bindLanguage: true, bindClientIP: true)]
        public async Task<JsonResult> Update(BaStatusUpdatingMetaModel metaModel)
        {
            return new JsonResult(await _playerBaStatusService.Update(metaModel));
        }
    }
}