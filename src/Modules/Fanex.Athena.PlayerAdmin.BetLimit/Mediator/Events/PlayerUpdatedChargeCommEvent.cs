namespace Fanex.Athena.PlayerAdmin.BetLimit.Mediator.Events
{
    using DataDomains;
    using MediatR;

    public class PlayerUpdatedChargeCommEvent : BasePlayerUpdatedBetSettingEvent<PlayerUpdatedProductDetailedLog>, INotification
    {
    }
}
