using Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient.TestableApiClients;
using NSubstitute;
using NUnit.Framework;
using System.Net.Http;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient
{
    public class LgApiClientTests
    {
        private HttpMessageHandler _handler;
        private HttpClient _httpClient;

        [SetUp]
        public void SetUp()
        {
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
            Assert.That(client.Public_ApiName, Is.EqualTo("LG"));
        }

        private LgApiClientTestable CreateClient()
            => Substitute.ForPartsOf<LgApiClientTestable>(_httpClient);
    }
}
