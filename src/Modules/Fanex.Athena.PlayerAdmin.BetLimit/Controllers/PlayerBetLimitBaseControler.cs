using System.Collections.Generic;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Helpers;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Mvc;

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
                item.BetSettingType = item.BetType == BetType.Parlay ? GetParlaySettingTypeName(item.SportType) : GetNormalSettingTypeName(item);
            }
            return data;
        }

        private string GetNormalSettingTypeName(PlayerBetLimit item)
        {
            var isHideSaba = SabaHelper.IsHideSaba(_contextService.SpecialPlatform);
            if (item.SportType == I4DId)
            {
                return GetBetTypeName(item.BetType);
            }
            return GetSportName(item.SportType, isHideSaba);
        }

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

        protected string GetSportName(string sportId, bool isHideSaba)
        {
            var prefixText = isHideSaba ? ViewResources.Virtual : ViewResources.Saba;

            if (sportId == SportType.SabaSoccer.ToString())
            {
                var sportName = _resourceService.GetSportName(SportType.Soccer);
                return $"{prefixText} {sportName}";
            }
            if (sportId == SportType.SabaBasketBall.ToString())
            {
                var sportName = _resourceService.GetSportName(SportType.BasketBall);
                return $"{prefixText}{sportName}";
            }
            if (sportId == SportType.SabaCricket.ToString())
            {
                var sportName = _resourceService.GetSportName(SportType.Cricket);
                return $"{prefixText}{sportName}";
            }
            if (sportId == SportType.VirtualSports.ToString())
            {
                return _resourceService.GettResourceData("DummySport", sportId);
            }

            return _resourceService.GetSportName(sportId);
        }

        protected string GetBetTypeName(int bettypeId) => _resourceService.GetBettype(bettypeId);
    }
}