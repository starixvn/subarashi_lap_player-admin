namespace Fanex.Athena.PlayerAdmin.Core.Mediator
{
    using Constants;
    using MediatR;

    public abstract class PlayerUpdatedEvent : INotification
    {
        public string SiteName { get; set; }

        public int LicenseeCustId { get; set; }

        public int UserId { get; set; }

        public int CustID { get; set; }

        public byte TypeId => UserControlLogType.Function;

        public abstract int ActionID { get; }

        public abstract string Doing { get; }
    }
}