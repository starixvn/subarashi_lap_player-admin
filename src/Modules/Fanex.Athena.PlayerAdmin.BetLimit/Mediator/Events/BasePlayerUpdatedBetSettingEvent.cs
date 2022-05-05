using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Mediator;
using MediatR;
using Newtonsoft.Json;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Mediator
{
    public class BasePlayerUpdatedBetSettingEvent<T> : PlayerUpdatedEvent, INotification
    {
        protected BasePlayerUpdatedBetSettingEvent() { }

        public T Detail { get; set; }

        public override int ActionID => LogActionId.UpdateBetLimit;

        public override string Doing => "LAP - " + SiteName + " - " + LicenseeCustId + " - " + JsonConvert.SerializeObject(Detail);
    }
}
