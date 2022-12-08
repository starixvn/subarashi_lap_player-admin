using Fanex.Athena.PlayerAdmin.BetLimit.Controllers;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.Core.Services;
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.MockClasses
{
    internal class PlayerBetLimitBaseControlerTestable : PlayerBetLimitBaseControler
    {
        public PlayerBetLimitBaseControlerTestable(IContextService contextService, IResourceService resourceService) 
            : base(contextService, resourceService)
        {
        }

        public IEnumerable<PlayerBetLimit> Public_AddSettingTypeName(IEnumerable<PlayerBetLimit> data)
            => AddSettingTypeName(data);

        public string Public_GetParlaySettingTypeName(string id)
            => GetParlaySettingTypeName(id);

        public bool Public_HasEditPermission()
            => HasEditPermission();

        public string Public_GetSportName(string sportId)
            => GetSportName(sportId);

        public string Public_GetBetTypeName(int bettypeId)
            => GetBetTypeName(bettypeId);
    }
}
