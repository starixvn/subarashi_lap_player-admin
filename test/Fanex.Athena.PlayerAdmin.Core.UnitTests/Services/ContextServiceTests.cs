using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Services
{
    public class ContextServiceTests
    {
        private static readonly IDictionary<string, object> _claims = new Dictionary<string, object>
        {
           { "LicenseeId", 10 },
           { "IsAllowedCustomerComm", false },
           { "IsEnableBA", false },
           { "IsEnableSingleWallet", false },
           { "IsEnabledSubsidiary", false },
           { "IsMasterLicensee", false},
           { "UserLevel", 1 },
           { "SiteDBServer", "siteDB" },
           {"OperatorID", "OperatorID"},
           {"WhiteLabelID", 0},
           {"SiteName", "SiteName"},
           {"UserSEQ", 10},
           {"UserID", "UserID"},
           {"IsShowIP", false},
           {"IsFakeLicensee", false},
           {"OfferedSubProducts", "OfferedSubProducts"},
           {"OfferedOddsGroups", "OfferedOddsGroups"},
           {"ClientIP", "127.0.0.1"},
        };

        private IHttpContextAccessor HttpContextAccessor { get; set; }
        private HttpContext HttpContext { get; set; }
        private IList<Claim> Claims { get; set; }

        [SetUp]
        public void SetUp()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContext = Substitute.For<HttpContext>();
            Claims = _claims.Select(_ => new Claim(_.Key, _.Value.ToString())).ToList();
        }

        [Test]
        public void CommonProperties_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var service = CreateService();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(service.LicenseeId, Is.EqualTo(_claims["LicenseeId"]));
                Assert.That(service.IsAllowedCustomerComm, Is.EqualTo(_claims["IsAllowedCustomerComm"]));
                Assert.That(service.IsEnableBA, Is.EqualTo(_claims["IsEnableBA"]));
                Assert.That(service.IsEnableSingleWallet, Is.EqualTo(_claims["IsEnableSingleWallet"]));
                Assert.That(service.IsEnabledSubsidiary, Is.EqualTo(_claims["IsEnabledSubsidiary"]));
                Assert.That(service.IsMasterLicensee, Is.EqualTo(_claims["IsMasterLicensee"]));
                Assert.That(service.UserLevel, Is.EqualTo(_claims["UserLevel"]));
                Assert.That(service.OperatorID, Is.EqualTo(_claims["OperatorID"]));
                Assert.That(service.SiteName, Is.EqualTo(_claims["SiteName"]));
                Assert.That(service.UserId, Is.EqualTo(_claims["UserSEQ"]));
                Assert.That(service.Username, Is.EqualTo(_claims["UserID"]));
                Assert.That(service.IsShowIP, Is.EqualTo(_claims["IsShowIP"]));
                Assert.That(service.IsFakeLicensee, Is.EqualTo(_claims["IsFakeLicensee"]));
                Assert.That(service.OwnerClientIP, Is.EqualTo(_claims["ClientIP"]));
                Assert.That(service.SiteDBServer, Is.EqualTo(_claims["SiteDBServer"]));
                Assert.IsNotEmpty(service.OfferedSubProductList);
                Assert.IsNotEmpty(service.OfferedOddsGroupList);
                Assert.That(service.Language, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            });
        }

        [Test]
        public void SiteDBServer_WhenClaimIsNull_ReturnEmptyString()
        {
            //Arrange
            RemoveClaim("SiteDBServer");
            var service = CreateService();

            //Act
            var siteDBServer = service.SiteDBServer;

            //Assert
            Assert.IsEmpty(siteDBServer);
        }

        [Test]
        public void WhiteLabelID_WhenWhiteLabelIDSmallerThanZero_ReturnNull()
        {
            //Arrange
            SetUpClaim("WhiteLabelID", -1);
            var service = CreateService();

            //Act
            var result = service.WhiteLabelID;

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void WhiteLabelID_WhenWhiteLabelIDGreaterOrEqualZero_ReturnCorrectResult()
        {
            //Arrange
            SetUpClaim("WhiteLabelID", 0);
            var service = CreateService();

            //Act
            var result = service.WhiteLabelID;

            //Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [TestCase(false, UserLevelType.Licensee)]
        [TestCase(true, UserLevelType.Subsidiary)]
        public void SubsidiaryViewType_WhenIsNotEnableSubsidiaryOrUserLevelSmallerThanSubsidiary_ReturnCorrectResult(bool isEnabledSubsidiary, int userLevel)
        {
            //Arrange
            SetUpClaim("IsEnabledSubsidiary", isEnabledSubsidiary);
            SetUpClaim("UserLevel", userLevel);
            var service = CreateService();

            //Act
            var result = service.SubsidiaryViewType;

            //Assert
            Assert.That(result, Is.EqualTo(SubsidiaryViewType.NotManagement));
        }

        [Test]
        public void SubsidiaryViewType_WhenEnableSubsidiaryAndIsMasterLicensee_ReturnMasterManagement()
        {
            //Arrange
            SetUpClaim("IsEnabledSubsidiary", true);
            SetUpClaim("UserLevel", UserLevelType.Licensee);
            SetUpClaim("IsMasterLicensee", true);
            SetUpClaim("WhiteLabelID", "-1");

            var service = CreateService();

            //Asset
            Assert.That(service.SubsidiaryViewType, Is.EqualTo(SubsidiaryViewType.MasterManagement));
        }

        [Test]
        public void SubsidiaryViewType_WhenEnableSubsidiaryAndIsNotMasterLicensee_RetunrNormalManagement()
        {
            //Arrange
            SetUpClaim("IsEnabledSubsidiary", true);
            SetUpClaim("UserLevel", UserLevelType.Licensee);
            SetUpClaim("IsMasterLicensee", false);

            var service = CreateService();

            //Asset
            Assert.That(service.SubsidiaryViewType, Is.EqualTo(SubsidiaryViewType.NormalManagement));
        }

        [Test]
        public void IsFakeLicensee_WhenNotHaveClaim_ReturnFalse()
        {
            //Arrange
            RemoveClaim("IsFakeLicensee");
            var service = CreateService();

            //Assert
            Assert.That(service.IsFakeLicensee, Is.EqualTo(false));
        }

        [Test]
        public void IsShowIpVN_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var expected = true;

            var service = CreateService();
            //HttpContext.User.WhenForAnyArgs(_ => _.IsInRole(String.Empty)).DoNotCallBase(); 
            HttpContext.User.IsInRole(String.Empty).ReturnsForAnyArgs(expected);

            //Assert
            Assert.That(service.IsShowIpVN, Is.EqualTo(expected));
        }

        [Test]
        public void OfferedSubProductList_WhenClaimIsEmpty_ReturnEmptyList()
        {
            //Arrange
            SetUpClaim("OfferedSubProducts", string.Empty);
            var service = CreateService();

            //Assert
            Assert.IsEmpty(service.OfferedSubProductList);
        }

        [Test]
        public void OfferedOddsGroupList_WhenClaimIsEmpty_ReturnEmptyList()
        {
            //Arrange
            SetUpClaim("OfferedOddsGroups", string.Empty);
            var service = CreateService();

            //Assert
            Assert.IsEmpty(service.OfferedOddsGroupList);
        }

        [Test]
        public async Task AccessToken_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var authenticationService = Substitute.For<IAuthenticationService>();
            var requestService = Substitute.For<IServiceProvider>();
            requestService.GetService<IAuthenticationService>().ReturnsForAnyArgs(authenticationService);
            HttpContext.RequestServices.Returns(requestService);

            var service = CreateService();

            var expectedResult = "test";
            var authenProperty = new AuthenticationProperties(new Dictionary<string, string> { { ".Token.access_token", expectedResult } });
            var authenTicket = new AuthenticationTicket(HttpContext.User, authenProperty, CookieAuthenticationDefaults.AuthenticationScheme);
            var authenResult = AuthenticateResult.Success(authenTicket);
            authenticationService.AuthenticateAsync(HttpContext, CookieAuthenticationDefaults.AuthenticationScheme)
                .ReturnsForAnyArgs(authenResult);

            //Act
            var result = await service.AccessToken;

            //Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IsInRoles_WhenRolesIsNull_ReturnFalse()
        {
            //Arrage
            var service = CreateService();

            //Act
            var result = service.IsInRoles(null);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CurrentRequestClientIP_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var connectionInfo = Substitute.For<ConnectionInfo>();
            connectionInfo.RemoteIpAddress.Returns(IPAddress.Broadcast);
            HttpContext.Connection.Returns(connectionInfo);
            var service = CreateService();

            //Act
            var result = service.CurrentRequestClientIP;

            //Assert
            Assert.That(result, Is.EqualTo(IPAddress.Broadcast.ToString()));
        }

        [Test]
        public void AgentList_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var agentInfo = FakeHelper.Create<CurrencyAgentInfo>();
            SetUpCurrentAgent(agentInfo);
            var service = CreateService();

            //Act
            var result = service.AgentList;
            var firstItem = result.First();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.Key, Is.EqualTo(agentInfo.CurrencyId));
                Assert.That(firstItem.Value, Is.EqualTo(agentInfo.AgentId));
            });
        }

        [Test]
        public void CurrencyList_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var agentInfo = FakeHelper.Create<CurrencyAgentInfo>();
            SetUpCurrentAgent(agentInfo);
            var service = CreateService();

            //Act
            var result = service.CurrencyList;
            var firstItem = result.First();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.Key, Is.EqualTo(agentInfo.CurrencyId));
                Assert.That(firstItem.Value, Is.EqualTo(agentInfo.Currency));
            });
        }

        private ContextService CreateService()
        {
            var claimIdentity = new ClaimsIdentity(Claims);
            var claimsPrincipal = Substitute.ForPartsOf<ClaimsPrincipal>(claimIdentity);
            HttpContextAccessor.HttpContext.Returns(HttpContext);
            HttpContext.User.Returns(claimsPrincipal);
            return new ContextService(HttpContextAccessor);
        }

        private void SetUpClaim<T>(string type, T value)
        {
            RemoveClaim(type);
            Claims.Add(new Claim(type, value.ToString()));
        }

        private void RemoveClaim(string type)
        {
            var claim = Claims.FirstOrDefault(_ => _.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase));
            if (claim != null)
            {
                Claims.Remove(claim);
            }
        }

        private void SetUpCurrentAgent(params CurrencyAgentInfo[] agentInfos)
        {
            foreach (var agent in agentInfos)
            {
                Claims.Add(new Claim("CurrencyAgent", JsonConvert.SerializeObject(agent)));
            }
        }

        private class CurrencyAgentInfo
        {
            public int[] AgentId { get; set; }
            public int CurrencyId { get; set; }
            public string Currency { get; set; }
        }
    }
}
