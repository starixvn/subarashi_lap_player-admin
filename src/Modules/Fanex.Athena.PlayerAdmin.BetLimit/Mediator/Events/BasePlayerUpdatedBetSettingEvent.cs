namespace Fanex.Athena.PlayerAdmin.BetLimit.Mediator
{
    using Core.Constants;
    using Core.Mediator;
    using MediatR;
    using Newtonsoft.Json;

    public class BasePlayerUpdatedBetSettingEvent<T> : PlayerUpdatedEvent, INotification
    {
        protected BasePlayerUpdatedBetSettingEvent() { }

        public T Detail { get; set; }

        public override int ActionID => LogActionId.UpdateBetLimit;

        public override string Doing => "LAP - " + SiteName + " - " + LicenseeCustId + " - " + JsonConvert.SerializeObject(Detail);
    }
}
