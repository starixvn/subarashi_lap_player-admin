namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Constants;
    using Core.Services;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using ViewModels;

    [Route("api/[controller]/[action]")]
    [EnableCors(Policy.FromPortalSite)]
    [BearerAuthorize(Roles.SuperAdmin, Roles.PlayerAdmin)]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly IContextService _contextService;

        public SettingController(IContextService contextService)
        {
            _contextService = contextService;
        }

        [HttpPost]
        public JsonResult GetSiteSetting()
        {
            return new JsonResult(new SiteSettingViewModel
            {
                IsAllowedCustomerComm = _contextService.IsAllowedCustomerComm,
                UserLevel = _contextService.UserLevel,
                IsEnableSingleWallet = _contextService.IsEnableSingleWallet,
                IsShowSabaIntelligence = _contextService.IsShowSabaIntelligence,
                IsSearchWithMatchPreciseOnly = _contextService.IsSearchWithMatchPreciseOnly,
                IsShowMobile = _contextService.IsInRoles(Roles.SuperAdmin, Roles.PlayerProfile)
            }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        [HttpPost]
        public JsonResult GetPlayerInfoSetting()
        {
            return new JsonResult(new PlayerInfoSettingViewModel
            {
                IsShowPlayerProfile = _contextService.IsInRoles(Roles.SuperAdmin, Roles.PlayerProfile),
                IsShowBankInformation = _contextService.IsInRoles(Roles.SuperAdmin, Roles.BankInfo),
                IsShowActivityRecord = _contextService.IsInRoles(Roles.SuperAdmin, Roles.ActivityRecord),
            }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        [HttpPost]
        public async Task<IActionResult> GetDepositSetting([FromServices] IDepositSettingService depositSettingService)
        {
            var commGroups = depositSettingService.GetCommGroupList(_contextService.LicenseeId);
            var riskLevels = depositSettingService.GetRiskLevelList(_contextService.Language);
            var oddsGroups = _contextService.OfferedOddsGroupList;

            return Ok(new DepositSettingViewModel
            {
                RiskLevels = await riskLevels,
                CommGroups = await commGroups,
                OddsGroups = oddsGroups
            });
        }

        [HttpPost]
        public async Task<IActionResult> GetSubsidiarySetting([FromServices] ISubsidiaryService subsidiaryService)
        {
            var subsidiaryViewType = _contextService.SubsidiaryViewType;
            var subsidiaryList = await subsidiaryService.GetSubsidiaryList(_contextService.SiteName, _contextService.AgentList,
                _contextService.WhiteLabelID, subsidiaryViewType);

            return Ok(new
            {
                SubsidiaryViewType = !subsidiaryList.Any() ? SubsidiaryViewType.NotManagement : subsidiaryViewType,
                SubsidiaryList = subsidiaryList
            });
        }
    }
}