using Fanex.Athena.PlayerAdmin.Core.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    public class ReportApiClient : BaseApiClient, IReportApiClient
    {
        private readonly IContextService _contextService;

        public ReportApiClient(HttpClient client, IContextService contextService) : base(client)
        {
            _contextService = contextService;
        }

        protected override async Task PrepareHeaders(HttpRequestHeaders headers)
        {
            headers.Authorization = new AuthenticationHeaderValue("Bearer", await _contextService.AccessToken);
            headers.Add("X-IgnoreSharding", bool.TrueString);
        }

        protected override string ApiName => "Report";
    }
}