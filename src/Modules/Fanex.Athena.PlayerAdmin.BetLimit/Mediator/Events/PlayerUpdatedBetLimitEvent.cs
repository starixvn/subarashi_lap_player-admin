namespace Fanex.Athena.PlayerAdmin.BetLimit.Mediator
{
    using System.Collections.Generic;
    using DataDomains;
    using MediatR;

    public class PlayerUpdatedBetLimitEvent : BasePlayerUpdatedBetSettingEvent<IEnumerable<PlayerBetLimitDetailedLog>>, INotification
    {
    }
}