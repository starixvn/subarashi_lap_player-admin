namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    using System.Collections.Generic;
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
    using Xunit;

    public class RequestValidationAttributeTest
    {
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<ILicenseeService> _mockLicenseeService;

        public RequestValidationAttributeTest()
        {
            _mockContextService = new Mock<IContextService>();
            _mockLicenseeService = new Mock<ILicenseeService>();
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenAllValidationsPass_CallsNext()
        {
            // Arrange
            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                false, false, -99, new int[0]);

            var actionContext = CreateActionContext();
            var metaModel = new TestValidationMetaModel { IsValid = true };
            var context = CreateActionExecutingContext(actionContext, metaModel);

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
        public async Task OnActionExecutionAsync_WhenValidationMetaModelIsInvalid_SetsInvalidResult()
        {
            // Arrange
            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                false, false, -99, new int[0]);

            var actionContext = CreateActionContext();
            var metaModel = new TestValidationMetaModel { IsValid = false };
            var context = CreateActionExecutingContext(actionContext, metaModel);

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
            Assert.IsType<RedirectResult>(context.Result);

            var redirectResult = context.Result as RedirectResult;
            Assert.Equal("/Home/Error?code=400", redirectResult.Url);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenValidateCustIdTrueAndCustomerBelongs_CallsNext()
        {
            // Arrange
            var agentList = new Dictionary<int, int[]> { { 1, new[] { 100, 200 } } };
            _mockContextService.Setup(x => x.AgentList).Returns(agentList);
            _mockLicenseeService.Setup(x => x.CheckCustomerBelongTo(agentList, 123, 1))
                .ReturnsAsync(true);

            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                true, false, -99, new int[0]);

            var actionContext = CreateActionContext();
            var metaModel = new TestCombinedMetaModel
            {
                IsValid = true,
                CustId = 123,
                CurrencyId = 1
            };
            var context = CreateActionExecutingContext(actionContext, metaModel);

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
        public async Task OnActionExecutionAsync_WhenValidateCustIdTrueAndCustomerDoesNotBelong_SetsInvalidResult()
        {
            // Arrange
            var agentList = new Dictionary<int, int[]> { { 1, new[] { 100, 200 } } };
            _mockContextService.Setup(x => x.AgentList).Returns(agentList);
            _mockLicenseeService.Setup(x => x.CheckCustomerBelongTo(agentList, 123, 1))
                .ReturnsAsync(false);

            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                true, false, -99, new int[0]);

            var actionContext = CreateActionContext();
            var metaModel = new TestCombinedMetaModel
            {
                IsValid = true,
                CustId = 123,
                CurrencyId = 1
            };
            var context = CreateActionExecutingContext(actionContext, metaModel);

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
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenValidateClientIPTrueAndIPsMatch_CallsNext()
        {
            // Arrange
            _mockContextService.Setup(x => x.CurrentRequestClientIP).Returns("192.168.1.1");
            _mockContextService.Setup(x => x.OwnerClientIP).Returns("192.168.1.1");

            var attribute = new DataValidationImplAttribute(
                _mockContextService.Object,
                _mockLicenseeService.Object,
                false, true, -99, new int[0]);

            var actionContext = CreateActionContext();
            var metaModel = new TestValidationMetaModel { IsValid = true };
            var context = CreateActionExecutingContext(actionContext, metaModel);

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
        public void RequestValidationAttribute_Constructor_SetsCorrectArguments()
        {
            // Arrange & Action
            var attribute = new RequestValidationAttribute(true, true, 5, [1, 2, 3]);

            // Assert
            Assert.Equal(typeof(DataValidationImplAttribute), attribute.ImplementationType);
            Assert.Equal(4, attribute.Arguments.Length);
            Assert.True((bool)attribute.Arguments[0]);
            Assert.True((bool)attribute.Arguments[1]);
            Assert.Equal(5, attribute.Arguments[2]);
            Assert.IsType<int[]>(attribute.Arguments[3]);
            var limitedProductIds = (int[])attribute.Arguments[3];
            Assert.Equal([1, 2, 3], limitedProductIds);
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

        private static ActionExecutingContext CreateActionExecutingContext(ActionContext actionContext, object metaModel = null)
        {
            var actionArguments = new Dictionary<string, object>();
            if (metaModel != null)
            {
                actionArguments.Add("model", metaModel);
            }

            return new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                actionArguments,
                new Mock<Controller>().Object);
        }
    }
}