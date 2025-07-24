using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Mediator.Handlers
{
    public class UpdateMultipleBAToCTSClientHandler : INotificationHandler<PlayerUpdatedMultipleBAToCTSEvent>
    {
        private ICTSApiClient CTSApiClient { get; }
        private CustomerClassApiSetting Setting { get; }
        private const string EnableBA = "1";

        public UpdateMultipleBAToCTSClientHandler(ICTSApiClient cTSApiClient, IOptions<CustomerClassApiSetting> setting)
        {
            CTSApiClient = cTSApiClient;
            Setting = setting.Value;
        }

        public async Task Handle(PlayerUpdatedMultipleBAToCTSEvent notification, CancellationToken cancellationToken)
        {
            if (Setting.Enabled)
            {
                var isBa = notification.NewValue == EnableBA;
                var path = isBa ? Setting.EndPoints.AddLicBA : Setting.EndPoints.RemoveLicBA;
                await CTSApiClient.PostAsync(path, new { CustIDs = notification.CustIDList }, "en-US");
            }
        }
    }
}