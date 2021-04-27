using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Filters;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors]
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
        [DataValidation(validateCustId: true)]
        [AutoBindingMetaModel(bindBalanceUp: true)]
        public async Task<IActionResult> GetData(PlayerBettingStatisticsMetaModel metaModel)
        {
            var now = DateTime.Now;
            metaModel.FromDate = new DateTime(now.Year, now.Month, 1);
            metaModel.ToDate = now;

            return Ok(await _playerBettingStatisticsService.GetPlayerBettingStatistics(metaModel));
        }
    }
}