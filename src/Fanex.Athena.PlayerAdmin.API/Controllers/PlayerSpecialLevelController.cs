namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Constants;
    using Core.Helpers;
    using Core.Models;
    using Core.Services;
    using MetaModel;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.Interfaces;
    using Utilities;

    [Route("api/[controller]/[action]")]
    [ReferrerValidation]
    [EnableCors(Policy.FromPortalSite)]
    [ApiController]
    public class PlayerSpecialLevelController : ControllerBase
    {
        private readonly IPlayerSpecialLevelService _playerSpecialLevelService;
        private readonly IContextService _contextService;
        private readonly ILogger<PlayerSpecialLevelController> _logger;

        public PlayerSpecialLevelController(
            IPlayerSpecialLevelService playerSpecialLevelService,
            IContextService contextService,
            ILogger<PlayerSpecialLevelController> logger)
        {
            _playerSpecialLevelService = playerSpecialLevelService;
            _contextService = contextService;
            _logger = logger;
        }

        [HttpPost]
        [BearerAuthorize(Roles.SuperAdmin, Roles.UpdateSpecialLevel)]
        [RequestValidation(validateCustId: true, validateClientIP: false)]
        [AutoBindingMetaModel(bindUserId: true, bindClientIP: true, bindLanguage: true, bindSiteName: true)]
        public async Task<IActionResult> Update(SpecialLevelUpdatingMetaModel metaModel)
        {
            if (_contextService.IsFakeLicensee)
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
                _logger.LogCritical(exception, ErrorMessageHelper.GenerateMessage(exception, HttpContext));
            }

            return Ok(new ResultViewModel
            {
                Success = false,
                ErrorMessage = ResourceKeyConstants.FunctionDenied
            });
        }
    }
}