using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient.TestableApiClients
{
    public class ApiClientTestable : BaseApiClient
    {
        public ApiClientTestable(HttpClient client) : base(client)
        {
        }

        protected override string ApiName => "Test";

        public Task<T> Public_HandleAsync<T>(string path, object value, string language, MediaType mediaType, Func<HttpResponseMessage, Task<T>> codeBlock,
            bool disposeHttpResponse = true, int timeOut = 100)
            => HandleAsync<T>(path, value, language, mediaType, codeBlock, disposeHttpResponse, timeOut);
    }
}
