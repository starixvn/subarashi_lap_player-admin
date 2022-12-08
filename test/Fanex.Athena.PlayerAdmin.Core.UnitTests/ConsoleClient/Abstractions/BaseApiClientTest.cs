using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient.TestableApiClients;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient.Abstractions
{
    public class BaseApiClientTest
    {
        private static readonly JsonSerializer _serializer = new JsonSerializer();

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
        public async Task HandleAsync_WhenCodeBlockIsNull_ReturnDefaultValue()
        {
            //Arrange
            var client = CreateClient();

            //Act
            var result = await client.Public_HandleAsync<string>("tst", default, default, default, null);

            //Assert
            Assert.That(result, Is.EqualTo(default(string)));
        }

        [Test]
        public async Task HandleAsync_WhenMediaTypeIsMsgPack_ReceiveCallWithCorrectContentType()
        {
            //Arrange
            var client = CreateClient();
            _handler.SubstituteProtected("SendAsync", Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            //Act
            await client.Public_HandleAsync<string>("test", new { a = "1" }, "en-US", MediaType.MsgPack, TestCodeBlock);

            //Assert
            _handler.Received(1).SubstituteProtected("SendAsync",
                Arg.Is<HttpRequestMessage>(_ => _.Headers.Accept.Any(_ => _.MediaType == "application/x-msgpack")),
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task HandleAsync_WhenMediaTypeIsJson_ReceiveCallWithCorrectContentType()
        {
            //Arrange
            var client = CreateClient();
            _handler.SubstituteProtected("SendAsync", Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            //Act
            await client.Public_HandleAsync<string>("test", new { a = "1" }, "en-US", MediaType.Json, TestCodeBlock);

            //Assert
            _handler.Received(1).SubstituteProtected("SendAsync",
                Arg.Is<HttpRequestMessage>(_ => _.Headers.Accept.Any(_ => _.MediaType == "application/json")),
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task PostAsync_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var testObj = FakeHelper.Create<TestClass>();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var content = new StringContent(JsonConvert.SerializeObject(testObj));
            response.Content = content;
            _handler.SubstituteProtected("SendAsync", Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(response));
            var client = CreateClient();

            //Act
            var result = await client.PostAsync<TestClass>("test", new { a = "1" }, "en-US");

            //Assert
            Assert.That(result.TestId, Is.EqualTo(testObj.TestId));
        }

        [Test]
        public async Task PostStringAsync_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var expectedResult = "expected";
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var content = new StringContent(expectedResult);
            response.Content = content;
            _handler.SubstituteProtected("SendAsync", Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(response));
            var client = CreateClient();

            //Act
            var result = await client.PostStringAsync("test", new { a = "1" }, "en-US");

            //Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        private ApiClientTestable CreateClient()
            => Substitute.ForPartsOf<ApiClientTestable>(_httpClient);

        private readonly Func<HttpResponseMessage, Task<string>> TestCodeBlock = _ => Task.FromResult(string.Empty);

        private class TestClass
        {
            public int TestId { get; set; }
        }
    }
}
