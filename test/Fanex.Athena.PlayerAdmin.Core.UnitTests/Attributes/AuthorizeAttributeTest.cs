namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    using Core.Attributes;

    public class AuthorizeAttributeTest
    {
        [Fact]
        public void CookieAuthorizeAttribute_WithNoRoles_SetsAuthenticationSchemeOnly()
        {
            // Arrange & Action
            var attribute = new CookieAuthorizeAttribute();

            // Assert
            Assert.Equal("Cookies", attribute.AuthenticationSchemes);
            Assert.Null(attribute.Roles);
        }

        [Fact]
        public void CookieAuthorizeAttribute_WithSingleRole_SetsRolesAndAuthenticationScheme()
        {
            // Arrange & Action
            var attribute = new CookieAuthorizeAttribute("Admin");

            // Assert
            Assert.Equal("Cookies", attribute.AuthenticationSchemes);
            Assert.Equal("Admin", attribute.Roles);
        }

        [Fact]
        public void CookieAuthorizeAttribute_WithMultipleRoles_SetsCommaSeparatedRoles()
        {
            // Arrange & Action
            var attribute = new CookieAuthorizeAttribute("Admin", "User", "Manager");

            // Assert
            Assert.Equal("Cookies", attribute.AuthenticationSchemes);
            Assert.Equal("Admin,User,Manager", attribute.Roles);
        }

        [Fact]
        public void BearerAuthorizeAttribute_WithNoRoles_SetsAuthenticationSchemeOnly()
        {
            // Arrange & Action
            var attribute = new BearerAuthorizeAttribute();

            // Assert
            Assert.Equal("Bearer", attribute.AuthenticationSchemes);
            Assert.Null(attribute.Roles);
        }

        [Fact]
        public void BearerAuthorizeAttribute_WithSingleRole_SetsRolesAndAuthenticationScheme()
        {
            // Arrange & Action
            var attribute = new BearerAuthorizeAttribute("Admin");

            // Assert
            Assert.Equal("Bearer", attribute.AuthenticationSchemes);
            Assert.Equal("Admin", attribute.Roles);
        }

        [Fact]
        public void BearerAuthorizeAttribute_WithMultipleRoles_SetsCommaSeparatedRoles()
        {
            // Arrange & Action
            var attribute = new BearerAuthorizeAttribute("Admin", "User", "Manager");

            // Assert
            Assert.Equal("Bearer", attribute.AuthenticationSchemes);
            Assert.Equal("Admin,User,Manager", attribute.Roles);
        }

        [Fact]
        public void CookieAuthorizeAttribute_WithEmptyRoles_SetsAuthenticationSchemeOnly()
        {
            // Arrange & Action
            var attribute = new CookieAuthorizeAttribute();

            // Assert
            Assert.Equal("Cookies", attribute.AuthenticationSchemes);
            Assert.Null(attribute.Roles);
        }

        [Fact]
        public void BearerAuthorizeAttribute_WithEmptyRoles_SetsAuthenticationSchemeOnly()
        {
            // Arrange & Action
            var attribute = new BearerAuthorizeAttribute();

            // Assert
            Assert.Equal("Bearer", attribute.AuthenticationSchemes);
            Assert.Null(attribute.Roles);
        }
    }
}