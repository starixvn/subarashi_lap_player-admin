using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Mediator
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