using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    public class RequestValidationAttributeTests
    {
        private IContextService _contextService;
        private ILicenseeService _licenseeService;
        private ActionExecutingContext _currentContext;
        private ActionExecutionDelegate _next;

        [SetUp]
        public void SetUp()
        {
            _contextService = Substitute.For<IContextService>();
            _licenseeService = Substitute.For<ILicenseeService>();

            _currentContext = ActionContextHelper.FakeActionExecutingContext();
            _next = () => Task.FromResult(ActionContextHelper.FakeActionExecutedContext());
        }

        [Test]
        public void RequestValidationAttribute_WhenInit_ArgumentsSizeCorrectly()
        {
            //Arrange
            var attr = new RequestValidationAttribute();

            //Assert
            Assert.That(attr.Arguments.Count(), Is.EqualTo(4));
        }

        [Test]
        public async Task OnActionExecutionAsync_WhenProductBelongToLicensee_NotCallHandleInvalidRequest()
        {
            //Arrange
            var isHandleInvalidRequest = false;
            _currentContext.WhenForAnyArgs(_ => _.HandleInvalidRequest(default))
                .Do(_ => isHandleInvalidRequest = true);
            _licenseeService.CheckProductBelongsTo(default, default, default).ReturnsForAnyArgs(true);

            var attr = CreateAttribute();

            //Act
            await attr.OnActionExecutionAsync(_currentContext, _next);

            //Assert
            Assert.IsFalse(isHandleInvalidRequest);
        }

        private DataValidationImplAttribute CreateAttribute(
            bool validateCustId = false,
            bool validateClientIP = false,
            int validateProductId = 0,
            int[] limitedProductIds = default
            )
        
            => new DataValidationImplAttribute(
                _contextService, 
                _licenseeService, 
                validateCustId, 
                validateClientIP, 
                validateProductId,
                limitedProductIds);
        
    }
}
