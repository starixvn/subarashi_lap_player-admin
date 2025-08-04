namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    using System.Collections.Generic;
    using Core.Attributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Options;
    using Moq;
    using Settings;
    using Xunit;

    public class ReferrerValidationAttributeTest
    {
        [Fact]
        public void OnActionExecuting_WhenAllowedCrossDomainsIsEmpty_DoesNotSetResult()
        {
            // Arrange
            var appSetting = new AppSetting { AllowedCrossDomains = new string[0] };
            var options = Options.Create(appSetting);
            var attribute = new ReferrerValidationImplAttribute(options);


            var actionContext = CreateActionContext();
            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            // Action
            attribute.OnActionExecuting(context);

            // Assert
            Assert.Null(context.Result);
        }

        [Fact]
        public void OnActionExecuting_WhenAllowedCrossDomainsIsNull_DoesNotSetResult()
        {
            // Arrange
            var appSetting = new AppSetting { AllowedCrossDomains = null };
            var options = Options.Create(appSetting);
            var attribute = new ReferrerValidationImplAttribute(options);


            var actionContext = CreateActionContext();
            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            // Action
            attribute.OnActionExecuting(context);

            // Assert
            Assert.Null(context.Result);
        }

        [Fact]
        public void OnActionExecuting_WhenNoReferrerHeader_SetsRedirectResult()
        {
            // Arrange
            var appSetting = new AppSetting { AllowedCrossDomains = new[] { "example.com" } };
            var options = Options.Create(appSetting);
            var attribute = new ReferrerValidationImplAttribute(options);


            var actionContext = CreateActionContext();
            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            // Action
            attribute.OnActionExecuting(context);

            // Assert
            Assert.NotNull(context.Result);
            Assert.IsType<RedirectResult>(context.Result);


            var redirectResult = context.Result as RedirectResult;
            Assert.Equal("/Home/Error?code=400", redirectResult.Url);
        }

        [Fact]
        public void OnActionExecuting_WhenReferrerMatchesHost_DoesNotSetResult()
        {
            // Arrange
            var appSetting = new AppSetting { AllowedCrossDomains = new[] { "example.com" } };
            var options = Options.Create(appSetting);
            var attribute = new ReferrerValidationImplAttribute(options);


            var actionContext = CreateActionContext();
            actionContext.HttpContext.Request.Host = new HostString("localhost:5000");
            actionContext.HttpContext.Request.Headers["Referer"] = "http://localhost:5000/test";


            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            // Action
            attribute.OnActionExecuting(context);

            // Assert
            Assert.Null(context.Result);
        }

        [Fact]
        public void OnActionExecuting_WhenReferrerMatchesAllowedDomain_DoesNotSetResult()
        {
            // Arrange
            var appSetting = new AppSetting { AllowedCrossDomains = new[] { "example.com" } };
            var options = Options.Create(appSetting);
            var attribute = new ReferrerValidationImplAttribute(options);


            var actionContext = CreateActionContext();
            actionContext.HttpContext.Request.Host = new HostString("localhost:5000");
            actionContext.HttpContext.Request.Headers["Referer"] = "http://test.example.com/test";


            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            // Action
            attribute.OnActionExecuting(context);

            // Assert
            Assert.Null(context.Result);
        }

        [Fact]
        public void OnActionExecuting_WhenReferrerDoesNotMatchAllowedDomains_SetsInvalidResult()
        {
            // Arrange
            var appSetting = new AppSetting { AllowedCrossDomains = new[] { "example.com" } };
            var options = Options.Create(appSetting);
            var attribute = new ReferrerValidationImplAttribute(options);


            var actionContext = CreateActionContext();
            actionContext.HttpContext.Request.Host = new HostString("localhost:5000");
            actionContext.HttpContext.Request.Headers["Referer"] = "http://malicious.com/test";


            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            // Action
            attribute.OnActionExecuting(context);

            // Assert
            Assert.NotNull(context.Result);
            Assert.IsType<RedirectResult>(context.Result);


            var redirectResult = context.Result as RedirectResult;
            Assert.Equal("/Home/Error?code=400", redirectResult.Url);
        }

        private static ActionContext CreateActionContext()
        {
            var modelState = new ModelStateDictionary();
            var httpContext = new DefaultHttpContext();
            return new ActionContext(
                httpContext: httpContext,
                routeData: new RouteData(),
                actionDescriptor: new ActionDescriptor(),
                modelState: modelState
            );
        }
    }
}