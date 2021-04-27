using System.Net.Http;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.ConsoleClient
{
    public class ReportApiClient : BaseApiClient, IReportApiClient
    {
        public ReportApiClient(HttpClient client) : base(client)
        {
        }

        protected override string ApiName => "Report";
    }
}