using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    public class AutoBindingMetaModelAttributeTests
    {
        private readonly Mock<IMetaModelBindingService> _bindingService = new();
        private readonly Mock<ILicenseeService> _licenseeService = new();
        private readonly Mock<ISubsidiaryService> _subsidiaryService = new();
        private readonly Mock<IContextService> _contextService = new();

        private ActionExecutingContext CreateContext(object metaModel)
        {
            var actionArguments = new Dictionary<string, object> { { "model", metaModel } };
            var httpContext = new DefaultHttpContext();
            return new ActionExecutingContext(
                new ActionContext(httpContext, new RouteData(), new ActionDescriptor()),
                new List<IFilterMetadata>(),
                actionArguments,
                controller: null);
        }

        [Fact]
        public async Task OnActionExecutionAsync_BindSiteName_SetsSiteName()
        {
            // Arrange
            var metaModel = new SiteNameMetaModelStub();
            _contextService.SetupGet(x => x.SiteName).Returns("TestSite");
            var filter = new AutoBindingMetaModelImplAttribute(
                _bindingService.Object, _licenseeService.Object, _subsidiaryService.Object, _contextService.Object,
                bindAgent: false, bindSiteName: true, bindLanguage: false, bindClientIP: false, bindUserId: false, bindBalanceUp: false, bindLicenseeId: false, bindUsername: false, bindProductList: false);
            var context = CreateContext(metaModel);
            var next = new Mock<ActionExecutionDelegate>();
            next.Setup(n => n()).Returns(Task.FromResult<ActionExecutedContext>(null));

            // Act
            await filter.OnActionExecutionAsync(context, next.Object);

            // Assert
            Assert.Equal("TestSite", metaModel.SiteName);
        }

        [Fact]
        public async Task OnActionExecutionAsync_BindLanguage_SetsLanguage()
        {
            var metaModel = new LanguageMetaModelStub();
            _contextService.SetupGet(x => x.Language).Returns("en");
            var filter = new AutoBindingMetaModelImplAttribute(
                _bindingService.Object, _licenseeService.Object, _subsidiaryService.Object, _contextService.Object,
                bindAgent: false, bindSiteName: false, bindLanguage: true, bindClientIP: false, bindUserId: false, bindBalanceUp: false, bindLicenseeId: false, bindUsername: false, bindProductList: false);
            var context = CreateContext(metaModel);
            var next = new Mock<ActionExecutionDelegate>();
            next.Setup(n => n()).Returns(Task.FromResult<ActionExecutedContext>(null));
            await filter.OnActionExecutionAsync(context, next.Object);
            Assert.Equal("en", metaModel.Language);
        }

        [Fact]
        public async Task OnActionExecutionAsync_BindClientIP_SetsIp()
        {
            var metaModel = new ClientIPMetaModelStub();
            _contextService.SetupGet(x => x.CurrentRequestClientIP).Returns("127.0.0.1");
            var filter = new AutoBindingMetaModelImplAttribute(
                _bindingService.Object, _licenseeService.Object, _subsidiaryService.Object, _contextService.Object,
                bindAgent: false, bindSiteName: false, bindLanguage: false, bindClientIP: true, bindUserId: false, bindBalanceUp: false, bindLicenseeId: false, bindUsername: false, bindProductList: false);
            var context = CreateContext(metaModel);
            var next = new Mock<ActionExecutionDelegate>();
            next.Setup(n => n()).Returns(Task.FromResult<ActionExecutedContext>(null));
            await filter.OnActionExecutionAsync(context, next.Object);
            Assert.Equal("127.0.0.1", metaModel.Ip);
        }

        [Fact]
        public async Task OnActionExecutionAsync_BindUserId_SetsUserId()
        {
            var metaModel = new UserIdMetaModelStub();
            _contextService.SetupGet(x => x.UserId).Returns(42);
            var filter = new AutoBindingMetaModelImplAttribute(
                _bindingService.Object, _licenseeService.Object, _subsidiaryService.Object, _contextService.Object,
                bindAgent: false, bindSiteName: false, bindLanguage: false, bindClientIP: false, bindUserId: true, bindBalanceUp: false, bindLicenseeId: false, bindUsername: false, bindProductList: false);
            var context = CreateContext(metaModel);
            var next = new Mock<ActionExecutionDelegate>();
            next.Setup(n => n()).Returns(Task.FromResult<ActionExecutedContext>(null));
            await filter.OnActionExecutionAsync(context, next.Object);
            Assert.Equal(42, metaModel.UserId);
        }

        // Stub meta models for testing
        private class SiteNameMetaModelStub : ISiteNameMetaModel { public string SiteName { get; set; } }
        private class LanguageMetaModelStub : ILanguageMetaModel { public string Language { get; set; } }
        private class ClientIPMetaModelStub : IClientIPMetaModel { public string Ip { get; set; } }
        private class UserIdMetaModelStub : IUserIdMetaModel { public int UserId { get; set; } }
    }
} 