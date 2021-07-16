using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.DataDomains;
using MediatR;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Mediator
{
    public class PlayerUpdatedSpecialLevelEvent : PlayerUpdatedEvent, INotification
    {
        public string Status { get; set; }
        public string NewValue { get; set; }
        public string OldValue { get; set; }

        public override int ActionID => LogActionId.UpdateSpecialLevel;

        public override string Doing => $"LAP - {SiteName} - {LicenseeCustId} - {LogActionDetailId.UpdatingSpecialLevel} - {Status} - {NewValue} - {OldValue}";
    }
}