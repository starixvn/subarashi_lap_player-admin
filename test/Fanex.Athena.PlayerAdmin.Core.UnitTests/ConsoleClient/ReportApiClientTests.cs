
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient.TestableApiClients;
using NSubstitute;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient
{
    public class ReportApiClientTests
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
            Assert.That(client.Public_ApiName, Is.EqualTo("Report"));
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
            Assert.IsTrue(header.Contains("X-IgnoreSharding"));
            Assert.That(header.Authorization.Scheme, Is.EqualTo("Bearer"));
        }

        private ReportApiClientTestable CreateClient()
            => Substitute.ForPartsOf<ReportApiClientTestable>(_httpClient, _contextService);
    }
}
