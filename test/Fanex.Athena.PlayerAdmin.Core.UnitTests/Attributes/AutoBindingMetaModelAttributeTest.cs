namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Services;
    using Fanex.Core.Utils;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using Models;
    using Moq;
    using Xunit;

    public class AutoBindingMetaModelAttributeTest
    {
        private readonly Mock<IMetaModelBindingService> _mockBindingService;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<ILicenseeService> _mockLicenseeService;
        private readonly Mock<ISubsidiaryService> _mockSubsidiaryService;

        public AutoBindingMetaModelAttributeTest()
        {
            _mockBindingService = new Mock<IMetaModelBindingService>();
            _mockLicenseeService = new Mock<ILicenseeService>();
            _mockSubsidiaryService = new Mock<ISubsidiaryService>();
            _mockContextService = new Mock<IContextService>();
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenBindSiteNameTrue_SetsSiteName()
        {
            // Arrange
            _mockContextService.Setup(x => x.SiteName).Returns("TestSite");

            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockLicenseeService.Object,
                _mockSubsidiaryService.Object,
                _mockContextService.Object,
                false, true, false, false, false, false, false, false, false);

            var actionContext = CreateActionContext();
            var metaModel = new TestSiteNameMetaModel();
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
            Assert.Equal("TestSite", metaModel.SiteName);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenBindLanguageTrue_SetsLanguage()
        {
            // Arrange
            _mockContextService.Setup(x => x.Language).Returns("en-US");

            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockLicenseeService.Object,
                _mockSubsidiaryService.Object,
                _mockContextService.Object,
                false, false, true, false, false, false, false, false, false);

            var actionContext = CreateActionContext();
            var metaModel = new TestLanguageMetaModel();
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
            Assert.Equal("en-US", metaModel.Language);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenBindClientIPTrue_SetsClientIP()
        {
            // Arrange
            _mockContextService.Setup(x => x.CurrentRequestClientIP).Returns("192.168.1.1");

            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockLicenseeService.Object,
                _mockSubsidiaryService.Object,
                _mockContextService.Object,
                false, false, false, true, false, false, false, false, false);

            var actionContext = CreateActionContext();
            var metaModel = new TestClientIPMetaModel();
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
            Assert.Equal("192.168.1.1", metaModel.Ip);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenBindUserIdTrue_SetsUserId()
        {
            // Arrange
            _mockContextService.Setup(x => x.UserId).Returns(123);

            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockLicenseeService.Object,
                _mockSubsidiaryService.Object,
                _mockContextService.Object,
                false, false, false, false, true, false, false, false, false);

            var actionContext = CreateActionContext();
            var metaModel = new TestUserIdMetaModel();
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
            Assert.Equal(123, metaModel.UserId);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenBindLicenseeIdTrue_SetsLicenseeId()
        {
            // Arrange
            _mockContextService.Setup(x => x.LicenseeId).Returns(456);

            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockLicenseeService.Object,
                _mockSubsidiaryService.Object,
                _mockContextService.Object,
                false, false, false, false, false, false, true, false, false);

            var actionContext = CreateActionContext();
            var metaModel = new TestLicenseeIdMetaModel();
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
            Assert.Equal(456, metaModel.LicenseeId);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenBindUsernameTrue_SetsUsername()
        {
            // Arrange
            _mockContextService.Setup(x => x.Username).Returns("testuser");

            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockLicenseeService.Object,
                _mockSubsidiaryService.Object,
                _mockContextService.Object,
                false, false, false, false, false, false, false, true, false);

            var actionContext = CreateActionContext();
            var metaModel = new TestUsernameMetaModel();
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
            Assert.Equal("testuser", metaModel.Username);
        }

        [Fact]
        public async Task OnActionExecutionAsync_WhenBindBalanceUpTrue_SetsBalanceUpData()
        {
            // Arrange
            var balanceUpData = new BalanceUpData
            {
                BalanceUpDay = new DateTime(2023, 1, 1),
                MoveBetsDay = new DateTime(2023, 1, 2)
            };
            _mockLicenseeService.Setup(x => x.GetBalanceUp()).ReturnsAsync(balanceUpData);

            var attribute = new AutoBindingMetaModelImplAttribute(
                _mockBindingService.Object,
                _mockLicenseeService.Object,
                _mockSubsidiaryService.Object,
                _mockContextService.Object,
                false, false, false, false, false, true, false, false, false);

            var actionContext = CreateActionContext();
            var metaModel = new TestBalanceUpAndMoveBetMetaModel();
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
            Assert.Equal(new DateTime(2023, 1, 1), metaModel.BalanceUpDay);
            Assert.Equal(new DateTime(2023, 1, 2), metaModel.MoveBetsDay);
        }

        [Fact]
        public void AutoBindingMetaModelAttribute_Constructor_SetsCorrectArguments()
        {
            // Arrange & Action
            var attribute = new AutoBindingMetaModelAttribute(
                bindAgent: true,
                bindSiteName: true,
                bindLanguage: true,
                bindClientIP: true,
                bindUserId: true,
                bindBalanceUp: true,
                bindLicenseeId: true,
                bindUsername: true,
                bindProductList: true);

            // Assert
            Assert.Equal(typeof(AutoBindingMetaModelImplAttribute), attribute.ImplementationType);
            Assert.Equal(9, attribute.Arguments.Length);
            Assert.True((bool)attribute.Arguments[0]); // bindAgent
            Assert.True((bool)attribute.Arguments[1]); // bindSiteName
            Assert.True((bool)attribute.Arguments[2]); // bindLanguage
            Assert.True((bool)attribute.Arguments[3]); // bindClientIP
            Assert.True((bool)attribute.Arguments[4]); // bindUserId
            Assert.True((bool)attribute.Arguments[5]); // bindBalanceUp
            Assert.True((bool)attribute.Arguments[6]); // bindLicenseeId
            Assert.True((bool)attribute.Arguments[7]); // bindUsername
            Assert.True((bool)attribute.Arguments[8]); // bindProductList
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