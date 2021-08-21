using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    public class SportsBookBetLimitController : PlayerBetLimitBaseControler
    {
        private readonly ISportsBookBetLimitService _service;
        private readonly AppSetting _appSetting;

        public SportsBookBetLimitController(IContextService contextService, IResourceService resourceService,
            ISportsBookBetLimitService sportBookBetLimitService,
            IOptions<AppSetting> appSetting) : base(contextService, resourceService)
        {
            _service = sportBookBetLimitService;
            _appSetting = appSetting.Value;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit)]
        [RequestValidation(validateCustId: true)]
        public async Task<JsonResult> GetSetting([FromBody] PlayerBetLimitSelectMetaModel metaModel)
        {
            var isEditable = HasEditPermission();
            var data = await _service.GetSettingAsync(metaModel.CustId, _appSetting.IsShowMixedSport);
            var copySportId = SportType.Soccer.ToString();

            var viewModel = new PlayerBetLimitViewModel
            {
                Data = AddSettingTypeName(data),
                Editable = isEditable,
                CopyId = copySportId,
                EnableCopy = data.Any(x => x.Id == copySportId)
            };

            return Json(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CookieAuthorize(Roles.SuperAdmin, Roles.BetLimit_Write)]
        [RequestValidation(validateCustId: true)]
        [AutoBindingMetaModel(bindClientIP: true, bindLanguage: true, bindSiteName: true, bindUserId: true)]
        public async Task<JsonResult> UpdateSetting([FromBody] PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            var result = await _service.UpdateSettingAsync(metaModel, _appSetting.IsShowMixedSport);

            result.Message = result.Code == 0 ? ViewResources.UpdateSuccess : result.Message;

            return Json(result);
        }
    }
}