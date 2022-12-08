using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using NPOI.HSSF.Record.Chart;
using NPOI.OpenXml4Net.OPC.Internal.Unmarshallers;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Policy;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    public class ReferrerValidationAttributeTests
    {
        private IOptions<AppSetting> _options;
        private AppSetting _appSetting;
        private ActionExecutingContext _currentContext;

        [SetUp]
        public void SetUp()
        {
            _currentContext = ActionContextHelper.FakeActionExecutingContext();

            _options = Substitute.For<IOptions<AppSetting>>();
            _appSetting = FakeHelper.Create<AppSetting>();
            _options.Value.Returns(_appSetting);
            _currentContext = ActionContextHelper.FakeActionExecutingContext();
        }

        [Test]
        public void ReferrerValidationAttribute_WhenInit_NotThrowException()
        {
            //Arrange
            Assert.DoesNotThrow(() => new ReferrerValidationAttribute());
        }

        [Test]
        public void OnActionExecuting_WhenAllowedCrossDomainsNotSetting_NotHandleInValidRequest()
        {
            //Arrange
            _appSetting.AllowedCrossDomains = null;
            var isHandleInvalidRequest = false;
            _currentContext.WhenForAnyArgs(_ => _.HandleInvalidRequest(default))
                .Do(_ => isHandleInvalidRequest = true);

            var attr = CreateAttribute();

            //Act
            attr.OnActionExecuting(_currentContext);

            //Asset
            Assert.IsFalse(isHandleInvalidRequest);
        }

        [Test]
        public void OnActionExecuting_WhenReferrerUrlIsEmpty_HandleInvalidRequest()
        {
            //Arrange
            _appSetting.AllowedCrossDomains = FakeHelper.CreateMany<string>();
            SetUpReferrerUrl(string.Empty);

            var isHandleInvalidRequest = false;
            _currentContext.WhenForAnyArgs(_ => _.HandleInvalidRequest(default))
                .Do(_ => isHandleInvalidRequest = true);

            var attr = CreateAttribute();

            //Act
            attr.OnActionExecuting(_currentContext);

            //Assert
            Assert.IsTrue(isHandleInvalidRequest);
        }

        [Test]
        public void OnActionExecuting_WhenReferrerURLEqualToDomain_NotCheckValidRequest()
        {
            _appSetting.AllowedCrossDomains = FakeHelper.CreateMany<string>();
            var url = "http://localhost.com";
            SetUpReferrerUrl(url);
            SetUpHostUrl("localhost.com");

            var isHandleInvalidRequest = false;
            _currentContext.WhenForAnyArgs(_ => _.HandleInvalidRequest(default))
                .Do(_ => isHandleInvalidRequest = true);

            var attr = CreateAttribute();

            //Act
            attr.OnActionExecuting(_currentContext);

            //Assert
            Assert.IsFalse(isHandleInvalidRequest);
        }

        [Test]
        public void OnActionExecuting_WhenAllowDomainContainClientDomain_NotCheckValidRequest()
        {

            var clientDomain = "localhost.com";
            var allowDomains = new List<string> { clientDomain };
            SetUpReferrerUrl($"http://{clientDomain}");
            _appSetting.AllowedCrossDomains = allowDomains;
            SetUpHostUrl(string.Empty);

            var isHandleInvalidRequest = false;
            _currentContext.WhenForAnyArgs(_ => _.HandleInvalidRequest(default))
                .Do(_ => isHandleInvalidRequest = true);

            var attr = CreateAttribute();

            //Act
            attr.OnActionExecuting(_currentContext);

            //Assert
            Assert.IsFalse(isHandleInvalidRequest);
        }

        [Test]
        public void OnActionExecuting_WhenAllowDomainNotContainClientDomain_CheckValidRequest()
        {
            var clientDomain = "localhost.com";
            SetUpReferrerUrl($"http://{clientDomain}");
            _appSetting.AllowedCrossDomains = new List<string> { "abc" };
            SetUpHostUrl(string.Empty);

            var isHandleInvalidRequest = false;
            _currentContext.WhenForAnyArgs(_ => _.HandleInvalidRequest(default))
                .Do(_ => isHandleInvalidRequest = true);

            var attr = CreateAttribute();

            //Act
            attr.OnActionExecuting(_currentContext);

            //Assert
            Assert.IsTrue(isHandleInvalidRequest);
        }

        private ReferrerValidationImplAttribute CreateAttribute()
            => new ReferrerValidationImplAttribute(_options);

        private void SetUpReferrerUrl(string url)
        {
            var refferValue = new StringValues(url);
            _currentContext.HttpContext.Request.Headers
            .TryGetValue(HeaderNames.Referer, out Arg.Any<StringValues>())
            .Returns(_ =>
            {
                _[1] = refferValue;
                return true;
            });
        }

        private void SetUpHostUrl(string url)
        {
            var host = new HostString(url);
            _currentContext.HttpContext.Request.Host.Returns(host);
        }
    }
}
