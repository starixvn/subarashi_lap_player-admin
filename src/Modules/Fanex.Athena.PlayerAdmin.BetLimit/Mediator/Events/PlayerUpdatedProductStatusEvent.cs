namespace Fanex.Athena.PlayerAdmin.BetLimit.Mediator
{
    using DataDomains;
    using MediatR;

    public class PlayerUpdatedProductStatusEvent : BasePlayerUpdatedBetSettingEvent<PlayerUpdatedProductDetailedLog>, INotification
    {
    }
}