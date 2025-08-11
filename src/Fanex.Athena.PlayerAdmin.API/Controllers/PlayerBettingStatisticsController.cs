namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Constants;
    using Core.Helpers;
    using MetaModel;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

    [Route("api/[controller]/[action]")]
    [EnableCors(Policy.FromPortalSite)]
    [ApiController]
    public class PlayerBettingStatisticsController : ControllerBase
    {
        private readonly IPlayerBettingStatisticsService _playerBettingStatisticsService;

        public PlayerBettingStatisticsController(IPlayerBettingStatisticsService playerBettingStatisticsService)
        {
            _playerBettingStatisticsService = playerBettingStatisticsService;
        }

        [HttpPost]
        [BearerAuthorize(Roles.SuperAdmin, Roles.AccountInfo)]
        [RequestValidation(validateCustId: true)]
        [AutoBindingMetaModel(bindBalanceUp: true, bindProductList: true)]
        public async Task<IActionResult> GetData(PlayerBettingStatisticsMetaModel metaModel)
        {
            var now = DateTimeHelper.DatabaseServerNow;
            metaModel.FromDate = DateTimeHelper.CreateDateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;

            return Ok(await _playerBettingStatisticsService.GetPlayerBettingStatistics(metaModel));
        }
    }
}