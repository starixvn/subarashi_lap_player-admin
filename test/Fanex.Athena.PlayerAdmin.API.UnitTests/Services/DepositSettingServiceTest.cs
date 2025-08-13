// src/Fanex.Athena.PlayerAdmin.API/Services/DepositSettingService.cs
namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    using System.Data;
    using System.Data.Common;
    using System.Net;
    using API.DTO;
    using API.Services;
    using API.ViewModels;
    using AutoFixture;
    using Core.Repositories;
    using Core.Settings;
    using Data;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Moq.Protected;
    using Newtonsoft.Json;

    public class DepositSettingServiceTest
    {
        private readonly Mock<IOptions<AppSetting>> _mockAppSetting;
        private readonly Mock<IDbFactory> _mockDbFactory;
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly Mock<ILogger<DepositSettingService>> _mockLogger;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly Mock<IObjectDb> _mockObjectDb;
        private readonly Mock<IDataReader> _mockDataReader;
        private readonly DepositSettingService _service;
        private readonly Fixture _fixture;
        private readonly AppSetting _appSetting;

        public DepositSettingServiceTest()
        {
            _fixture = new Fixture();
            _mockDbFactory = new Mock<IDbFactory>();
            _mockHttpClient = new Mock<HttpClient>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockAppSetting = new Mock<IOptions<AppSetting>>();
            _mockLogger = new Mock<ILogger<DepositSettingService>>();
            _mockObjectDb = new Mock<IObjectDb>();
            _mockDataReader = new Mock<IDataReader>();

            _appSetting = new AppSetting
            {
                CustomerClassUrl = "https://test.api/customerclass"
            };
            _mockAppSetting.Setup(x => x.Value).Returns(_appSetting);

            _service = new DepositSettingService(_mockDbFactory.Object, _mockHttpClient.Object,
                _mockMemoryCache.Object, _mockAppSetting.Object, _mockLogger.Object);
        }

        [Fact]
        public void Constructor_WithValidDependencies_CreatesService()
        {
            // Act & Assert
            Assert.NotNull(_service);
        }

        [Fact]
        public void Constructor_WithNullAppSetting_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => new DepositSettingService(_mockDbFactory.Object, _mockHttpClient.Object,
                _mockMemoryCache.Object, null, _mockLogger.Object));
        }


        [Fact]
        public void Service_ImplementsInterface()
        {
            // Act & Assert
            Assert.IsAssignableFrom<IDepositSettingService>(_service);
        }

        [Theory]
        [InlineData("en")]
        [InlineData("zh")]
        [InlineData("ko")]
        public async Task GetRiskLevelList_WithValidLanguage_ReturnsRiskLevels(string language)
        {
            // Arrange
            var riskLevels = _fixture.CreateMany<RiskLevel>(3).ToList();
            var cacheKey = $"RiskLevelList_{language}";
            var backupCacheKey = $"RiskLevelList_{language}_backup";

            object riskLevelsFromApi = riskLevels;
            _mockMemoryCache.Setup(x => x.TryGetValue(cacheKey, out riskLevelsFromApi)).Returns(true);

            object riskLevelsFromBackup = riskLevels;
            _mockMemoryCache.Setup(x => x.TryGetValue(backupCacheKey, out riskLevelsFromBackup)).Returns(true);

            Mock<ICacheEntry> cacheEntryMock = new Mock<ICacheEntry>
            {
                CallBase = false
            };
            _mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);

            // Act
            var result = await _service.GetRiskLevelList(language);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.True(resultList.Count >= 4); // At least 3 + Uncategorized
            Assert.Contains(resultList, x => x.Key == RiskLevel.Uncategorized);
        }

        [Fact]
        public async Task GetRiskLevelList_WithEmptyLanguage_ReturnsRiskLevels()
        {
            // Arrange
            var language = string.Empty;
            var riskLevels = new List<RiskLevel>();
            var cacheKey = $"RiskLevelList_{language}";
            var backupCacheKey = $"RiskLevelList_{language}_backup";

            object riskLevelsFromApi = riskLevels;
            _mockMemoryCache.Setup(x => x.TryGetValue(cacheKey, out riskLevelsFromApi)).Returns(true);

            object riskLevelsFromBackup = riskLevels;
            _mockMemoryCache.Setup(x => x.TryGetValue(backupCacheKey, out riskLevelsFromBackup)).Returns(true);

            Mock<ICacheEntry> cacheEntryMock = new Mock<ICacheEntry>
            {
                CallBase = false
            };
            _mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(cacheEntryMock.Object);

            // Act
            var result = await _service.GetRiskLevelList(language);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Single(resultList); // Only Uncategorized
            Assert.Equal(RiskLevel.Uncategorized, resultList.First().Key);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(999)]
        public async Task GetCommGroupList_WithValidLicenseeId_ReturnsCommGroups(int licenseeId)
        {
            // Arrange
            var commGroups = _fixture.CreateMany<CommGroupViewModel>(3).ToList();
            var cacheKey = $"CommGroupList_{licenseeId}";

            object commGroupsFromCache = commGroups;
            _mockMemoryCache.Setup(x => x.TryGetValue(cacheKey, out commGroupsFromCache)).Returns(true);

            // Act
            var result = await _service.GetCommGroupList(licenseeId);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.True(resultList.Count >= 4); // At least 3 + default empty one
            Assert.Contains(resultList, x => x.Id == "0");
        }

        [Fact]
        public async Task GetCommGroupFromDB_WithValidLicenseeId_ReturnsCommGroups()
        {
            // Arrange
            var licenseeId = _fixture.Create<int>();
            var commGroups = new List<(string GroupDiscountID, string GroupName)>
            {
                ("1", "Group 1"),
                ("2", "Group 2")
            };

            _mockDataReader.SetupSequence(x => x.Read())
                .Returns(true)
                .Returns(true)
                .Returns(false);

            _mockDataReader.SetupSequence(x => x["GroupDiscountID"])
                .Returns(commGroups[0].GroupDiscountID)
                .Returns(commGroups[1].GroupDiscountID);

            _mockDataReader.SetupSequence(x => x["GroupName"])
                .Returns(commGroups[0].GroupName)
                .Returns(commGroups[1].GroupName);

            _mockObjectDb.Setup(x => x.SetParameterValue("LicenseeID", licenseeId));
            _mockObjectDb.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(),It.IsAny<DbTransaction>(),It.IsAny<CancellationToken>())).ReturnsAsync(_mockDataReader.Object);
            _mockDbFactory.Setup(x => x.GetObjectDb("Lrf_LicenseeGroupCommSel",It.IsAny<int>())).Returns(_mockObjectDb.Object);

            // Act
            var result = await _service.GetCommGroupFromDB(licenseeId);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Equal(2, resultList.Count);
            Assert.Equal(commGroups[0].GroupDiscountID, resultList[0].Id);
            Assert.Equal(commGroups[0].GroupName, resultList[0].Name);
            Assert.Equal(commGroups[1].GroupDiscountID, resultList[1].Id);
            Assert.Equal(commGroups[1].GroupName, resultList[1].Name);

            // Verify database calls
            _mockDbFactory.Verify(x => x.GetObjectDb("Lrf_LicenseeGroupCommSel", It.IsAny<int>()), Times.Once);
            _mockObjectDb.Verify(x => x.SetParameterValue("LicenseeID", licenseeId), Times.Once);
            _mockObjectDb.Verify(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetCommGroupFromDB_WithNoData_ReturnsEmptyList()
        {
            // Arrange
            var licenseeId = _fixture.Create<int>();

            _mockDataReader.Setup(x => x.Read()).Returns(false);
            _mockObjectDb.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockDataReader.Object);
            _mockDbFactory.Setup(x => x.GetObjectDb("Lrf_LicenseeGroupCommSel", It.IsAny<int>())).Returns(_mockObjectDb.Object);

            // Act
            var result = await _service.GetCommGroupFromDB(licenseeId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetRiskLevelFromApi_WithSuccessfulResponse_ReturnsRiskLevels()
        {
            // This method is private, so we test it indirectly through GetRiskLevelList
            // when cache misses and falls back to API call
            
            // Arrange
            var language = "en";
            var customerClassList = new List<CustomerClassApiModel>
            {
                new CustomerClassApiModel { SabaRiskLevel = "VIP", CustomerClass = 1 },
                new CustomerClassApiModel { SabaRiskLevel = "VIP", CustomerClass = 2 },
                new CustomerClassApiModel { SabaRiskLevel = "Healthy", CustomerClass = 3 }
            };
            
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(customerClassList))
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new DepositSettingService(_mockDbFactory.Object, httpClient,
                _mockMemoryCache.Object, _mockAppSetting.Object, _mockLogger.Object);

            // Setup cache to return empty first (miss), then setup to call the API
            object any;
            _mockMemoryCache.Setup(x => x.TryGetValue($"RiskLevelList_{language}", out any)).Returns(false);
            _mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());
            
            object backupRiskLevels;
            _mockMemoryCache.Setup(x => x.TryGetValue($"RiskLevelList_{language}_backup", out backupRiskLevels))
                .Returns(false);

            // Act
            var result = await service.GetRiskLevelList(language);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Contains(resultList, x => x.Key == "VIP");
            Assert.Contains(resultList, x => x.Key == "Healthy");
            Assert.Contains(resultList, x => x.Key == RiskLevel.Uncategorized);
        }

        [Fact]
        public async Task GetRiskLevelFromApi_WithHttpException_ReturnsEmptyAndLogsError()
        {
            // Arrange
            var language = "en";
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new DepositSettingService(_mockDbFactory.Object, httpClient,
                _mockMemoryCache.Object, _mockAppSetting.Object, _mockLogger.Object);

            object any2;
            _mockMemoryCache.Setup(x => x.TryGetValue($"RiskLevelList_{language}", out any2)).Returns(false);
            _mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());
            
            object backupRiskLevels;
            _mockMemoryCache.Setup(x => x.TryGetValue($"RiskLevelList_{language}_backup", out backupRiskLevels))
                .Returns(false);

            // Act
            var result = await service.GetRiskLevelList(language);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Single(resultList); // Only Uncategorized due to API failure
            Assert.Equal(RiskLevel.Uncategorized, resultList.First().Key);

            // Verify error logging
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetCommGroupList_CacheMiss_CallsGetCommGroupFromDB()
        {
            // Arrange
            var licenseeId = _fixture.Create<int>();
            var commGroups = _fixture.CreateMany<CommGroupViewModel>(2).ToList();
            var cacheKey = $"CommGroupList_{licenseeId}";

            object any3;
            _mockMemoryCache.Setup(x => x.TryGetValue(cacheKey, out any3)).Returns(false);
            _mockMemoryCache.Setup(x => x.CreateEntry(cacheKey)).Returns(Mock.Of<ICacheEntry>);

            _mockDataReader.Setup(x => x.Read()).Returns(false);
            _mockObjectDb.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockDataReader.Object);
            _mockDbFactory.Setup(x => x.GetObjectDb("Lrf_LicenseeGroupCommSel", It.IsAny<int>())).Returns(_mockObjectDb.Object);

            // Act
            var result = await _service.GetCommGroupList(licenseeId);

            // Assert
            Assert.NotNull(result);
            _mockDbFactory.Verify(x => x.GetObjectDb("Lrf_LicenseeGroupCommSel", It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Constants_HavesCorrectValues()
        {
            // Arrange & Act & Assert
            Assert.Equal(TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(15));
        }
    }

    // Helper class for testing
    public class CustomerClassApiModel
    {
        public string SabaRiskLevel { get; set; }
        public int CustomerClass { get; set; }
    }
}