namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using Constants;
    using Core.Services;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;

    public class ContextServiceTest
    {
        private readonly ContextService _contextService;
        private readonly Mock<IAuthenticationService> _mockAuthService;
        private readonly Mock<ConnectionInfo> _mockConnection;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly Mock<ClaimsPrincipal> _mockUser;

        public ContextServiceTest()
        {
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockHttpContext = new Mock<HttpContext>();
            _mockUser = new Mock<ClaimsPrincipal>();
            _mockConnection = new Mock<ConnectionInfo>();
            _mockServiceProvider = new Mock<IServiceProvider>();
            _mockAuthService = new Mock<IAuthenticationService>();

            _mockHttpContext.Setup(x => x.User).Returns(_mockUser.Object);
            _mockHttpContext.Setup(x => x.Connection).Returns(_mockConnection.Object);
            _mockHttpContext.Setup(x => x.RequestServices).Returns(_mockServiceProvider.Object);
            _mockServiceProvider.Setup(x => x.GetService(typeof(IAuthenticationService))).Returns(_mockAuthService.Object);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(_mockHttpContext.Object);

            _contextService = new ContextService(_mockHttpContextAccessor.Object);
        }

        [Fact]
        public void LicenseeId_ReturnsClaimValue()
        {
            // Arrange
            var claim = new Claim("LicenseeId", "123");
            _mockUser.Setup(x => x.FindFirst("LicenseeId")).Returns(claim);

            // Act
            var result = _contextService.LicenseeId;

            // Assert
            Assert.Equal(123, result);
        }

        [Fact]
        public void SiteName_ReturnsClaimValue()
        {
            // Arrange
            var claim = new Claim("SiteName", "TestSite");
            _mockUser.Setup(x => x.FindFirst("SiteName")).Returns(claim);

            // Act
            var result = _contextService.SiteName;

            // Assert
            Assert.Equal("TestSite", result);
        }

        [Fact]
        public void UserId_ReturnsClaimValue()
        {
            // Arrange
            var claim = new Claim("UserSEQ", "456");
            _mockUser.Setup(x => x.FindFirst("UserSEQ")).Returns(claim);

            // Act
            var result = _contextService.UserId;

            // Assert
            Assert.Equal(456, result);
        }

        [Fact]
        public void Username_ReturnsClaimValue()
        {
            // Arrange
            var claim = new Claim("UserID", "testuser");
            _mockUser.Setup(x => x.FindFirst("UserID")).Returns(claim);

            // Act
            var result = _contextService.Username;

            // Assert
            Assert.Equal("testuser", result);
        }

        [Fact]
        public void IsAllowedCustomerComm_ReturnsClaimValue()
        {
            // Arrange
            var claim = new Claim("IsAllowedCustomerComm", "true");
            _mockUser.Setup(x => x.FindFirst("IsAllowedCustomerComm")).Returns(claim);

            // Act
            var result = _contextService.IsAllowedCustomerComm;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsEnableSingleWallet_ReturnsClaimValue()
        {
            // Arrange
            var claim = new Claim("IsEnableSingleWallet", "false");
            _mockUser.Setup(x => x.FindFirst("IsEnableSingleWallet")).Returns(claim);

            // Act
            var result = _contextService.IsEnableSingleWallet;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsEnabledSubsidiary_ReturnsClaimValue()
        {
            // Arrange
            var claim = new Claim("IsEnabledSubsidiary", "true");
            _mockUser.Setup(x => x.FindFirst("IsEnabledSubsidiary")).Returns(claim);

            // Act
            var result = _contextService.IsEnabledSubsidiary;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsMasterLicensee_ReturnsClaimValue()
        {
            // Arrange
            var claim = new Claim("IsMasterLicensee", "true");
            _mockUser.Setup(x => x.FindFirst("IsMasterLicensee")).Returns(claim);

            // Act
            var result = _contextService.IsMasterLicensee;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UserLevel_ReturnsClaimValue()
        {
            // Arrange
            var claim = new Claim("UserLevel", "5");
            _mockUser.Setup(x => x.FindFirst("UserLevel")).Returns(claim);

            // Act
            var result = _contextService.UserLevel;

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void SiteDBServer_ReturnsClaimValue_WhenExists()
        {
            // Arrange
            var claim = new Claim("SiteDBServer", "TestDBServer");
            _mockUser.Setup(x => x.FindFirst("SiteDBServer")).Returns(claim);

            // Act
            var result = _contextService.SiteDBServer;

            // Assert
            Assert.Equal("TestDBServer", result);
        }

        [Fact]
        public void SiteDBServer_ReturnsEmptyString_WhenNotExists()
        {
            // Arrange
            _mockUser.Setup(x => x.FindFirst("SiteDBServer")).Returns((Claim)null);

            // Act
            var result = _contextService.SiteDBServer;

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void OperatorID_ReturnsClaimValue()
        {
            // Arrange
            var claim = new Claim("OperatorID", "OP123");
            _mockUser.Setup(x => x.FindFirst("OperatorID")).Returns(claim);

            // Act
            var result = _contextService.OperatorID;

            // Assert
            Assert.Equal("OP123", result);
        }

        [Fact]
        public void WhiteLabelID_ReturnsValue_WhenPositive()
        {
            // Arrange
            var claim = new Claim("WhiteLabelID", "10");
            _mockUser.Setup(x => x.FindFirst("WhiteLabelID")).Returns(claim);

            // Act
            var result = _contextService.WhiteLabelID;

            // Assert
            Assert.Equal(10, result);
        }

        [Fact]
        public void WhiteLabelID_ReturnsNull_WhenNegative()
        {
            // Arrange
            var claim = new Claim("WhiteLabelID", "-1");
            _mockUser.Setup(x => x.FindFirst("WhiteLabelID")).Returns(claim);

            // Act
            var result = _contextService.WhiteLabelID;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Language_ReturnsCurrentUICultureName()
        {
            // Arrange
            var originalCulture = CultureInfo.CurrentUICulture;
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");

            try
            {
                // Act
                var result = _contextService.Language;

                // Assert
                Assert.Equal("en-US", result);
            }
            finally
            {
                CultureInfo.CurrentUICulture = originalCulture;
            }
        }

        [Fact]
        public void IsShowIP_ReturnsClaimValue()
        {
            // Arrange
            var claim = new Claim("IsShowIP", "true");
            _mockUser.Setup(x => x.FindFirst("IsShowIP")).Returns(claim);

            // Act
            var result = _contextService.IsShowIP;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsShowIpVN_ReturnsTrue_WhenInSuperAdminRole()
        {
            // Arrange
            _mockUser.Setup(x => x.IsInRole("super_admin")).Returns(true);

            // Act
            var result = _contextService.IsShowIpVN;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsShowIpVN_ReturnsTrue_WhenInBlViewIpvnRole()
        {
            // Arrange
            _mockUser.Setup(x => x.IsInRole("super_admin")).Returns(false);
            _mockUser.Setup(x => x.IsInRole("bl_view_ipvn")).Returns(true);

            // Act
            var result = _contextService.IsShowIpVN;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsShowIpVN_ReturnsFalse_WhenNotInRequiredRoles()
        {
            // Arrange
            _mockUser.Setup(x => x.IsInRole("super_admin")).Returns(false);
            _mockUser.Setup(x => x.IsInRole("bl_view_ipvn")).Returns(false);

            // Act
            var result = _contextService.IsShowIpVN;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsShowSabaIntelligence_ReturnsTrue_WhenInRequiredRoles()
        {
            // Arrange
            _mockUser.Setup(x => x.IsInRole("super_admin")).Returns(true);

            // Act
            var result = _contextService.IsShowSabaIntelligence;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSearchWithMatchPreciseOnly_ReturnsTrue_WhenInPreciseRoleButNotSuperAdmin()
        {
            // Arrange
            _mockUser.Setup(x => x.IsInRole("SearchWithMatchPrecise_PlayerAdminOnly")).Returns(true);
            _mockUser.Setup(x => x.IsInRole("super_admin")).Returns(false);

            // Act
            var result = _contextService.IsSearchWithMatchPreciseOnly;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSearchWithMatchPreciseOnly_ReturnsFalse_WhenInSuperAdminRole()
        {
            // Arrange
            _mockUser.Setup(x => x.IsInRole("SearchWithMatchPrecise_PlayerAdminOnly")).Returns(true);
            _mockUser.Setup(x => x.IsInRole("super_admin")).Returns(true);

            // Act
            var result = _contextService.IsSearchWithMatchPreciseOnly;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsShowSabaCoins_ReturnsTrue_WhenInRequiredRoles()
        {
            // Arrange
            _mockUser.Setup(x => x.IsInRole("SabaCoins")).Returns(true);

            // Act
            var result = _contextService.IsShowSabaCoins;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsFakeLicensee_ReturnsClaimValue()
        {
            // Arrange
            var claim = new Claim("IsFakeLicensee", "true");
            _mockUser.Setup(x => x.FindFirst("IsFakeLicensee")).Returns(claim);

            // Act
            var result = _contextService.IsFakeLicensee;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsFakeLicensee_ReturnsFalse_WhenClaimNotExists()
        {
            // Arrange
            _mockUser.Setup(x => x.FindFirst("IsFakeLicensee")).Returns((Claim)null);

            // Act
            var result = _contextService.IsFakeLicensee;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SpecialPlatform_ReturnsClaimValue()
        {
            // Arrange
            var claim = new Claim("SpecialPlatform", "2");
            _mockUser.Setup(x => x.FindFirst("SpecialPlatform")).Returns(claim);

            // Act
            var result = _contextService.SpecialPlatform;

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void SpecialPlatform_ReturnsZero_WhenClaimNotExists()
        {
            // Arrange
            _mockUser.Setup(x => x.FindFirst("SpecialPlatform")).Returns((Claim)null);

            // Act
            var result = _contextService.SpecialPlatform;

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void OfferedSubProductList_ReturnsEmptyEnumerable_WhenClaimIsEmpty()
        {
            // Arrange
            _mockUser.Setup(x => x.FindFirst("OfferedSubProducts")).Returns((Claim)null);

            // Act
            var result = _contextService.OfferedSubProductList;

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void OfferedSubProductList_ReturnsSplitValues_WhenClaimExists()
        {
            // Arrange
            var claim = new Claim("OfferedSubProducts", "SB,CS,LC");
            _mockUser.Setup(x => x.FindFirst("OfferedSubProducts")).Returns(claim);

            // Act
            var result = _contextService.OfferedSubProductList;

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Contains("SB", result);
            Assert.Contains("CS", result);
            Assert.Contains("LC", result);
        }

        [Fact]
        public void OfferedOddsGroupList_ReturnsEmptyEnumerable_WhenClaimIsEmpty()
        {
            // Arrange
            _mockUser.Setup(x => x.FindFirst("OfferedOddsGroups")).Returns((Claim)null);

            // Act
            var result = _contextService.OfferedOddsGroupList;

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void OfferedOddsGroupList_ReturnsSplitValues_WhenClaimExists()
        {
            // Arrange
            var claim = new Claim("OfferedOddsGroups", "Group1,Group2,Group3");
            _mockUser.Setup(x => x.FindFirst("OfferedOddsGroups")).Returns(claim);

            // Act
            var result = _contextService.OfferedOddsGroupList;

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Contains("Group1", result);
            Assert.Contains("Group2", result);
            Assert.Contains("Group3", result);
        }

        // AccessToken test removed - cannot mock extension methods with Moq
        // The AccessToken property uses Context.GetTokenAsync() which is an extension method

        [Fact]
        public void IsInRoles_ReturnsTrue_WhenUserIsInAnyRole()
        {
            // Arrange
            _mockUser.Setup(x => x.IsInRole("role1")).Returns(false);
            _mockUser.Setup(x => x.IsInRole("role2")).Returns(true);
            _mockUser.Setup(x => x.IsInRole("role3")).Returns(false);

            // Act
            var result = _contextService.IsInRoles("role1", "role2", "role3");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsInRoles_ReturnsFalse_WhenUserIsNotInAnyRole()
        {
            // Arrange
            _mockUser.Setup(x => x.IsInRole("role1")).Returns(false);
            _mockUser.Setup(x => x.IsInRole("role2")).Returns(false);
            _mockUser.Setup(x => x.IsInRole("role3")).Returns(false);

            // Act
            var result = _contextService.IsInRoles("role1", "role2", "role3");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CurrentRequestClientIP_ReturnsRemoteIpAddress()
        {
            // Arrange
            var ipAddress = IPAddress.Parse("192.168.1.1");
            _mockConnection.Setup(x => x.RemoteIpAddress).Returns(ipAddress);

            // Act
            var result = _contextService.CurrentRequestClientIP;

            // Assert
            Assert.Equal("192.168.1.1", result);
        }

        [Fact]
        public void OwnerClientIP_ReturnsClaimValue()
        {
            // Arrange
            var claim = new Claim("ClientIP", "10.0.0.1");
            _mockUser.Setup(x => x.FindFirst("ClientIP")).Returns(claim);

            // Act
            var result = _contextService.OwnerClientIP;

            // Assert
            Assert.Equal("10.0.0.1", result);
        }

        [Fact]
        public void Constructor_WithValidHttpContextAccessor_InitializesCorrectly()
        {
            // Arrange & Act
            var service = new ContextService(_mockHttpContextAccessor.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_WithNullHttpContextAccessor_DoesNotThrowException()
        {
            // The ContextService constructor doesn't validate null parameters
            // Act & Assert
            var service = new ContextService(null);
            Assert.NotNull(service);
        }

        [Fact]
        public void SubsidiaryViewType_ReturnsMasterManagement_WhenMasterLicenseeWithoutWhiteLabel()
        {
            // Arrange
            _mockUser.Setup(x => x.FindFirst("IsEnabledSubsidiary")).Returns(new Claim("IsEnabledSubsidiary", "true"));
            _mockUser.Setup(x => x.FindFirst("UserLevel")).Returns(new Claim("UserLevel", "1"));
            _mockUser.Setup(x => x.FindFirst("IsMasterLicensee")).Returns(new Claim("IsMasterLicensee", "true"));
            _mockUser.Setup(x => x.FindFirst("WhiteLabelID")).Returns(new Claim("WhiteLabelID", "-1"));

            // Act
            var result = _contextService.SubsidiaryViewType;

            // Assert
            Assert.Equal(SubsidiaryViewType.MasterManagement, result);
        }

        [Fact]
        public void SubsidiaryViewType_ReturnsNormalManagement_WhenEnabledSubsidiaryButNotMaster()
        {
            // Arrange
            _mockUser.Setup(x => x.FindFirst("IsEnabledSubsidiary")).Returns(new Claim("IsEnabledSubsidiary", "true"));
            _mockUser.Setup(x => x.FindFirst("UserLevel")).Returns(new Claim("UserLevel", "1"));
            _mockUser.Setup(x => x.FindFirst("IsMasterLicensee")).Returns(new Claim("IsMasterLicensee", "false"));

            // Act
            var result = _contextService.SubsidiaryViewType;

            // Assert
            Assert.Equal(SubsidiaryViewType.NormalManagement, result);
        }

        [Fact]
        public void SubsidiaryViewType_ReturnsNotManagement_WhenNotEnabledSubsidiary()
        {
            // Arrange
            _mockUser.Setup(x => x.FindFirst("IsEnabledSubsidiary")).Returns(new Claim("IsEnabledSubsidiary", "false"));
            _mockUser.Setup(x => x.FindFirst("UserLevel")).Returns(new Claim("UserLevel", "1"));

            // Act
            var result = _contextService.SubsidiaryViewType;

            // Assert
            Assert.Equal(SubsidiaryViewType.NotManagement, result);
        }

        [Fact]
        public void SubsidiaryViewType_ReturnsNotManagement_WhenUserLevelIsSubsidiaryOrHigher()
        {
            // Arrange
            _mockUser.Setup(x => x.FindFirst("IsEnabledSubsidiary")).Returns(new Claim("IsEnabledSubsidiary", "true"));
            _mockUser.Setup(x => x.FindFirst("UserLevel")).Returns(new Claim("UserLevel", "10"));

            // Act
            var result = _contextService.SubsidiaryViewType;

            // Assert
            Assert.Equal(SubsidiaryViewType.NotManagement, result);
        }

        [Fact]
        public void AgentList_ReturnsReadOnlyDictionary_FromCurrencyAgentClaims()
        {
            // Arrange
            var currencyAgent1 = new { CurrencyId = 1, AgentId = new[] { 100, 101 }, Currency = "USD" };
            var currencyAgent2 = new { CurrencyId = 2, AgentId = new[] { 200, 201 }, Currency = "EUR" };

            var claims = new List<Claim>
            {
                new("CurrencyAgent", JsonConvert.SerializeObject(currencyAgent1)),
                new("CurrencyAgent", JsonConvert.SerializeObject(currencyAgent2))
            };
            _mockUser.Setup(x => x.Claims).Returns(claims);

            // Act
            var result = _contextService.AgentList;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.True(result.ContainsKey(1));
            Assert.True(result.ContainsKey(2));
            Assert.Equal(new[] { 100, 101 }, result[1]);
            Assert.Equal(new[] { 200, 201 }, result[2]);
        }

        [Fact]
        public void CurrencyList_ReturnsReadOnlyDictionary_FromCurrencyAgentClaims()
        {
            // Arrange
            var currencyAgent1 = new { CurrencyId = 1, AgentId = new[] { 100, 101 }, Currency = "USD" };
            var currencyAgent2 = new { CurrencyId = 2, AgentId = new[] { 200, 201 }, Currency = "EUR" };

            var claims = new List<Claim>
            {
                new("CurrencyAgent", JsonConvert.SerializeObject(currencyAgent1)),
                new("CurrencyAgent", JsonConvert.SerializeObject(currencyAgent2))
            };
            _mockUser.Setup(x => x.Claims).Returns(claims);

            // Act
            var result = _contextService.CurrencyList;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.True(result.ContainsKey(1));
            Assert.True(result.ContainsKey(2));
            Assert.Equal("USD", result[1]);
            Assert.Equal("EUR", result[2]);
        }

        [Fact]
        public void AgentList_CachesBetweenCalls()
        {
            // Arrange
            var currencyAgent = new { CurrencyId = 1, AgentId = new[] { 100 }, Currency = "USD" };
            var claims = new List<Claim>
            {
                new("CurrencyAgent", JsonConvert.SerializeObject(currencyAgent))
            };
            _mockUser.Setup(x => x.Claims).Returns(claims);

            // Act
            var result1 = _contextService.AgentList;
            var result2 = _contextService.AgentList;

            // Assert
            Assert.Same(result1, result2); // Should be the same instance due to caching
        }

        [Fact]
        public void CurrencyList_CachesBetweenCalls()
        {
            // Arrange
            var currencyAgent = new { CurrencyId = 1, AgentId = new[] { 100 }, Currency = "USD" };
            var claims = new List<Claim>
            {
                new("CurrencyAgent", JsonConvert.SerializeObject(currencyAgent))
            };
            _mockUser.Setup(x => x.Claims).Returns(claims);

            // Act
            var result1 = _contextService.CurrencyList;
            var result2 = _contextService.CurrencyList;

            // Assert
            Assert.Same(result1, result2); // Should be the same instance due to caching
        }
    }
}