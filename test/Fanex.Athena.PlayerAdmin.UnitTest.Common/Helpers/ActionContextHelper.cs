using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers
{
    public static class ActionContextHelper
    {
        public static ActionExecutingContext FakeActionExecutingContext()
        {
            var context = Substitute.For<ActionContext>(Substitute.For<HttpContext>(), new RouteData(), new ActionDescriptor());
            var filters = Substitute.For<IList<IFilterMetadata>>();
            var actionArgs = Substitute.For<IDictionary<string, object>>();
            var controller = FakeHelper.Create<object>();
            return Substitute.For<ActionExecutingContext>(context, filters, actionArgs, controller);
        }

        public static ActionExecutedContext FakeActionExecutedContext()
        {
            var context = Substitute.For<ActionContext>(Substitute.For<HttpContext>(), new RouteData(), new ActionDescriptor());
            var filters = Substitute.For<IList<IFilterMetadata>>();
            var controller = FakeHelper.Create<object>();
            return Substitute.For<ActionExecutedContext>(context, filters, controller);
        }

        public static void BindItemToContextArgument<T>(this ActionExecutingContext context, T bindItem)
        {
            if (context.ActionArguments != null)
            {
                var arguaments = new Dictionary<string, object>();
                context.ActionArguments.Returns(arguaments);
            }

            var arguments = context.ActionArguments;
            arguments.Add(Guid.NewGuid().ToString(), bindItem);
        }
    }
}
