namespace Fanex.Athena.PlayerAdmin.Core.Mediator
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Models;
    using Repositories;

    public class PlayerUpdatedHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : PlayerUpdatedEvent, INotification
    {
        private readonly ILogRepository _logRepository;

        public PlayerUpdatedHandler(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            var logData = new LogData
            {
                CustID = notification.CustID,
                UserId = notification.UserId,
                ActionID = notification.ActionID,
                TypeId = notification.TypeId,
                Doing = notification.Doing
            };

            await _logRepository.Write(logData);
        }
    }
}