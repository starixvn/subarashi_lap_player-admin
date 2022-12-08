using Fanex.Athena.PlayerAdmin.Core.Attributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    public class AuthorizeAttributeTests
    {
        [Test]
        public void CookieAuthorizeAttribute_WhenInit_ReturnCorrectScheme()
        {
            //Arrange
            var attr = new CookieAuthorizeAttribute("role");

            //Assert
            Assert.That(attr.AuthenticationSchemes, Is.EqualTo("Cookies"));
        }

        [Test]
        public void BearerAuthorizeAttribute_WhenInit_ReturnCorrectScheme()
        {
            //Arrange
            var attr = new BearerAuthorizeAttribute("role");

            //Assert
            Assert.That(attr.AuthenticationSchemes, Is.EqualTo("Bearer"));
        }
    }
}
