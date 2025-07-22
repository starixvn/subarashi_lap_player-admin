using Fanex.Athena.PlayerAdmin.BetLimit.DataDomains;
using MediatR;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Mediator
{
    public class PlayerUpdatedProductStatusEvent : BasePlayerUpdatedBetSettingEvent<PlayerUpdatedProductDetailedLog>, INotification
    {
    }
}