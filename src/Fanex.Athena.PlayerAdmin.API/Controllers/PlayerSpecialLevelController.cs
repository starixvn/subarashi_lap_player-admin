using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.API.Utilities;
using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Filters;
using Fanex.Athena.PlayerAdmin.Infrastructure.Helpers;
using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using Fanex.Athena.PlayerAdmin.Infrastructure.Services;
using Fanex.Logging;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors]
    [ApiController]
    public class PlayerSpecialLevelController : ControllerBase
    {
        private readonly IPlayerSpecialLevelService _playerSpecialLevelService;
        private readonly ILicenseeService _licenseeService;
        private readonly ILogger _logger;

        public PlayerSpecialLevelController(
            IPlayerSpecialLevelService playerSpecialLevelService,
            ILicenseeService licenseeService,
            ILogger logger)
        {
            _playerSpecialLevelService = playerSpecialLevelService;
            _licenseeService = licenseeService;
            _logger = logger;
        }

        [HttpPost]
        [ReferrerValidation]
        [BearerAuthorize(Roles.SuperAdmin, Roles.UpdateSpecialLevel)]
        [DataValidation(validateCustId: true)]
        [AutoBindingMetaModel(bindUserId: true, bindClientIP: true, bindLanguage: true, bindSiteName: true)]
        public async Task<IActionResult> Update(SpecialLevelUpdatingMetaModel metaModel)
        {
            if (_licenseeService.IsFakeLicensee(metaModel.SiteName))
            {
                return Ok(new ResultViewModel
                {
                    Success = false,
                    ErrorMessage = ResourceKeyConstants.FunctionDenied
                });
            }

            try
            {
                return Ok(await _playerSpecialLevelService.Update(metaModel));
            }
            catch (Exception exception)
            {
                await _logger.ErrorAsync(ErrorMessageHelper.GenerateMessage(exception, HttpContext));
            }

            return Ok(new ResultViewModel
            {
                Success = false,
                ErrorMessage = ResourceKeyConstants.FunctionDenied
            });
        }
    }
}