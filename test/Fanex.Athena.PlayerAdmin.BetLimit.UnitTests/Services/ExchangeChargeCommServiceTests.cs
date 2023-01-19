
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Mediator.Events;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services;
using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Services
{
    public class ExchangeChargeCommServiceTests
    {
        private const int ApiProcessingErrorCode = 102;
        private const int SuccessStatus = 1;

        private ILgApiClient _lgApiClient;
        private IPublisher _publisher;
        private AppSetting _appSetting;
        private IOptions<AppSetting> _options;

        [SetUp]
        public void SetUp()
        {
            _lgApiClient = Substitute.For<ILgApiClient>();
            _publisher = Substitute.For<IPublisher>();
            _appSetting = FakeHelper.Create<AppSetting>();
            _options = Substitute.For<IOptions<AppSetting>>();
            _options.Value.Returns(_appSetting);
        }

        [Test]
        public async Task GetChargeCommission_WhenDataFromApiIsNull_ReturnErrorResult()
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var lang = FakeHelper.Create<string>();
            var apiResult = FakeHelper.Create<ApiData<ExchangeCommissionData>>();
            apiResult.Data = null;
            _lgApiClient.PostAsync<ApiData<ExchangeCommissionData>>(default, default, default).ReturnsForAnyArgs(apiResult);

            var service = CreateService();

            //Act
            var result = await service.GetChargeCommission(custId, lang);

            //Assert
            Assert.AreEqual(ApiProcessingErrorCode, result.Code);
            Assert.AreEqual(ViewResources.ErrorProcessingMsg, result.Message);
        }

        [Test]
        public async Task GetChargeCommission_WhenDataFromApiStatusIsSuccess_ReturnCorrectResult()
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var lang = FakeHelper.Create<string>();
            var apiResult = FakeHelper.Create<ApiData<ExchangeCommissionData>>();
            apiResult.Data.Status = SuccessStatus;
            _lgApiClient.PostAsync<ApiData<ExchangeCommissionData>>(default, default, default).ReturnsForAnyArgs(apiResult);

            var service = CreateService();

            //Act
            var result = await service.GetChargeCommission(custId, lang);

            //Assert
            Assert.AreEqual(0, result.Code);
            Assert.AreEqual(ViewResources.UpdateSuccess, result.Message);
            Assert.AreEqual(apiResult.Data.Commission, result.SpecificExtendedData);
        }

        [Test]
        public async Task GetChargeCommission_WhenDataFromApiStatusIsError_ReturnErrResult()
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var lang = FakeHelper.Create<string>();
            var apiResult = FakeHelper.Create<ApiData<ExchangeCommissionData>>();
            apiResult.Data.Status = 100;
            _lgApiClient.PostAsync<ApiData<ExchangeCommissionData>>(default, default, default).ReturnsForAnyArgs(apiResult);

            var service = CreateService();

            //Act
            var result = await service.GetChargeCommission(custId, lang);

            //Assert
            Assert.AreEqual(100, result.Code);
            Assert.AreEqual(ViewResources.ErrorProcessingMsg, result.Message);
        }

        [Test]
        public async Task UpdateChargeCommission_WhenDataFromApiIsNull_ReturnErrorResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerUpdateChargeCommMetaModel>();
            var apiResult = FakeHelper.Create<ApiData<ExchangeCommissionData>>();
            apiResult.Data = null;
            _lgApiClient.PostAsync<ApiData<ExchangeCommissionData>>(default, default, default).ReturnsForAnyArgs(apiResult);

            var service = CreateService();

            //Act
            var result = await service.UpdateChargeCommission(metaModel);

            //Assert
            Assert.AreEqual(ApiProcessingErrorCode, result.Code);
            Assert.AreEqual(ViewResources.ErrorProcessingMsg, result.Message);
            await _publisher.DidNotReceiveWithAnyArgs().Publish(default);
        }

        [Test]
        public async Task UpdateChargeCommission_WhenDataFromApiStatusIsSuccess_ReturnSuccessResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerUpdateChargeCommMetaModel>();
            var apiResult = FakeHelper.Create<ApiData<ExchangeCommissionData>>();
            apiResult.Data.Status = 1;
            _lgApiClient.PostAsync<ApiData<ExchangeCommissionData>>(default, default, default).ReturnsForAnyArgs(apiResult);

            var service = CreateService();

            //Act
            var result = await service.UpdateChargeCommission(metaModel) as IResultInfo<double?>;

            //Assert
            Assert.AreEqual(0, result.Code);
            Assert.AreEqual(ViewResources.UpdateSuccess, result.Message);
            Assert.AreEqual(apiResult.Data.Commission, result.SpecificExtendedData);
            await _publisher.Received(1).Publish(Arg.Any<PlayerUpdatedChargeCommEvent>());
        }

        [Test]
        public async Task UpdateChargeCommission_WhenDataFromApiStatusIsError_ReturnErrResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerUpdateChargeCommMetaModel>();
            var apiResult = FakeHelper.Create<ApiData<ExchangeCommissionData>>();
            apiResult.Data.Status = 100;
            _lgApiClient.PostAsync<ApiData<ExchangeCommissionData>>(default, default, default).ReturnsForAnyArgs(apiResult);

            var service = CreateService();

            //Act
            var result = await service.UpdateChargeCommission(metaModel);

            //Assert
            Assert.AreEqual(100, result.Code);
            Assert.AreEqual(ViewResources.ErrorProcessingMsg, result.Message);
            await _publisher.DidNotReceiveWithAnyArgs().Publish(default);
        }

        private ExchangeChargeCommService CreateService()
            => new ExchangeChargeCommService(_lgApiClient, _publisher, _options);
    }
}
