using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient.TestableApiClients
{
    public class BetListApiClientTestable : BetListApiClient
    {
        public BetListApiClientTestable(HttpClient client, IContextService contextService)
            : base(client, contextService)
        { }

        internal string Public_ApiName => ApiName;

        internal Task Public_PrepareHeaders(HttpRequestHeaders headers)
            => PrepareHeaders(headers);
    }
}
