
using Fanex.Athena.PlayerAdmin.BetLimit.Controllers;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Controllers
{
    public class ExchangeDcsBetLimitControllerTests
    {
        private IDcsBetLimitService _dcsService;
        private IExchangeChargeCommService _chargeCommService;
        private IContextService _contextService;
        private IResourceService _resourceService;

        [SetUp]
        public void SetUp()
        {
            _dcsService = Substitute.For<IDcsBetLimitService>();
            _chargeCommService = Substitute.For<IExchangeChargeCommService>();
            _contextService= Substitute.For<IContextService>();
            _resourceService= Substitute.For<IResourceService>();
        }

        [TestCase(0)]
        [TestCase(1)]
        public async Task GetSetting_WhenCall_ReturnCorrectValue(int code)
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitSelectMetaModel>();
            var expectedCommResult = new ResultInfo<double?> { Code= code, SpecificExtendedData = 100 };
            _chargeCommService.GetChargeCommission(metaModel.CustId, _contextService.Language).Returns(expectedCommResult);
            _dcsService.GetStatusAsync(metaModel.CustId, ProductTypes.Exchange).Returns(true);
            var expectedCommission = expectedCommResult.IsSuccessful ? expectedCommResult.SpecificExtendedData : null;

            var controller = CreateController();

            //Act
            var result = await controller.GetSetting(metaModel);
            var data = result.Value as PlayerChargeCommViewModel;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(data);
                Assert.AreEqual(expectedCommission, data.Commission);
                Assert.IsTrue(data.StatusEnable);
            });
        }

        [TestCase(0, "")]
        [TestCase(1, "Error")]
        public async Task UpdateStatus_WhenCall_ReturnCorrectResult(int code, string msg)
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var expectedResult = new ResultInfo { Code= code, Message = msg };
            var exptedMsg = expectedResult.IsSuccessful ? ViewResources.UpdateSuccess : expectedResult.Message;
            _dcsService.UpdateStatusAsync(metaModel).Returns(expectedResult);

            var controller = CreateController();
            
            //Act
            var result = await controller.UpdateStatus(metaModel);
            var data = result.Value as IResultInfo;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(data);
                Assert.AreEqual(ProductTypes.Exchange, metaModel.ProductId);
                Assert.AreEqual(exptedMsg, data.Message);
            });
        }

        [Test]
        public async Task UpdateChargeComm_WhenNotValidComm_ReturnInvaidChargeCommResult()
        {
            //Arrange
            const int InvalidChargeCommissionErrorCode = 100;
            const int invaldComm = 1000;
            var metaModel = FakeHelper.Create<PlayerUpdateChargeCommMetaModel>();
            metaModel.Comm = invaldComm;

            var controller = CreateController();

            //Act
            var result = await controller.UpdateChargeComm(metaModel);
            var data = result.Value as ResultInfo;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(data);
                Assert.AreEqual(InvalidChargeCommissionErrorCode, data.Code);
                Assert.AreEqual(ViewResources.InvalidChargeCommission, data.Message);
            });
        }

        [Test]
        public async Task UpdateChargeComm_WhenNotHavePer_ReturnFunctionDeniedResult()
        {
            //Arrange
            const int validComm = 2;
            var metaModel = FakeHelper.Create<PlayerUpdateChargeCommMetaModel>();
            metaModel.Comm = validComm;
            _dcsService.GetStatusAsync(metaModel.CustId, ProductTypes.Exchange).Returns(false);

            var controller = CreateController();

            //Act
            var result = await controller.UpdateChargeComm(metaModel);
            var data = result.Value as ResultInfo;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(data);
                Assert.IsFalse(data.IsSuccessful);
                Assert.AreEqual(ViewResources.FunctionDeniedMsg, data.Message);
            });
        }

        [Test]
        public async Task UpdateChargeComm_WhenHavePer_ReturnCorrectResult()
        {
            //Arrange
            const int validComm = 2;
            var metaModel = FakeHelper.Create<PlayerUpdateChargeCommMetaModel>();
            var expectedResult = FakeHelper.Create<ResultInfo>();
            metaModel.Comm = validComm;
            _dcsService.GetStatusAsync(metaModel.CustId, ProductTypes.Exchange).Returns(true);
            _chargeCommService.UpdateChargeCommission(metaModel).Returns(expectedResult);

            var controller = CreateController();

            //Act
            var result = await controller.UpdateChargeComm(metaModel);
            var data = result.Value as ResultInfo;

            //Assert
            Assert.AreSame(expectedResult, data);
        }

        private ExchangeDcsBetLimitController CreateController()
            => new ExchangeDcsBetLimitController(_contextService, _resourceService, _dcsService, _chargeCommService);
    }
}
