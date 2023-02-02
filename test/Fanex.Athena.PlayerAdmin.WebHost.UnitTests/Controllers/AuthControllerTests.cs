using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Athena.PlayerAdmin.WebHost.Controllers;
using Fanex.Athena.PlayerAdmin.WebHost.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.WebHost.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private IdentityServerSetting _setting;
        private IHttpClientFactory _httpClienFactory;
        private IOptionsSnapshot<IdentityServerSetting> _options;
        private HttpMessageHandler _handler;
        private HttpClient _httpClient;
        private HttpContext _httpContext;

        [SetUp]
        public void SetUp()
        {
            _setting = FakeHelper.Create<IdentityServerSetting>();
            _options = Substitute.For<IOptionsSnapshot<IdentityServerSetting>>();
            _options.Value.Returns(_setting);
            _handler = Substitute.For<HttpMessageHandler>();
            _httpClienFactory= Substitute.For<IHttpClientFactory>();
            _httpClient = Substitute.For<HttpClient>(_handler);
            _httpClient.BaseAddress = new System.Uri("http://localhost.com");
            _httpClienFactory.CreateClient("Auth").Returns(_httpClient);
            _httpContext = Substitute.For<HttpContext>();
        }

        [Test]
        public async Task Token_WhenNullOrEmptyToken_ReturnUnSuccessfulResult()
        {
            //Arrange
            var request = FakeHelper.Create<LapTokenRequest>();
            request.Token = string.Empty;

            var controler = CreateController();

            //Act
            var result = await controler.Token(request);
            var value = result.Value;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsFalse(value.GetPropertyValue<bool>("IsSuccess"));
            });
        }

        [Test]
        public void Token_WhenResponseIsException_ThrowException()
        {
            //Arrange
            var request = FakeHelper.Create<LapTokenRequest>();
            var tokenReponse = ProtocolResponse.FromException<TokenResponse>(new ArgumentException("test"));
            var content = new StringContent(JsonConvert.SerializeObject(tokenReponse, JsonSerializerSettings));
            _httpClient.SendAsync(Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
                .Returns(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content= content });
            var controller = CreateController();
            SetUpMockHttpContextForController(controller);

            //Assert
            Assert.ThrowsAsync<ArgumentException>(() => controller.Token(request));
        }

        private void SetUpMockHttpContextForController(ControllerBase controller)
        {
            var actionContext = new ActionContext(_httpContext, new RouteData(), new ControllerActionDescriptor());
            var controllerContext = new ControllerContext(actionContext);
            controller.ControllerContext = controllerContext;

            var connection = Substitute.For<ConnectionInfo>();
            var ipAddress = new IPAddress(new byte[] { 127, 0, 0, 1 });
            connection.RemoteIpAddress = ipAddress;
            _httpContext.Connection.Returns(connection);
        }

        private AuthController CreateController()
            => Substitute.ForPartsOf<AuthController>(_options, _httpClienFactory);
    }
}
