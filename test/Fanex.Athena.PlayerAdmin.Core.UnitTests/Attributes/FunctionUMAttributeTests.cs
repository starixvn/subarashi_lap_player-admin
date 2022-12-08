using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    public class FunctionUMAttributeTests
    {
        private IFunctionUMService _umService;
        private FunctionUMType _umType;
        private ActionExecutingContext _currentContext;
        private ActionExecutionDelegate _next;

        [SetUp]
        public void SetUp()
        {
            _umService = Substitute.For<IFunctionUMService>();
            _umType = FakeHelper.Create<FunctionUMType>();

            _currentContext = ActionContextHelper.FakeActionExecutingContext();
            _next = () => Task.FromResult(ActionContextHelper.FakeActionExecutedContext());
        }

        [Test]
        public void FunctionUMAttribute_WhenInit_ArgumentsHaveCorrectValue()
        {
            //Arrange
            var attr = new FunctionUMAttribute(_umType);

            //Assert
            Assert.That(attr.Arguments, Does.Contain(_umType));
        }

        [Test]
        public async Task OnActionExecutionAsync_WhenIsUMAndIsAjaxRequest_ReturnCorrectJsonResult()
        {
            //Arrange
            _umService.GetUnderMaintenanceInfo(_umType).Returns((true, "2022-10-10", "2022-11-10"));
            _currentContext.HttpContext.FakeAjaxRequest();

            var attr = CreateAttribute();

            //Act
            await attr.OnActionExecutionAsync(_currentContext, _next);

            //Assert
            Assert.That(_currentContext.Result, Is.InstanceOf<JsonResult>());
        }

        [Test]
        public async Task OnActionExecutionAsync_WhenIsUMAndIsNotAjaxRequest_ReturnDefaultResult()
        {
            //Arrange
            _umService.GetUnderMaintenanceInfo(_umType).Returns((true, "2022-10-10", "2022-11-10"));
            var isCallNextMiddleware = false;
            _next = () =>
            {
                isCallNextMiddleware = true;
                return Task.FromResult(ActionContextHelper.FakeActionExecutedContext());
            };

            var attr = CreateAttribute();

            //Act
            await attr.OnActionExecutionAsync(_currentContext, _next);

            //Assert
            Assert.That(_currentContext.Result, Is.Not.InstanceOf<JsonResult>());
            Assert.That(isCallNextMiddleware, Is.False);
        }

        [Test]
        public async Task OnActionExecutionAsync_WhenIsNotUM_CallNextMiddleware()
        {
            //Arrange
            _umService.GetUnderMaintenanceInfo(_umType).Returns((false, "2022-10-10", "2022-11-10"));
            var isCallNextMiddleware = false;
            _next = () =>
            {
                isCallNextMiddleware = true;
                return Task.FromResult(ActionContextHelper.FakeActionExecutedContext());
            };

            var attr = CreateAttribute();

            //Act
            await attr.OnActionExecutionAsync(_currentContext, _next);

            //Assert
            Assert.That(_currentContext.Result, Is.Not.InstanceOf<JsonResult>());
            Assert.That(isCallNextMiddleware, Is.True);
        }

        private FunctionUMImplAttribute CreateAttribute()
            => new FunctionUMImplAttribute(_umService, _umType);
    }
}
