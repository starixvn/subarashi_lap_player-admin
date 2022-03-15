using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.DataDomains;
using MediatR;

namespace Fanex.Athena.PlayerAdmin.Core.Mediator
{
    public class PlayerUpdatedOddsGroupEvent : PlayerUpdatedEvent, INotification
    {
        public string Status { get; set; }
        public string NewValue { get; set; }
        public string OldValue { get; set; }

        public override int ActionID => LogActionId.UpdateOddsGroup;

        public override string Doing => $"LAP - {SiteName} - {LicenseeCustId} - {LogActionDetailId.UpdatingOddsGroup} -" +
            $" {Status} - {NewValue.ToUpperInvariant()} - {OldValue.ToUpperInvariant()}";
    }
}