namespace Fanex.Athena.PlayerAdmin.Core.Mediator
{
    using Constants;
    using DataDomains;
    using MediatR;

    public class PlayerUpdatedSpecialLevelEvent : PlayerUpdatedEvent, INotification
    {
        public string Status { get; set; }
        public string NewValue { get; set; }
        public string OldValue { get; set; }

        public override int ActionID => LogActionId.UpdateSpecialLevel;

        public override string Doing => $"LAP - {SiteName} - {LicenseeCustId} - {LogActionDetailId.UpdatingSpecialLevel} - {Status} - {NewValue} - {OldValue}";
    }
}