using Fanex.Athena.PlayerAdmin.Core.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    public class BetListApiClient : BaseApiClient, IBetListApiClient
    {
        private readonly IContextService _contextService;

        public BetListApiClient(HttpClient client, IContextService contextService) : base(client)
        {
            _contextService = contextService;
        }

        protected override string ApiName => "Bet List";

        protected override Task PrepareHeaders(HttpRequestHeaders headers)
        {
            headers.Add("X-SiteDBServer", _contextService.SiteDBServer);
            return Task.CompletedTask;
        }

        public async Task<HttpResponseMessage> PostAsync(string path, object value, string language, int timeOut = 100)
        {
            return await HandleAsync(path, value, language, MediaType.Json,
                 (response) => Task.FromResult(response)
                , false, timeOut);
        }
    }
}