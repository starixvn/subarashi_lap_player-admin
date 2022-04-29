using Fanex.Athena.PlayerAdmin.BetLimit.DataDomains;
using MediatR;
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Mediator
{
    public class PlayerUpdatedBetLimitEvent : BasePlayerUpdatedBetSettingEvent<IEnumerable<PlayerBetLimitDetailedLog>>, INotification
    {
    }
}