namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using Moq;
    using Settings;
    using Xunit;

    public class FunctionUMAttributeTest
    {
        [Fact]
        public async Task OnActionExecutionAsync_WhenNotUnderMaintenance_CallsNext()
        {
            // Arrange
            var mockUMService = new Mock<IFunctionUMService>();
            mockUMService.Setup(x => x.GetUnderMaintenanceInfo(FunctionUMType.Commission))
                .Returns((false, null, null));

            var attribute = new FunctionUMImplAttribute(mockUMService.Object, FunctionUMType.Commission);
            var actionContext = CreateActionContext();
            var context = CreateActionExecutingContext(actionContext);


            var nextCalled = false;

            Task<ActionExecutedContext> next()

            {
                nextCalled = true;
                return Task.FromResult(new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), new Mock<Controller>().Object));
            }

            // Action
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.True(nextCalled);
            Assert.Null(context.Result);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenUnderMaintenanceAndAjaxRequest_SetsJsonResult()
        {
            // Arrange
            var mockUMService = new Mock<IFunctionUMService>();
            mockUMService.Setup(x => x.GetUnderMaintenanceInfo(FunctionUMType.Commission))
                .Returns((true, "2023-01-01 10:00", "2023-01-01 12:00"));

            var attribute = new FunctionUMImplAttribute(mockUMService.Object, FunctionUMType.Commission);
            var actionContext = CreateActionContext();

            // Set up AJAX request

            actionContext.HttpContext.Request.Headers["X-Requested-With"] = "XMLHttpRequest";


            var context = CreateActionExecutingContext(actionContext);


            var nextCalled = false;

            Task<ActionExecutedContext> next()

            {
                nextCalled = true;
                return Task.FromResult(new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), new Mock<Controller>().Object));
            }

            // Action
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.False(nextCalled);
            Assert.NotNull(context.Result);
            Assert.IsType<JsonResult>(context.Result);


            var jsonResult = context.Result as JsonResult;
            Assert.Equal((int)HttpStatusCode.OK, jsonResult.StatusCode);


            var resultValue = jsonResult.Value;
            var codeProperty = resultValue.GetType().GetProperty("Code");
            var messageProperty = resultValue.GetType().GetProperty("Message");


            Assert.NotNull(codeProperty);
            Assert.NotNull(messageProperty);
            Assert.Equal(999999, codeProperty.GetValue(resultValue));


            var message = messageProperty.GetValue(resultValue) as string;
            Assert.Contains("2023-01-01 10:00", message);
            Assert.Contains("2023-01-01 12:00", message);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenUnderMaintenanceAndNotAjaxRequest_DoesNotSetResult()
        {
            // Arrange
            var mockUMService = new Mock<IFunctionUMService>();
            mockUMService.Setup(x => x.GetUnderMaintenanceInfo(FunctionUMType.BetSetting))
                .Returns((true, "2023-01-01 10:00", "2023-01-01 12:00"));

            var attribute = new FunctionUMImplAttribute(mockUMService.Object, FunctionUMType.BetSetting);
            var actionContext = CreateActionContext();
            var context = CreateActionExecutingContext(actionContext);


            var nextCalled = false;

            Task<ActionExecutedContext> next()

            {
                nextCalled = true;
                return Task.FromResult(new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), new Mock<Controller>().Object));
            }

            // Action
            await attribute.OnActionExecutionAsync(context, next);

            // Assert
            Assert.False(nextCalled);
            Assert.Null(context.Result);
        }

        [Fact]
        public void FunctionUMAttribute_Constructor_SetsCorrectArguments()
        {
            // Arrange & Action
            var attribute = new FunctionUMAttribute(FunctionUMType.Commission);

            // Assert
            Assert.Equal(typeof(FunctionUMImplAttribute), attribute.ImplementationType);
            Assert.Equal(-1, attribute.Order);
            Assert.Single(attribute.Arguments);
            Assert.Equal(FunctionUMType.Commission, attribute.Arguments[0]);
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

        private static ActionExecutingContext CreateActionExecutingContext(ActionContext actionContext)
        {
            return new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);
        }
    }
}