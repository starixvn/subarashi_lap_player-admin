using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.Helpers;

namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
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
        [AutoBindingMetaModel(bindBalanceUp: true, bindProductCodeList: true)]
        public async Task<IActionResult> GetData(PlayerBettingStatisticsMetaModel metaModel)
        {
            var now = DateTimeHelper.DatabaseServerNow;
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;

            return Ok(await _playerBettingStatisticsService.GetPlayerBettingStatistics(metaModel));
        }
    }
}