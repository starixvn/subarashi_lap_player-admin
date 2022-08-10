using Fanex.Athena.PlayerAdmin.Core.ConsoleClient.Abstractions;
using Fanex.Athena.PlayerAdmin.Core.DataDomains;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Mediator.Handlers
{
    public class UpdateBAToCTSClientHandler : INotificationHandler<PlayerUpdatedBAEvent>
    {
        private ICTSApiClient CTSApiClient { get; }
        private CustomerClassApiSetting Setting { get; }

        public UpdateBAToCTSClientHandler(ICTSApiClient cTSApiClient, IOptions<CustomerClassApiSetting> setting)
        {
            CTSApiClient = cTSApiClient;
            Setting = setting.Value;
        }
        

        public async Task Handle(PlayerUpdatedBAEvent notification, CancellationToken cancellationToken)
        {
            if (Setting.Enabled && notification.Status == LogText.Success)
            {
                var isBa = notification.NewValue == LogText.On;
                var path = isBa ? Setting.EndPoints.AddLicBA : Setting.EndPoints.RemoveLicBA;
                var custIds = notification.CustID.ToString();
                await CTSApiClient.PostAsync(path, new { CustIDs = custIds }, "en-US");
            }
        }
    }
}
