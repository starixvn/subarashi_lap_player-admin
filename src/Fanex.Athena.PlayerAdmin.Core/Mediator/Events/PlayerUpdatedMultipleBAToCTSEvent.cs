using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.DataDomains;
using MediatR;

namespace Fanex.Athena.PlayerAdmin.Core.Mediator
{
    public class PlayerUpdatedMultipleBAToCTSEvent : INotification
    {
        public string NewValue { get; set; }

        public string CustIDList { get; set; }
    }
}