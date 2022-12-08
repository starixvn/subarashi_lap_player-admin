

using Fanex.Athena.PlayerAdmin.Core.Middlewares;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NUnit.Framework;
using ONEbook.UM.Models;
using ONEbook.UM.Services;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Middlewares
{
    public class UnderMaintenanceHandlerTests
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        private RequestDelegate Next { get; set; }
        private IMaintenanceService MaintenanceService { get; set; }
        private HttpContext Context { get; set; }

        [SetUp]
        public void SetUp()
        {
            Next = Substitute.For<RequestDelegate>();
            MaintenanceService = Substitute.For<IMaintenanceService>();
            Context = Substitute.For<HttpContext>();
            var httpResponse = Substitute.For<HttpResponse>();
            var httpRequest = Substitute.For<HttpRequest>();
            var header = Substitute.For<IHeaderDictionary>();
            httpRequest.Headers.Returns(header);
            Context.Request.Returns(httpRequest);
            Context.Response.Returns(httpResponse);
        }

        [Test]
        public async Task Invoke_WhenNotUM_CallNextMiddleware()
        {
            //Arrange
            MaintenanceService.IsUnderMaintenanceAsync().Returns(false);
            var handler = CreateHandler();

            //Act
            await handler.Invoke(Context);

            //Assert
            await Next.Received(1).Invoke(Context);
        }

        [Test]
        public async Task Invoke_WhenUMAndHaveAjaxRequest_ReponseJsonContent()
        {
            //Arrange
            Context.Request.Headers.TryGetValue(RequestedWithHeader, out var content).Returns
                (_ =>
                    {
                        _[1] = new StringValues(XmlHttpRequest);
                        return true;
                    }
                );
            MaintenanceService.IsUnderMaintenanceAsync().Returns(true);
            MaintenanceService.GetUnderMaintenanceTimeAsync().Returns(FakeHelper.Create<UnderMaintenanceTime>());
            var handler = CreateHandler();

            //Act
#pragma warning disable S108 // Nested blocks of code should not be left empty
            try
            {
                await handler.Invoke(Context);
            }
            catch { }
#pragma warning restore S108 // Nested blocks of code should not be left empty


            //Assert
            Assert.That(Context.Response.ContentType, Is.EqualTo("application/json"));
        }

        [Test]
        public async Task Invoke_WhenUMAndNotAjaxRequest_ReponseHtmlContent()
        {
            //Arrange
            Context.Request.Headers.TryGetValue(RequestedWithHeader, out var content).Returns(false);
            MaintenanceService.IsUnderMaintenanceAsync().Returns(true);
            MaintenanceService.GetUnderMaintenanceTimeAsync().Returns(FakeHelper.Create<UnderMaintenanceTime>());
            var handler = CreateHandler();

            //Act
#pragma warning disable S108 // Nested blocks of code should not be left empty
            try
            {
                await handler.Invoke(Context);
            }
            catch { }
#pragma warning restore S108 // Nested blocks of code should not be left empty


            //Assert
            Assert.That(Context.Response.ContentType, Is.EqualTo("text/html"));
        }

        private UnderMaintenanceHandler CreateHandler()
            => new UnderMaintenanceHandler(Next, MaintenanceService);
    }
}
