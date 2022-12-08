using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Core.Utils;
using Microsoft.AspNetCore.Mvc.Filters;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Attributes
{
    public class AutoBindingMetaModelAttributeTests
    {
        private IMetaModelBindingService _bindingService;
        private ILicenseeService _licenseeService;
        private ISubsidiaryService _subsidiaryService;
        private IContextService _contextService;

        private ActionExecutingContext _currentContext;
        private ActionExecutionDelegate _next;

        [SetUp]
        public void SetUp()
        {
            _bindingService = Substitute.For<IMetaModelBindingService>();
            _licenseeService = Substitute.For<ILicenseeService>();
            _subsidiaryService = Substitute.For<ISubsidiaryService>();
            _contextService = Substitute.For<IContextService>();

            _currentContext = ActionContextHelper.FakeActionExecutingContext();
            _next = () => Task.FromResult(ActionContextHelper.FakeActionExecutedContext());
        }

        [Test]
        public void AutoBindingMetaModelAttribute_WhenInit_ArgumentsSizeCorrectly()
        {
            //Arrange
            var attr = new AutoBindingMetaModelAttribute();

            //Assert
            Assert.That(attr.Arguments.Count(), Is.EqualTo(9));
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task OnActionExecutionAsync_WhenNeedBindAgent_CallRelevantMethod(bool isIgnoreSubsidiarySearch)
        {
            //Arrange
            IReadOnlyDictionary<int, int[]> defaultAgentList = new Dictionary<int, int[]>();
            var agentList = new Dictionary<int, int[]>();
            var expectedAgentList = isIgnoreSubsidiarySearch ? defaultAgentList as IDictionary<int, int[]> : agentList;
            var subsidiary = Substitute.For<ISubsidiarySearchMetaModel>();

            BindItemToContextArgument(subsidiary);
            _contextService.AgentList.Returns(defaultAgentList);
            _subsidiaryService.GetAgentList(_contextService.SiteName, _contextService.AgentList, subsidiary.SubsidiaryList)
                .Returns(agentList);
            _subsidiaryService.IsIgnoredSubsidiarySearch(default,default,default)
                .ReturnsForAnyArgs(isIgnoreSubsidiarySearch);

            var attr = CreateAttribute(bindAgent: true);

            //Act
            await attr.OnActionExecutionAsync(_currentContext, _next);

            //Assert
            _bindingService.Received(1).BindAgentXml(_currentContext.ActionArguments, expectedAgentList);
        }

        [Test]
        public async Task OnActionExecutionAsync_WhenNeedBindSiteName_BindSiteName()
        {
            //Arrange
            var expected = "test";
            var bindedItem = Substitute.For<ISiteNameMetaModel>();
            BindItemToContextArgument(bindedItem);
            _contextService.SiteName.Returns(expected);

            var attr = CreateAttribute(bindSiteName: true);

            //Act
            await attr.OnActionExecutionAsync(_currentContext, _next);

            //Assert
            Assert.That(bindedItem.SiteName, Is.EqualTo(expected));
        }

        [Test]
        public async Task OnActionExecutionAsync_WhenNeedBindLanguage_BindLanguage()
        {
            //Arrange
            var expected = "test";
            var bindedItem = Substitute.For<ILanguageMetaModel>();
            BindItemToContextArgument(bindedItem);
            _contextService.Language.Returns(expected);

            var attr = CreateAttribute(bindLanguage: true);

            //Act
            await attr.OnActionExecutionAsync(_currentContext, _next);

            //Assert
            Assert.That(bindedItem.Language, Is.EqualTo(expected));
        }

        [Test]
        public async Task OnActionExecutionAsync_WhenNeedBindClientIp_BindClientIp()
        {
            //Arrange
            var expected = FakeHelper.Create<string>();
            var bindedItem = Substitute.For<IClientIPMetaModel>();
            BindItemToContextArgument(bindedItem);
            _contextService.CurrentRequestClientIP.Returns(expected);

            var attr = CreateAttribute(bindClientIP: true);

            //Act
            await attr.OnActionExecutionAsync(_currentContext, _next);

            //Assert
            Assert.That(bindedItem.Ip, Is.EqualTo(expected));
        }

        [Test]
        public async Task OnActionExecutionAsync_WhenNeedBindUserId_BindUserId()
        {
            //Arrange
            var expected = FakeHelper.Create<int>();
            var bindedItem = Substitute.For<IUserIdMetaModel>();
            BindItemToContextArgument(bindedItem);
            _contextService.UserId.Returns(expected);

            var attr = CreateAttribute(bindUserId: true);

            //Act
            await attr.OnActionExecutionAsync(_currentContext, _next);

            //Assert
            Assert.That(bindedItem.UserId, Is.EqualTo(expected));
        }

        [Test]
        public async Task OnActionExecutionAsync_WhenNeedBindLicenseeId_BindLicenseeId()
        {
            //Arrange
            var expected = FakeHelper.Create<int>();
            var bindedItem = Substitute.For<ILicenseeIdMetaModel>();
            BindItemToContextArgument(bindedItem);
            _contextService.LicenseeId.Returns(expected);

            var attr = CreateAttribute(bindLicenseeId: true);

            //Act
            await attr.OnActionExecutionAsync(_currentContext, _next);

            //Assert
            Assert.That(bindedItem.LicenseeId, Is.EqualTo(expected));
        }

        [Test]
        public async Task OnActionExecutionAsync_WhenNeedBindUsername_BindUsername()
        {
            //Arrange
            var expected = FakeHelper.Create<string>();
            var bindedItem = Substitute.For<IUsernameMetaModel>();
            BindItemToContextArgument(bindedItem);
            _contextService.Username.Returns(expected);

            var attr = CreateAttribute(bindUsername: true);

            //Act
            await attr.OnActionExecutionAsync(_currentContext, _next);

            //Assert
            Assert.That(bindedItem.Username, Is.EqualTo(expected));
        }

        [Test]
        public async Task OnActionExecutionAsync_WhenNeedBindBalanceUp_BindBalanceUp()
        {
            //Arrange
            var expected = FakeHelper.Create<BalanceUpData>();
            var bindedItem = Substitute.For<IBalanceUpAndMoveBetMetaModel>();
            BindItemToContextArgument(bindedItem);
            _licenseeService.GetBalanceUp().Returns(expected);

            var attr = CreateAttribute(bindBalanceUp: true);

            //Act
            await attr.OnActionExecutionAsync(_currentContext, _next);

            //Assert
            Assert.That(bindedItem.BalanceUpDay, Is.EqualTo(expected.BalanceUpDay));
            Assert.That(bindedItem.MoveBetsDay, Is.EqualTo(expected.MoveBetsDay));
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task OnActionExecutionAsync_WhenNeedBindProductListAndHaveFilter_BindProductListFromFilter(bool isBindFunctionKey)
        {
            //Arrange
            var expected = FakeHelper.Create<string>();
            var productFromFilter = FakeHelper.Create<string>();
            var bindedProductList = Substitute.For<IProductCodeListMetaModel>();
            bindedProductList.ProductCodeList = productFromFilter;
            if(isBindFunctionKey)
            {
                var bindedFunction = Substitute.For<IFunctionKeyMetaModel>();
                BindItemToContextArgument(bindedFunction);
            }
            BindItemToContextArgument(bindedProductList);
            _contextService.Username.Returns(expected);

            var attr = CreateAttribute(bindProductList: true);

            //Act
            await attr.OnActionExecutionAsync(_currentContext, _next);

            //Assert
            Assert.That(bindedProductList.OriginalProductCodeList, Is.EqualTo(productFromFilter));
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task OnActionExecutionAsync_WhenNeedBindProductListAndHaveNoProductFilter_BindProductListFromDB(bool isBindFunctionKey)
        {
            //Arrange
            var expected = FakeHelper.Create<string>();
            var productAccess = FakeHelper.Create<ProductAccess>();
            var bindedProductList = Substitute.For<IProductCodeListMetaModel>();
            bindedProductList.ProductCodeList = String.Empty;
            if (isBindFunctionKey)
            {
                var bindedFunction = Substitute.For<IFunctionKeyMetaModel>();
                BindItemToContextArgument(bindedFunction);
            }
            BindItemToContextArgument(bindedProductList);
            _contextService.Username.Returns(expected);
            _licenseeService.GetSingleWalletProductAccess(default, default).ReturnsForAnyArgs(productAccess);

            var attr = CreateAttribute(bindProductList: true);

            //Act
            await attr.OnActionExecutionAsync(_currentContext, _next);

            //Assert
            Assert.That(bindedProductList.OriginalProductCodeList, Is.EqualTo(productAccess.ProductCodeListString));
        }


        private AutoBindingMetaModelImplAttribute CreateAttribute(
            bool bindAgent = false,
            bool bindSiteName = false,
            bool bindLanguage = false,
            bool bindClientIP = false,
            bool bindUserId = false,
            bool bindBalanceUp = false,
            bool bindLicenseeId = false,
            bool bindUsername = false,
            bool bindProductList = false
            )
            => new AutoBindingMetaModelImplAttribute(
                _bindingService,
                _licenseeService,
                _subsidiaryService,
                _contextService,
                bindAgent,
                bindSiteName,
                bindLanguage,
                bindClientIP,
                bindUserId,
                bindBalanceUp,
                bindLicenseeId,
                bindUsername,
                bindProductList);

        private void BindItemToContextArgument<T>(T bindItem)
            => _currentContext.BindItemToContextArgument(bindItem);
    }
}
