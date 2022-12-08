using Fanex.Athena.PlayerAdmin.API.Controllers;
using Fanex.Athena.PlayerAdmin.API.Services;
using Fanex.Athena.PlayerAdmin.API.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    public class SettingControllerTests
    {
        private IContextService _contextService;
        private ILicenseeService _licenseeService;
        private IDepositSettingService _depositSettingService;
        private ISubsidiaryService _subsidiaryService;

        [SetUp]
        public void Setup()
        {
            _contextService = Substitute.For<IContextService>();
            _licenseeService = Substitute.For<ILicenseeService>();
            _depositSettingService = Substitute.For<IDepositSettingService>();
            _subsidiaryService = Substitute.For<ISubsidiaryService>();
        }

        [Test]
        public async Task GetSiteSetting_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            _licenseeService.CheckProductBelongsTo(default, default).ReturnsForAnyArgs(false);
            var controller = CreateController();

            //Act
            var result = await controller.GetSiteSetting(_licenseeService);
            var value = result.Value as SiteSettingViewModel;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(_contextService.IsAllowedCustomerComm, value.IsAllowedCustomerComm);
                Assert.IsFalse(value.IsAllowedBA);
                Assert.AreEqual(_contextService.UserLevel, value.UserLevel);
                Assert.AreEqual(_contextService.IsEnableSingleWallet, value.IsEnableSingleWallet);
            });
        }

        [Test]
        public async Task GetDepositSetting_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var expectedCommGroups = FakeHelper.CreateMany<CommGroupViewModel>();
            var expectedCustomerClass = FakeHelper.CreateMany<string>();
            var expectedOddGrouds = FakeHelper.CreateMany<string>();

            _depositSettingService.GetCommGroupList(_contextService.LicenseeId).Returns(expectedCommGroups);
            _depositSettingService.GetCustomerClassList().Returns(expectedCustomerClass);
            _contextService.OfferedOddsGroupList.Returns(expectedOddGrouds);

            var controller = CreateController();

            //Act
            var result = await controller.GetDepositSetting(_depositSettingService) as OkObjectResult;
            var value = result.Value as DepositSettingViewModel;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedCommGroups, value.CommGroups);
                Assert.AreEqual(expectedCustomerClass, value.CustomerClass);
                Assert.AreEqual(expectedOddGrouds, value.OddsGroups);
            });
        }

        [Test]
        public async Task GetSubsidiarySetting_WhenEmptySubsidiaryList_ReturnCorrectResult()
        {
            //Arrange
            var expectedSubsidiaryList = Enumerable.Empty<SelectTreeItem>();
            _subsidiaryService.GetSubsidiaryList(default, default, default, default)
                .ReturnsForAnyArgs(expectedSubsidiaryList);

            var controller = CreateController();

            //Act
            var result = await controller.GetSubsidiarySetting(_subsidiaryService) as OkObjectResult;
            var value = result.Value;

            //Assert
            Assert.AreEqual(SubsidiaryViewType.NotManagement, value.GetPropertyValue<byte>("SubsidiaryViewType"));
            Assert.AreEqual(expectedSubsidiaryList, value.GetPropertyValue<IEnumerable<SelectTreeItem>>("SubsidiaryList"));
        }

        [Test]
        public async Task GetSubsidiarySetting_WhenNotEmptySubsidiaryList_ReturnCorrectResult()
        {
            //Arrange
            var expectedSubsidiaryList = new List<SelectTreeItem> { new SelectTreeItem() };
            var expectedSubsidiaryViewType = FakeHelper.Create<byte>();
            _subsidiaryService.GetSubsidiaryList(default, default, default, default)
                .ReturnsForAnyArgs(expectedSubsidiaryList);
            _contextService.SubsidiaryViewType.Returns(expectedSubsidiaryViewType);

            var controller = CreateController();

            //Act
            var result = await controller.GetSubsidiarySetting(_subsidiaryService) as OkObjectResult;
            var value = result.Value;

            //Assert
            Assert.AreEqual(expectedSubsidiaryViewType, value.GetPropertyValue<byte>("SubsidiaryViewType"));
            Assert.AreEqual(expectedSubsidiaryList, value.GetPropertyValue<IEnumerable<SelectTreeItem>>("SubsidiaryList"));
        }

        private SettingController CreateController()
            => new SettingController(_contextService);



    }
}
