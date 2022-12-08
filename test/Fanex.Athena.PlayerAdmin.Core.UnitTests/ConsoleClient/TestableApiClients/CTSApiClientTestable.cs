using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient.TestableApiClients
{
    public class CTSApiClientTestable : CTSApiClient
    {
        public CTSApiClientTestable(HttpClient client) : base(client)
        { }

        public string Public_ApiName => ApiName;

        public Task Public_PrepareHeaders(HttpRequestHeaders headers)
            => PrepareHeaders(headers);

        public Task Public_HandleUnSuccessRequestAsync(HttpResponseMessage response)
            => HandleUnSuccessRequestAsync(response);
    }
}
