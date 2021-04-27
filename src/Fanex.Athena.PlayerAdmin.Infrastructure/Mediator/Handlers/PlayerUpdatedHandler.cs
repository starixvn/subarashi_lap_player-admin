using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using Fanex.Athena.PlayerAdmin.Infrastructure.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Mediator
{
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