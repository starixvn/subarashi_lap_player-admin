using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Attributes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ReferrerValidation]
    [EnableCors(Policy.FromPortalSite)]
    [ApiController]
    public class PlayerActivityRecordController : ControllerBase
    {
        private readonly IPlayerActivityRecordService _playerActivityRecordService;

        public PlayerActivityRecordController(IPlayerActivityRecordService playerActivityRecordService)
        {
            _playerActivityRecordService = playerActivityRecordService;
        }

        [HttpPost]
        [BearerAuthorize(Roles.SuperAdmin, Roles.AccountInfo)]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<ActionResult<PlayerActivityRecord>> Index(PlayerActivityRecordMetaModel metaModel)
        {
            if (metaModel.IbcCustId <= 0)
            {
                return null;
            }

            return await _playerActivityRecordService.GetActivityRecord(metaModel);
        }
    }
}