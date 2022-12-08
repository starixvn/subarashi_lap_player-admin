using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using System;

namespace Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers
{
    public static class HttpContextHelper
    {
        public static HttpContext FakeRequestHeader(this HttpContext context, string key, string value)
        {
            if (!context.IsSubstitute())
            {
                throw new InvalidOperationException("HttpContext not is a substitute class");
            }

            if (context.Request == null || !context.Request.IsSubstitute())
            {
                context.Request.Returns(Substitute.For<HttpRequest>());
            }

            if (context.Request.Headers == null || !context.Request.Headers.IsSubstitute())
            {
                context.Request.Headers.Returns(Substitute.For<IHeaderDictionary>());
            }

            var headers = context.Request.Headers;
            headers[key].Returns(new StringValues(value));
            headers.TryGetValue(key, out var stringValue).Returns(_ =>
            {
                _[1] = new StringValues(value);
                return true;
            });

            return context;
        }

        public static HttpContext FakeAjaxRequest(this HttpContext context)
        {
            context.FakeRequestHeader("X-Requested-With", "XMLHttpRequest");
            return context;
        }

    }
}
