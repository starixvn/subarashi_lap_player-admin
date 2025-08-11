namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Constants;
    using DTO;
    using MetaModel;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

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
        [RequestValidation(validateCustId: true)]
        [AutoBindingMetaModel(bindSiteName: true)]
        public async Task<ActionResult<PlayerActivityRecord>> Index(PlayerActivityRecordMetaModel metaModel)
        {
            if (metaModel.CustId <= 0)
            {
                return null;
            }

            return await _playerActivityRecordService.GetActivityRecord(metaModel);
        }
    }
}