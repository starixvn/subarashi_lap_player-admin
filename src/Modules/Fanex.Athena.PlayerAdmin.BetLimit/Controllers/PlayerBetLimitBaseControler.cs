using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Services;
using Fanex.Athena.PlayerAdmin.Infrastructure.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    public class PlayerBetLimitBaseControler : Controller
    {
        private const string MixedSportId = "99MP";
        private const string SoccerParlay = "1MP";
        private const string I4DId = "223";

        protected readonly IContextService _contextService;
        private readonly IResourceService _resourceService;

        public PlayerBetLimitBaseControler(IContextService contextService, IResourceService resourceService)
        {
            _contextService = contextService;
            _resourceService = resourceService;
        }

        protected IEnumerable<PlayerBetLimit> AddSettingTypeName(IEnumerable<PlayerBetLimit> data)
        {
            foreach (var item in data)
            {
                item.BetSettingType = (item.Id == I4DId) ? GetBetTypeName(item.BetType) : GetSportName(item.Id);
            }

            return data;
        }

        protected bool HasEditPermission() => _contextService.IsInRoles(Roles.SuperAdmin, Roles.BetLimit_Write);

        protected string GetSportName(string sportId)
        {
            if (sportId == MixedSportId)
            {
                return ViewResources.MixSportsParlay;
            }
            else if (sportId == SoccerParlay)
            {
                return ViewResources.SoccerParlay;
            }
            else
            {
                return _resourceService.GetSportName(sportId);
            }
        }

        protected string GetBetTypeName(int bettypeId) => _resourceService.GetBettype(bettypeId);
    }
}