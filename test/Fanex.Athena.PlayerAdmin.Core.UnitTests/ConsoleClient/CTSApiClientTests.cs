using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient.TestableApiClients;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using NSubstitute;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient
{
    public class CTSApiClientTests
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
            Assert.That(client.Public_ApiName, Is.EqualTo("CTS API"));
        }

        [Test]
        public void HandleUnSuccessRequestAsync_WhenResponseContentNotEmpty_ThrowCorrectException()
        {
            //Arrange
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var content = new StringContent("Test");
            response.Content = content;
            var client = CreateClient();

            //Assert
            var ex = Assert.CatchAsync<HttpRequestException>(() => client.Public_HandleUnSuccessRequestAsync(response));
            Assert.IsTrue(ex.Message.Contains("Test"));
        }

        [Test]
        public void HandleUnSuccessRequestAsync_WhenResponseContentIsEmpty_ThrowCorrectException()
        {
            //Arrange
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var content = new StringContent(string.Empty);
            response.Content = content;
            var client = CreateClient();

            //Assert
            var ex = Assert.CatchAsync<HttpRequestException>(() => client.Public_HandleUnSuccessRequestAsync(response));
            Assert.IsTrue(ex.Message.IndexOf("Status code:") != 0);
        }

        [Test]
        public void PostAsync_WhenApiReturnUnSuccessfulStatusCode_ThrowCorrectException()
        {
            //Arrange
            var client = CreateClient();
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var content = new StringContent("Test");
            response.Content = content;
            _handler.SubstituteProtected("SendAsync", Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(response));

            //Assert
            var ex = Assert.CatchAsync<ServiceException>(() => client.PostAsync("test", new { a = "1" }, "en-US"));
            Assert.IsTrue(ex.Message.Contains("Test"));
        }

        private CTSApiClientTestable CreateClient()
            => Substitute.ForPartsOf<CTSApiClientTestable>(_httpClient);

    }
}
