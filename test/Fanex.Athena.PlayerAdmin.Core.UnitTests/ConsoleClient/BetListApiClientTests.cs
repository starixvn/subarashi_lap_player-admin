
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient.TestableApiClients;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient
{
    public class BetListApiClientTests
    {
        private IContextService _contextService;
        private HttpMessageHandler _handler;
        private HttpClient _httpClient;

        [SetUp]
        public void SetUp()
        {
            _contextService = Substitute.For<IContextService>();
            _handler = Substitute.For<HttpMessageHandler>();
            _httpClient = Substitute.For<HttpClient>(_handler);
            _httpClient.BaseAddress = new System.Uri("http://localhost.com");
        }

        [Test]
        public void ApiName_WhenCall_ReturnCorrectly()
        {
            //Arrange
            var client = CreateClient();

            //Assert
            Assert.That(client.Public_ApiName, Is.EqualTo("Bet List"));
        }

        [Test]
        public async Task PrepareHeaders_WhenCall_AddRespectiveHeader()
        {
            //Arrange
            var client = CreateClient();
            var header = new HttpRequestMessage().Headers;

            //Act
            await client.Public_PrepareHeaders(header);

            //Assert
            Assert.IsTrue(header.Contains("X-SiteDBServer"));
        }

        [Test]
        public async Task PostAsync_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var client = CreateClient();
            _handler.SubstituteProtected("SendAsync", Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            //Act
            var result = await client.PostAsync("test", new { a = "1" }, "en-US");

            //Assert
            _handler.Received(1).SubstituteProtected("SendAsync",
                Arg.Is<HttpRequestMessage>(_ => _.Headers.Accept.Any(_ => _.MediaType == "application/json")),
                Arg.Any<CancellationToken>());
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        private BetListApiClientTestable CreateClient()
            => Substitute.ForPartsOf<BetListApiClientTestable>(_httpClient, _contextService);
    }
}
