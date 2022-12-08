using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using System.Net.Http;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient.TestableApiClients
{
    public class LgApiClientTestable : LgApiClient
    {
        public LgApiClientTestable(HttpClient client)
            : base(client)
        { }

        internal string Public_ApiName => ApiName;
    }
}
