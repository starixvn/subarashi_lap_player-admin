using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Mediator;
using MediatR;
using Newtonsoft.Json;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Mediator
{
    public class PlayerUpdatedProductStatusEvent : PlayerUpdatedEvent, INotification
    {
        public PlayerProductStatusDetailedLog Detail { get; set; }

        public override int ActionID => LogActionId.UpdateBetLimit;

        public override string Doing => "LAP - " + SiteName + " - " + LicenseeCustId + " - " + JsonConvert.SerializeObject(Detail);
    }

    public class PlayerProductStatusDetailedLog
    {
        public int DetailedId { get; set; }

        public string ProductId { get; set; }

        public string Status { get; set; }

        public string Old { get; set; }

        public string New { get; set; }
    }
}