using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;
using Newtonsoft.Json;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Services
{
    public class ContextServiceTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly ContextService _contextService;

        public ContextServiceTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
            _contextService = new ContextService(_httpContextAccessorMock.Object);
        }

        [Fact]
        public void LicenseeId_ClaimExists_ReturnsClaimValue()
        {
            // Arrange
            var expectedLicenseeId = 123;
            var claims = new[] { new Claim("LicenseeId", expectedLicenseeId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _httpContextAccessorMock.Object.HttpContext.User = claimsPrincipal;

            // Act
            var result = _contextService.LicenseeId;

            // Assert
            Assert.Equal(expectedLicenseeId, result);
        }

        [Fact]
        public void SiteName_ClaimExists_ReturnsClaimValue()
        {
            // Arrange
            var expectedSiteName = "TestSite";
            var claims = new[] { new Claim("SiteName", expectedSiteName) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _httpContextAccessorMock.Object.HttpContext.User = claimsPrincipal;

            // Act
            var result = _contextService.SiteName;

            // Assert
            Assert.Equal(expectedSiteName, result);
        }

        [Fact]
        public void UserId_ClaimExists_ReturnsClaimValue()
        {
            // Arrange
            var expectedUserId = 456;
            var claims = new[] { new Claim("UserSEQ", expectedUserId.ToString()) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _httpContextAccessorMock.Object.HttpContext.User = claimsPrincipal;

            // Act
            var result = _contextService.UserId;

            // Assert
            Assert.Equal(expectedUserId, result);
        }

        [Theory]
        [InlineData("super_admin", true)]
        [InlineData("another_role", false)]
        public void IsInRoles_VariousInputs_ReturnsExpectedResult(string role, bool expected)
        {
            // Arrange
            var claims = new[] { new Claim(ClaimTypes.Role, "super_admin") };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _httpContextAccessorMock.Object.HttpContext.User = claimsPrincipal;

            // Act
            var result = _contextService.IsInRoles(role);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AgentList_ClaimExists_ReturnsAgentList()
        {
            // Arrange
            var currencyAgentInfo = new { AgentId = new[] { 1, 2 }, CurrencyId = 1, Currency = "USD" };
            var claims = new[] { new Claim("CurrencyAgent", JsonConvert.SerializeObject(currencyAgentInfo)) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _httpContextAccessorMock.Object.HttpContext.User = claimsPrincipal;

            // Act
            var result = _contextService.AgentList;

            // Assert
            Assert.Single(result);
            Assert.Equal(currencyAgentInfo.AgentId, result[1]);
        }

        [Fact]
        public void CurrencyList_ClaimExists_ReturnsCurrencyList()
        {
            // Arrange
            var currencyAgentInfo = new { AgentId = new[] { 1, 2 }, CurrencyId = 1, Currency = "USD" };
            var claims = new[] { new Claim("CurrencyAgent", JsonConvert.SerializeObject(currencyAgentInfo)) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _httpContextAccessorMock.Object.HttpContext.User = claimsPrincipal;

            // Act
            var result = _contextService.CurrencyList;

            // Assert
            Assert.Single(result);
            Assert.Equal(currencyAgentInfo.Currency, result[1]);
        }
    }
} 