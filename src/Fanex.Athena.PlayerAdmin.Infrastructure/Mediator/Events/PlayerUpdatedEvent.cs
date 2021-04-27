using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using MediatR;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Mediator
{
    public abstract class PlayerUpdatedEvent : INotification
    {
        public string SiteName { get; set; }

        public int LicenseeCustId { get; set; }

        public int UserId { get; set; }

        public int CustID { get; set; }

        public abstract int ActionID { get; }

        public abstract string Doing { get; }

        public byte TypeId => UserControlLogType.Function;
    }
}