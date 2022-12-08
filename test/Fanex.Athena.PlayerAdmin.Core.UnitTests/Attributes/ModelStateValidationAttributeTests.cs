using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Net;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    public class ModelStateValidationAttributeTests
    {
        [Test]
        public void OnActionExecuting_WhenNotValidModel_SendBadRequestResponse()
        {
            //Arrange
            var context = ActionContextHelper.FakeActionExecutingContext();
            context.ModelState.AddModelError("test", "Error Test");

            var attr = new ModelStateValidationAttribute();

            //Act
            attr.OnActionExecuting(context);

            //Assert
            var result = context.Result as JsonResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }
    }
}
