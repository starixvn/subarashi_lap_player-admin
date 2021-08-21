using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Mediator;
using MediatR;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Mediator
{
    public class PlayerUpdatedBetLimitEvent : PlayerUpdatedEvent, INotification
    {
        public IEnumerable<PlayerBetLimitDetailedLog> Detail { get; set; }

        public override int ActionID => LogActionId.UpdateBetLimit;

        public override string Doing => "LAP - " + SiteName + " - " + LicenseeCustId + " - " + JsonConvert.SerializeObject(Detail);
    }

    public class PlayerBetLimitDetailedLog
    {
        public int DetailedId { get; set; }

        public string SportType { get; set; }

        public string Status { get; set; }

        public string Old { get; set; }

        public string New { get; set; }
    }
}