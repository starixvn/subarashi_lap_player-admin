using Fanex.Athena.PlayerAdmin.BetLimit.DataDomains;
using MediatR;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Mediator.Events
{
    public class PlayerUpdatedChargeCommEvent : BasePlayerUpdatedBetSettingEvent<PlayerUpdatedProductDetailedLog>, INotification
    {
    }
}
