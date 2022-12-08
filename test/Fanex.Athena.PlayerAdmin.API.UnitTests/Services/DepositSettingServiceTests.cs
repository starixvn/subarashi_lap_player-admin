using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.Services;
using Fanex.Athena.PlayerAdmin.API.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    public class DepositSettingServiceTests
    {
        private IDbFactory _dbFactory;
        private IMemoryCache _cache;
        private HttpMessageHandler _handler;
        private HttpClient _httpClient;
        private AppSetting _appSetting;
        private ILogger<DepositSettingService> _logger;
        private IOptions<AppSetting> _option;

        [SetUp]
        public void SetUp()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _cache= Substitute.For<IMemoryCache>();
            _handler = Substitute.For<HttpMessageHandler>();
            _httpClient = Substitute.For<HttpClient>(_handler);
            _httpClient.BaseAddress = new System.Uri("http://localhost.com");
            _logger = Substitute.For<ILogger<DepositSettingService>>();
            _option = Substitute.For<IOptions<AppSetting>>();
            _appSetting = FakeHelper.Create<AppSetting>();
            _option.Value.Returns(_appSetting);
        }

        [Test]
        public async Task GetCustomerClaasList_WhenNotLoadFromCache_ReturnCorrectList()
        {
            //Arrange
            _cache.TryGetValue(default, out var _).ReturnsForAnyArgs(false);
            _appSetting.CustomerClassUrl = "http://localhost.com";
            var customerClasses = FakeHelper.CreateMany<CustomerClassApiModel>();
            var response = CreateResponseMsg(customerClasses);
            _handler.SubstituteProtected("SendAsync", Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(response));
            
            var service = CreateService();

            //Act
            var result = await service.GetCustomerClassList();

            //Assert
            Assert.IsTrue(result.Count() > 1);
        }

        [Test]
        public async Task GetCustomerClassList_WhenApiHaveException_ReturnCorrectList()
        {
            //Arrange
            _cache.TryGetValue(default, out var _).ReturnsForAnyArgs(false);
            _appSetting.CustomerClassUrl = "http://localhost.com";
            var customerClasses = FakeHelper.CreateMany<CustomerClassApiModel>();
            var response = CreateResponseMsg(customerClasses);
            response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            _handler.SubstituteProtected("SendAsync", Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(response));

            var service = CreateService();

            //Act
            var result = await service.GetCustomerClassList();

            //Assert
            Assert.IsTrue(result.Count() == 1);
        }

        [Test]
        public async Task GetCommGroupList_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            _cache.TryGetValue(default, out var _).ReturnsForAnyArgs(arg =>
            {
                arg[1] = Enumerable.Empty<CommGroupViewModel>();
                return true;
            });
            var service = CreateService();

            //Act
            var result = await service.GetCommGroupList(1);

            //Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetCommGroupFromDB_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var licenseeId = FakeHelper.Create<int>();
            var obejectDb = Substitute.For<IObjectDb>();
            var reader = Substitute.For<IDataReader>();
            reader.Read().Returns(true, false);
            obejectDb.ExecuteReaderAsync().Returns(reader);
            _dbFactory.GetObjectDb("Lrf_LicenseeGroupCommSel").Returns(obejectDb);
            var comGrouGroup = FakeHelper.Create<CommGroupViewModel>();
            SetUpReader(reader, comGrouGroup);

            var service = CreateService();

            //Act
            var result = await service.GetCommGroupFromDB(licenseeId);
            var firstItem = result.First();

            //Assert
            Assert.Multiple(() =>
            {
                obejectDb.Received(1).SetParameterValue("LicenseeID", licenseeId);
                Assert.AreEqual(comGrouGroup.Id, firstItem.Id);
                Assert.AreEqual(comGrouGroup.Name, firstItem.Name);
            });
        }

        private DepositSettingService CreateService()
            => new DepositSettingService(_dbFactory, _httpClient, _cache, _option, _logger);

        private HttpResponseMessage CreateResponseMsg<T>(T contentObject)
        {
            var response = new HttpResponseMessage();
            response.StatusCode = System.Net.HttpStatusCode.OK;
            var content = new StringContent(JsonConvert.SerializeObject(contentObject));
            response.Content = content;
            return response;
        }

        private void SetUpReader(IDataReader reader, CommGroupViewModel model)
        {
            reader["GroupCommId"].Returns(model.Id);
            reader["GroupName"].Returns(model.Name);
        }
    }
}
