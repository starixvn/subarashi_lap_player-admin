using System.Net.Http;

namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    public class LgApiClient : BaseApiClient, ILgApiClient
    {
        public LgApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        protected override string ApiName => "LG";
    }
}