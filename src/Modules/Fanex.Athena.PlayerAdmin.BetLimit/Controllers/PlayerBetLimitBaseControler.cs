using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Controllers
{
    public class PlayerBetLimitBaseControler : Controller
    {
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
                item.BetSettingType = item.BetType switch
                {
                    BetType.Parlay => GetParlaySettingTypeName(item.SportType),
                    _ => GetNormalSettingTypeName(item)
                }; 
            }
            return data;
        }

        private string GetNormalSettingTypeName(PlayerBetLimit item)
        
            => (item.SportType == I4DId) ? GetBetTypeName(item.BetType) : GetSportName(item.SportType);
        

        protected string GetParlaySettingTypeName(string id)
        {
            const string soccer = "1";
            const string otherSports = "99";
            return id switch
               {
                   soccer => ViewResources.SoccerParlay,
                   otherSports => ViewResources.MixSportsParlay,
                   _ => _resourceService.GetSportName(id)
               };
        }
        

        protected bool HasEditPermission() => _contextService.IsInRoles(Roles.SuperAdmin, Roles.BetLimit_Write);

        protected string GetSportName(string sportId)
        {
            if (sportId == SportType.SabaSoccer.ToString())
            {
                var sportName = _resourceService.GetSportName(SportType.Soccer);
                return $"{ViewResources.Saba} {sportName}";
            }
            if (sportId == SportType.SabaBasketBall.ToString())
            {
                var sportName = _resourceService.GetSportName(SportType.BasketBall);
                return $"{ViewResources.Saba}{sportName}";
            }
            if (sportId == SportType.SabaCricket.ToString())
            {
                var sportName = _resourceService.GetSportName(SportType.Cricket);
                return $"{ViewResources.Saba}{sportName}";
            }
            return _resourceService.GetSportName(sportId);
        }

        protected string GetBetTypeName(int bettypeId) => _resourceService.GetBettype(bettypeId);
    }
}