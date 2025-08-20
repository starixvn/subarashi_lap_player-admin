// test/Fanex.Athena.PlayerAdmin.API.UnitTests/Services/PlayerListServiceTests.cs
namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    using System.Data;
    using System.Data.Common;
    using API.DTO;
    using API.MetaModel;
    using API.Services;
    using API.Services.Interfaces;
    using AutoFixture;
    using Core.Repositories;
    using Core.Services;
    using Data;
    using Moq;

    public class PlayerListServiceTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IDbFactory> _mockDbFactory;
        private readonly Mock<IDepositSettingService> _mockDepositSettingService;
        private readonly Mock<IObjectDb> _mockObjectDb;
        private readonly Mock<IDataReader> _mockDataReader;
        private readonly PlayerListService _service;

        public PlayerListServiceTest()
        {
            _fixture = new Fixture();
            _mockDbFactory = new Mock<IDbFactory>();
            _mockContextService = new Mock<IContextService>();
            _mockDepositSettingService = new Mock<IDepositSettingService>();
            _mockObjectDb = new Mock<IObjectDb>();
            _mockDataReader = new Mock<IDataReader>();
            _service = new PlayerListService(_mockDbFactory.Object, _mockContextService.Object, _mockDepositSettingService.Object);

            // Setup default behavior
            _mockDbFactory.Setup(f => f.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(_mockObjectDb.Object);
            _mockObjectDb.Setup(db => db.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockDataReader.Object);
            _mockObjectDb.Setup(db => db.GetParameterValue("TotalRows")).Returns(0);
        }

        [Fact]
        public void Constructor_WithValidDbFactory_CreatesService()
        {
            // Act & Assert
            Assert.NotNull(_service);
        }

        [Fact]
        public void Constructor_WithNullDependencies_DoesNotThrowException()
        {
            // The service constructor doesn't validate null parameters
            // Act & Assert
            var service = new PlayerListService(null, null, null);
            Assert.NotNull(service);
        }

        [Fact]
        public void Service_ImplementsInterface()
        {
            // Act & Assert
            Assert.IsAssignableFrom<IPlayerListService>(_service);
        }

        [Fact]
        public void Service_HasCorrectDependencies()
        {
            // Act & Assert
            Assert.NotNull(_mockDbFactory);
            Assert.NotNull(_mockContextService);
            Assert.NotNull(_mockDepositSettingService);
            Assert.NotNull(_service);
        }

        [Fact]
        public async Task Search_WithDepositedIsZero_ReturnsEmptyResult()
        {
            // Arrange
            var metaModel = _fixture.Build<PlayerSearchingMetaModel>()
                .With(m => m.IsDeposited, 0)
                .Create();

            // Act
            var result = await _service.Search(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.DataList);
            Assert.Equal(0, result.TotalItem);
        }

        [Fact]
        public async Task Search_WithValidMetaModel_ReturnsPlayerList()
        {
            // Arrange
            var metaModel = _fixture.Build<PlayerSearchingMetaModel>()
                .With(m => m.IsDeposited, 1)
                .Create();

            var riskLevels = CreateRiskLevels();
            var players = CreatePlayerDataReaderMock();

            _mockDepositSettingService.Setup(s => s.GetRiskLevelList(It.IsAny<string>()))
                .ReturnsAsync(riskLevels);

            _mockDataReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(false);

            SetupPlayerDataReader(_mockDataReader);

            _mockObjectDb.Setup(db => db.GetParameterValue("TotalRows")).Returns(1);

            // Act
            var result = await _service.Search(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.DataList);
            Assert.Equal(1, result.TotalItem);
            _mockDepositSettingService.Verify(s => s.GetRiskLevelList(_mockContextService.Object.Language), Times.Once);
        }

        [Fact]
        public async Task Search_SetsCorrectStoredProcedureParameters()
        {
            // Arrange
            var metaModel = _fixture.Build<PlayerSearchingMetaModel>()
                .With(m => m.IsDeposited, 1)
                .With(m => m.UserName, "testuser")
                .With(m => m.SearchOption, (byte)1)
                .With(m => m.SpecialLevel, "2")
                .With(m => m.OddsGroup, "A")
                .With(m => m.PageSize, 10)
                .With(m => m.PageIndex, 1)
                .Create();

            var riskLevels = CreateRiskLevels();
            _mockDepositSettingService.Setup(s => s.GetRiskLevelList(It.IsAny<string>()))
                .ReturnsAsync(riskLevels);

            _mockDataReader.Setup(r => r.Read()).Returns(false);

            // Act
            await _service.Search(metaModel);

            // Assert
            _mockObjectDb.Verify(db => db.SetParameterValue("UserName", "testuser"), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("SearchOption", (byte)1), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("SpecialLevel", "2"), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("OddsGroups", "A"), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("PageSize", 10), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("PageIndex", 1), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("TotalRows", 0), Times.Once);
        }

        [Fact]
        public async Task GetPlayerListById_WithValidMetaModel_ReturnsPlayerList()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerListByIdMetaModel>();
            var riskLevels = CreateRiskLevels();

            _mockDepositSettingService.Setup(s => s.GetRiskLevelList(It.IsAny<string>()))
                .ReturnsAsync(riskLevels);

            _mockDataReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(false);

            SetupPlayerDataReader(_mockDataReader);
            _mockObjectDb.Setup(db => db.GetParameterValue("TotalRows")).Returns(1);

            // Act
            var result = await _service.GetPlayerListById(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.DataList);
            Assert.Equal(1, result.TotalItem);
        }

        [Fact]
        public async Task GetPlayerListById_SetsCorrectStoredProcedureParameters()
        {
            // Arrange
            var metaModel = _fixture.Build<PlayerListByIdMetaModel>()
                .With(m => m.IdList, "1,2,3")
                .With(m => m.SpecialLevel, "5")
                .With(m => m.OddsGroup, "B")
                .With(m => m.PageSize, 20)
                .With(m => m.PageIndex, 2)
                .Create();

            var riskLevels = CreateRiskLevels();
            _mockDepositSettingService.Setup(s => s.GetRiskLevelList(It.IsAny<string>()))
                .ReturnsAsync(riskLevels);

            _mockDataReader.Setup(r => r.Read()).Returns(false);

            // Act
            await _service.GetPlayerListById(metaModel);

            // Assert
            _mockObjectDb.Verify(db => db.SetParameterValue("CustIds", "1,2,3"), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("SpecialLevel", "5"), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("OddsGroups", "B"), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("PageSize", 20), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("PageIndex", 2), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("TotalRows", 0), Times.Once);
        }

        [Fact]
        public async Task ArchivedSearch_WithValidMetaModel_ReturnsPlayerList()
        {
            // Arrange
            var metaModel = _fixture.Build<PlayerArchivedSearchingMetaModel>()
                .With(m => m.UserName, "testuser")
                .With(m => m.SearchOption, (byte)1)
                .With(m => m.PageSize, 10)
                .With(m => m.PageIndex, 1)
                .Create();

            var currencyList = new Dictionary<int, string> { { 1, "USD" }, { 2, "EUR" } };
            _mockContextService.SetupGet(c => c.CurrencyList).Returns(currencyList);

            _mockDataReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(false);

            SetupArchivedPlayerDataReader(_mockDataReader);
            _mockObjectDb.Setup(db => db.GetParameterValue("TotalRows")).Returns(1);

            // Act
            var result = await _service.ArchivedSearch(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.DataList);
            Assert.Equal(1, result.TotalItem);
            var player = result.DataList.First();
            Assert.Equal(3, player.Status); // Archived status
        }

        [Fact]
        public async Task ArchivedSearch_SetsCorrectStoredProcedureParameters()
        {
            // Arrange
            var metaModel = _fixture.Build<PlayerArchivedSearchingMetaModel>()
                .With(m => m.AgentListXml, "<agents>test</agents>")
                .With(m => m.UserName, "testuser_search")
                .With(m => m.SearchOption, (byte)2)
                .With(m => m.PageSize, 25)
                .With(m => m.PageIndex, 3)
                .Create();

            _mockContextService.SetupGet(c => c.CurrencyList).Returns(new Dictionary<int, string>());
            _mockDataReader.Setup(r => r.Read()).Returns(false);

            // Act
            await _service.ArchivedSearch(metaModel);

            // Assert
            _mockObjectDb.Verify(db => db.SetParameterValue("AgentList", "<agents>test</agents>"), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("UserName", "testuser[_]search"), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("SearchOption", (byte)2), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("BaseCurrency", string.Empty), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("PageSize", 25), Times.Once);
            _mockObjectDb.Verify(db => db.SetParameterValue("PageIndex", 3), Times.Once);
        }

        [Fact]
        public async Task ArchivedSearch_WithEmptyUserName_DoesNotSetUserNameParameter()
        {
            // Arrange
            var metaModel = _fixture.Build<PlayerArchivedSearchingMetaModel>()
                .With(m => m.UserName, string.Empty)
                .Create();

            _mockContextService.SetupGet(c => c.CurrencyList).Returns(new Dictionary<int, string>());
            _mockDataReader.Setup(r => r.Read()).Returns(false);

            // Act
            await _service.ArchivedSearch(metaModel);

            // Assert
            _mockObjectDb.Verify(db => db.SetParameterValue("UserName", It.IsAny<string>()), Times.Never);
            _mockObjectDb.Verify(db => db.SetParameterValue("SearchOption", It.IsAny<byte>()), Times.Never);
        }

        [Theory]
        [InlineData("test_user", "test[_]user")]
        [InlineData("user_name_test", "user[_]name[_]test")]
        [InlineData("normal", "normal")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public async Task Search_WithUnderscoreInUserName_EscapesCorrectly(string input, string expected)
        {
            // Arrange
            var metaModel = _fixture.Build<PlayerSearchingMetaModel>()
                .With(m => m.IsDeposited, 1)
                .With(m => m.UserName, input)
                .Create();

            var riskLevels = CreateRiskLevels();
            _mockDepositSettingService.Setup(s => s.GetRiskLevelList(It.IsAny<string>()))
                .ReturnsAsync(riskLevels);

            _mockDataReader.Setup(r => r.Read()).Returns(false);

            // Act
            await _service.Search(metaModel);

            // Assert
            if (expected != null)
            {
                _mockObjectDb.Verify(db => db.SetParameterValue("UserName", expected), Times.Once);
            }
        }

        [Fact]
        public async Task Search_WithMultiplePlayersAndRiskLevels_MapsCorrectRiskLevels()
        {
            // Arrange
            var metaModel = _fixture.Build<PlayerSearchingMetaModel>()
                .With(m => m.IsDeposited, 1)
                .Create();

            var riskLevels = new List<RiskLevel>
            {
                new RiskLevel { Key = "High", CustomerClassList = new List<string> { "1", "2" } },
                new RiskLevel { Key = "Medium", CustomerClassList = new List<string> { "3" } }
            };

            _mockDepositSettingService.Setup(s => s.GetRiskLevelList(It.IsAny<string>()))
                .ReturnsAsync(riskLevels);

            _mockDataReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(true)
                .Returns(false);

            SetupPlayerDataReader(_mockDataReader);

            _mockDataReader.SetupSequence(r => r["CustomerClass"])
                .Returns("1")
                .Returns("3");
            _mockObjectDb.Setup(db => db.GetParameterValue("TotalRows")).Returns(2);

            // Act
            var result = await _service.Search(metaModel);

            // Assert
            Assert.Equal(2, result.DataList.Count());
            var firstPlayer = result.DataList.First();
            var secondPlayer = result.DataList.Skip(1).First();
            Assert.Equal("High", firstPlayer.RiskLevelKey);
            Assert.Equal("Medium", secondPlayer.RiskLevelKey);
        }

        [Fact]
        public async Task Search_WithUncategorizedRiskLevel_AssignsUncategorizedKey()
        {
            // Arrange
            var metaModel = _fixture.Build<PlayerSearchingMetaModel>()
                .With(m => m.IsDeposited, 1)
                .Create();

            var riskLevels = CreateRiskLevels();
            _mockDepositSettingService.Setup(s => s.GetRiskLevelList(It.IsAny<string>()))
                .ReturnsAsync(riskLevels);

            _mockDataReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(false);

            SetupPlayerDataReader(_mockDataReader);
            _mockDataReader.Setup(r => r["CustomerClass"]).Returns("999"); // Unknown customer class

            _mockObjectDb.Setup(db => db.GetParameterValue("TotalRows")).Returns(1);

            // Act
            var result = await _service.Search(metaModel);

            // Assert
            var player = result.DataList.First();
            Assert.Equal(RiskLevel.Uncategorized, player.RiskLevelKey);
        }

        [Fact]
        public async Task ArchivedSearch_WithUnknownCurrency_UsesCurrencyFromDictionary()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerArchivedSearchingMetaModel>();
            var currencyList = new Dictionary<int, string> { { 1, "USD" }, { 2, "EUR" } };
            _mockContextService.SetupGet(c => c.CurrencyList).Returns(currencyList);

            _mockDataReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(false);

            SetupArchivedPlayerDataReader(_mockDataReader);
            _mockDataReader.Setup(r => r["currency"]).Returns(1);

            // Act
            var result = await _service.ArchivedSearch(metaModel);

            // Assert
            var player = result.DataList.First();
            Assert.Equal("USD", player.Currency);
        }

        [Fact]
        public async Task ArchivedSearch_WithUnknownCurrencyId_UsesEmptyString()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerArchivedSearchingMetaModel>();
            var currencyList = new Dictionary<int, string> { { 1, "USD" } };
            _mockContextService.SetupGet(c => c.CurrencyList).Returns(currencyList);

            _mockDataReader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(false);

            SetupArchivedPlayerDataReader(_mockDataReader);
            _mockDataReader.Setup(r => r["currency"]).Returns(999); // Unknown currency

            // Act
            var result = await _service.ArchivedSearch(metaModel);

            // Assert
            var player = result.DataList.First();
            Assert.Equal(string.Empty, player.Currency);
        }

        [Fact]
        public async Task Search_DisposesObjectDbCorrectly()
        {
            // Arrange
            var metaModel = _fixture.Build<PlayerSearchingMetaModel>()
                .With(m => m.IsDeposited, 1)
                .Create();

            var riskLevels = CreateRiskLevels();
            _mockDepositSettingService.Setup(s => s.GetRiskLevelList(It.IsAny<string>()))
                .ReturnsAsync(riskLevels);

            _mockDataReader.Setup(r => r.Read()).Returns(false);

            // Act
            await _service.Search(metaModel);

            // Assert
            _mockObjectDb.Verify(db => db.Dispose(), Times.Once);
        }

        private IEnumerable<RiskLevel> CreateRiskLevels()
        {
            return new List<RiskLevel>
            {
                new RiskLevel
                {
                    Key = "Low",
                    CustomerClassList = new List<string> { "1" }
                },
                new RiskLevel
                {
                    Key = "High",
                    CustomerClassList = new List<string> { "2", "3" }
                }
            };
        }

        private void SetupPlayerDataReader(Mock<IDataReader> mockReader)
        {
            mockReader.Setup(r => r["CustId"]).Returns(123);
            mockReader.Setup(r => r["ExchangeId"]).Returns(1);
            mockReader.Setup(r => r["Currency"]).Returns("USD");
            mockReader.Setup(r => r["UserName"]).Returns("username");
            mockReader.Setup(r => r["NickName"]).Returns("nickname");
            mockReader.Setup(r => r["SpecialLevel"]).Returns(1);
            mockReader.Setup(r => r["OddsGroup"]).Returns("a");
            mockReader.Setup(r => r["TotalBalance"]).Returns(1000.50m);
            mockReader.Setup(r => r["Outstanding"]).Returns(50.25m);
            mockReader.Setup(r => r["GroupName"]).Returns("GroupA");
            mockReader.Setup(r => r["CustomerClass"]).Returns("1");
        }

        private void SetupArchivedPlayerDataReader(Mock<IDataReader> mockReader)
        {
            mockReader.Setup(r => r["custid"]).Returns(456);
            mockReader.Setup(r => r["username"]).Returns("archiveduser");
            mockReader.Setup(r => r["nickname"]).Returns("archivednick");
            mockReader.Setup(r => r["balance"]).Returns(750.75m);
            mockReader.Setup(r => r["currency"]).Returns(1);
        }

        private IEnumerable<Player> CreatePlayerDataReaderMock()
        {
            return new List<Player>
            {
                new Player
                {
                    IbcCustId = 123,
                    CurrencyId = 1,
                    Currency = "USD",
                    SystemId = "username",
                    UserName = "nickname",
                    SpecialLevel = 1,
                    OddsGroup = "A",
                    CurrentBalance = 1000.50m,
                    OutstandingBalance = 50.25m,
                    CommissionGroup = "GroupA",
                    CustomerClass = "1",
                    RiskLevelKey = "Low"
                }
            };
        }
    }
}