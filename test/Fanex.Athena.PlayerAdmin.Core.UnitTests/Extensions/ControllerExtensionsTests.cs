namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Extensions
{
    using System.Net;
    using System.Text;
    using Core.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Routing;

    public class ControllerExtensionsTests
    {
        [Fact]
        public async Task ToJsonResult_WritesJsonWithOkStatusAndContentType()
        {
            var controller = new ControllerBaseMock();
            var actionResult = controller.ToJsonResult("{\"a\":1}");

            var httpContext = new DefaultHttpContext();
            httpContext.Response.Body = new MemoryStream();
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            await actionResult.ExecuteResultAsync(actionContext);

            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = new StreamReader(httpContext.Response.Body, Encoding.UTF8).ReadToEnd();

            Assert.Equal("application/json", httpContext.Response.ContentType);
            Assert.Equal((int)HttpStatusCode.OK, httpContext.Response.StatusCode);
            Assert.Equal("{\"a\":1}", body);
        }

        private class ControllerBaseMock : ControllerBase
        {
        }
    }
}