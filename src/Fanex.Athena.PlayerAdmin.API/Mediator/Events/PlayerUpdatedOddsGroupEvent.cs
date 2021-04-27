using Fanex.Athena.PlayerAdmin.API.DataDomains;
using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Mediator;
using MediatR;

namespace Fanex.Athena.PlayerAdmin.API.Mediator
{
    public class PlayerUpdatedOddsGroupEvent : PlayerUpdatedEvent, INotification
    {
        public string Status { get; set; }
        public string NewValue { get; set; }
        public string OldValue { get; set; }

        public override int ActionID => LogActionId.UpdateOddsGroup;

        public override string Doing => $"LAP - {SiteName} - {LicenseeCustId} - {LogActionDetailId.UpdatingOddsGroup} - {Status} - {NewValue} - {OldValue}";
    }
}