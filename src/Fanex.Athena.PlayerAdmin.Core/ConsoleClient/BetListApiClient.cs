using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    public class BetListApiClient : BaseApiClient, IBetListApiClient
    {
        public BetListApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        protected override string ApiName => "Bet List";

        public async Task<HttpResponseMessage> PostAsync(string path, object value, string language, int timeOut = 100)
        {
            return await HandleAsync(path, value, language, MediaType.Json,
                 (response) => Task.FromResult(response)
                , false, timeOut);
        }
    }
}