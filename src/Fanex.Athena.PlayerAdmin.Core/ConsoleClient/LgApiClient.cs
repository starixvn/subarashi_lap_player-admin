namespace Fanex.Athena.PlayerAdmin.Core.ConsoleClient
{
    using System.Net.Http;

    public class LgApiClient : BaseApiClient, ILgApiClient
    {
        public LgApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        protected override string ApiName => "LG";
    }
}