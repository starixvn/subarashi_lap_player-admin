namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    using System.Net;
    using Core.Attributes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using Moq;

    public class ModelStateValidationAttributeTest
    {
        [Fact]
        public void OnActionExecuting_WhenModelStateIsValid_DoesNotSetResult()
        {
            // Arrange
            var attribute = new ModelStateValidationAttribute();
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
        public void OnActionExecuting_WhenModelStateIsInvalid_SetsJsonResult()
        {
            // Arrange
            var attribute = new ModelStateValidationAttribute();
            var actionContext = CreateActionContext();
            actionContext.ModelState.AddModelError("TestProperty", "Test error message");

            var context = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            // Action
            attribute.OnActionExecuting(context);

            // Assert
            Assert.NotNull(context.Result);
            Assert.IsType<JsonResult>(context.Result);

            var jsonResult = context.Result as JsonResult;
            Assert.Equal((int)HttpStatusCode.BadRequest, jsonResult.StatusCode);

            var resultValue = jsonResult.Value;
            var isSuccessProperty = resultValue.GetType().GetProperty("IsSuccess");
            var msgProperty = resultValue.GetType().GetProperty("Msg");

            Assert.NotNull(isSuccessProperty);
            Assert.NotNull(msgProperty);
            Assert.False((bool)isSuccessProperty.GetValue(resultValue));
            Assert.Equal("Model state is invalid", msgProperty.GetValue(resultValue));
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