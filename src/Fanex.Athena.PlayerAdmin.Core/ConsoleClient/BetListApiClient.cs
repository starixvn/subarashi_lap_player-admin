namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Services;

    public class BetListApiClient : BaseApiClient, IBetListApiClient
    {
        private readonly IContextService _contextService;

        public BetListApiClient(HttpClient client, IContextService contextService) : base(client)
        {
            _contextService = contextService;
        }

        protected override string ApiName => "Bet List";

        protected override async Task PrepareHeaders(HttpRequestHeaders headers)
        {
            headers.Add("X-SiteDBServer", _contextService.SiteDBServer);
            headers.Authorization = new AuthenticationHeaderValue("Bearer", await _contextService.AccessToken);
        }

        public async Task<HttpResponseMessage> PostAsync(string path, object value, string language, int timeOut = 100)
        {
            return await HandleAsync(path, value, language, MediaType.Json,
                 response => Task.FromResult(response)
                , false, timeOut);
        }
    }
}