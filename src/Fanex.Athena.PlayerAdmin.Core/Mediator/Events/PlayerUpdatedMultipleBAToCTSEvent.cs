namespace Fanex.Athena.PlayerAdmin.Core.Mediator
{
    using MediatR;

    public class PlayerUpdatedMultipleBAToCTSEvent : INotification
    {
        public string NewValue { get; set; }

        public string CustIDList { get; set; }
    }
}