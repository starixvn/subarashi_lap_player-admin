namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    using System.Net.Http;
    using System.Threading.Tasks;

    public class CTSApiClient : BaseApiClient, ICTSApiClient
    {
        protected override string ApiName => "CTS API";

        public CTSApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<HttpResponseMessage> PostAsync(string path, object value, string language, int timeOut = 100)
            => await HandleAsync(path, value, language, MediaType.Json,
                 response => Task.FromResult(response)
                , false, timeOut);

        protected override async Task HandleUnSuccessRequestAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(content))
            {
                throw new HttpRequestException($"Status code: {response.StatusCode}\n" +
                    $"Message: {content}");
            }
            await base.HandleUnSuccessRequestAsync(response);
        }

    }
}
