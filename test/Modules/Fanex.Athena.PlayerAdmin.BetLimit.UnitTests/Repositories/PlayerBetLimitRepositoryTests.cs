namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Repositories
{
    using System.Data;
    using System.Data.Common;
    using BetLimit.Repositories;
    using Core.Repositories;
    using Data;
    using Moq;

    public class PlayerBetLimitRepositoryTests
    {
        private readonly Mock<IDbFactory> _dbFactory;
        private readonly Mock<IObjectDb> _db;
        private readonly Mock<IDataReader> _reader;
        private readonly PlayerBetLimitRepository _repository;

        public PlayerBetLimitRepositoryTests()
        {
            _dbFactory = new Mock<IDbFactory>();
            _db = new Mock<IObjectDb>();
            _reader = new Mock<IDataReader>();
            _dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(_db.Object);
            _repository = new PlayerBetLimitRepository(_dbFactory.Object);
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_CallsCorrectStoredProcedure()
        {
            _reader.Setup(r => r.Read()).Returns(false);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            await _repository.GetEnableBetLimitProducts("TestSite", "PROD");

            _dbFactory.Verify(x => x.GetObjectDb("Lrf_LicenseeProductByBetLimitSel", It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_SetsCorrectParameters_ForLocalEnvironment()
        {
            _reader.Setup(r => r.Read()).Returns(false);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            await _repository.GetEnableBetLimitProducts("LocalSite", "LOCAL");

            _db.Verify(x => x.SetParameterValue("LicenseeName", "LocalSite"), Times.Once);
            _db.Verify(x => x.SetParameterValue("SettingStatus", 1), Times.Once); // LOCAL is in the list
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_SetsCorrectParameters_ForSitEnvironment()
        {
            _reader.Setup(r => r.Read()).Returns(false);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            await _repository.GetEnableBetLimitProducts("SitSite", "SIT");

            _db.Verify(x => x.SetParameterValue("LicenseeName", "SitSite"), Times.Once);
            _db.Verify(x => x.SetParameterValue("SettingStatus", 1), Times.Once); // SIT is in the list
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_SetsCorrectParameters_ForUatEnvironment()
        {
            _reader.Setup(r => r.Read()).Returns(false);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            await _repository.GetEnableBetLimitProducts("UatSite", "UAT");

            _db.Verify(x => x.SetParameterValue("LicenseeName", "UatSite"), Times.Once);
            _db.Verify(x => x.SetParameterValue("SettingStatus", 1), Times.Once); // UAT is in the list
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_SetsCorrectParameters_ForProdEnvironment()
        {
            _reader.Setup(r => r.Read()).Returns(false);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            await _repository.GetEnableBetLimitProducts("ProdSite", "PROD");

            _db.Verify(x => x.SetParameterValue("LicenseeName", "ProdSite"), Times.Once);
            _db.Verify(x => x.SetParameterValue("SettingStatus", 0), Times.Once); // PROD is not in the list
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_SetsCorrectParameters_ForUnknownEnvironment()
        {
            _reader.Setup(r => r.Read()).Returns(false);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            await _repository.GetEnableBetLimitProducts("TestSite", "UNKNOWN");

            _db.Verify(x => x.SetParameterValue("LicenseeName", "TestSite"), Times.Once);
            _db.Verify(x => x.SetParameterValue("SettingStatus", 0), Times.Once); // UNKNOWN is not in the list
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_ReturnsEmptyList_WhenNoData()
        {
            _reader.Setup(r => r.Read()).Returns(false);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            var result = await _repository.GetEnableBetLimitProducts("TestSite", "PROD");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_ReturnsEnabledProducts()
        {
            _reader.SetupSequence(r => r.Read()).Returns(true).Returns(true).Returns(false);
            _reader.SetupSequence(r => r["IsEnable"]).Returns(true).Returns(true);
            _reader.SetupSequence(r => r["ProductID"]).Returns(1).Returns(2);
            _reader.SetupSequence(r => r["ProductCode"]).Returns("PROD1").Returns("PROD2");
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            var result = await _repository.GetEnableBetLimitProducts("TestSite", "PROD");

            Assert.Equal(2, result.Count());
            var products = result.ToList();
            Assert.Equal(1, products[0].ProductId);
            Assert.Equal("PROD1", products[0].ProductCode);
            Assert.Equal(2, products[1].ProductId);
            Assert.Equal("PROD2", products[1].ProductCode);
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_FiltersDisabledProducts()
        {
            // Setup the reader to have 3 rows
            var readCalls = 0;
            _reader.Setup(r => r.Read()).Returns(() =>
            {
                readCalls++;
                return readCalls <= 3; // Returns true for first 3 calls, false for the 4th
            });

            // Setup the data for each row
            _reader.Setup(r => r["IsEnable"]).Returns(() =>
            {
                switch (readCalls)
                {
                    case 1: return true;  // First row enabled
                    case 2: return false; // Second row disabled
                    case 3: return true;  // Third row enabled
                    default: return false;
                }
            });

            _reader.Setup(r => r["ProductID"]).Returns(() =>
            {
                switch (readCalls)
                {
                    case 1: return 1;
                    case 2: return 2;
                    case 3: return 3;
                    default: return 0;
                }
            });

            _reader.Setup(r => r["ProductCode"]).Returns(() =>
            {
                switch (readCalls)
                {
                    case 1: return "PROD1";
                    case 2: return "PROD2";
                    case 3: return "PROD3";
                    default: return "";
                }
            });

            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            var result = await _repository.GetEnableBetLimitProducts("TestSite", "PROD");

            Assert.Equal(2, result.Count()); // Only enabled products (rows 1 and 3)
            var products = result.ToList();
            Assert.Equal(1, products[0].ProductId);
            Assert.Equal("PROD1", products[0].ProductCode);
            Assert.Equal(3, products[1].ProductId);
            Assert.Equal("PROD3", products[1].ProductCode);
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_HandlesAllDisabledProducts()
        {
            _reader.SetupSequence(r => r.Read()).Returns(true).Returns(true).Returns(false);
            _reader.SetupSequence(r => r["IsEnable"]).Returns(false).Returns(false); // All disabled
            _reader.SetupSequence(r => r["ProductID"]).Returns(1).Returns(2);
            _reader.SetupSequence(r => r["ProductCode"]).Returns("PROD1").Returns("PROD2");
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            var result = await _repository.GetEnableBetLimitProducts("TestSite", "PROD");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_HandlesNullSiteName()
        {
            _reader.Setup(r => r.Read()).Returns(false);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            var result = await _repository.GetEnableBetLimitProducts(null, "PROD");

            _db.Verify(x => x.SetParameterValue("LicenseeName", null), Times.Once);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_HandlesEmptySiteName()
        {
            _reader.Setup(r => r.Read()).Returns(false);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            var result = await _repository.GetEnableBetLimitProducts("", "PROD");

            _db.Verify(x => x.SetParameterValue("LicenseeName", ""), Times.Once);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_HandlesNullEnvironment()
        {
            _reader.Setup(r => r.Read()).Returns(false);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            var result = await _repository.GetEnableBetLimitProducts("TestSite", null);

            _db.Verify(x => x.SetParameterValue("SettingStatus", 0), Times.Once); // null is not in the list
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_HandlesEmptyEnvironment()
        {
            _reader.Setup(r => r.Read()).Returns(false);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            var result = await _repository.GetEnableBetLimitProducts("TestSite", "");

            _db.Verify(x => x.SetParameterValue("SettingStatus", 0), Times.Once); // empty string is not in the list
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_HandlesProductIdZero()
        {
            _reader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            _reader.Setup(r => r["IsEnable"]).Returns(true);
            _reader.Setup(r => r["ProductID"]).Returns(0);
            _reader.Setup(r => r["ProductCode"]).Returns("PROD0");
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            var result = await _repository.GetEnableBetLimitProducts("TestSite", "PROD");

            Assert.Single(result);
            var product = result.First();
            Assert.Equal(0, product.ProductId);
            Assert.Equal("PROD0", product.ProductCode);
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_HandlesNullProductCode()
        {
            _reader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            _reader.Setup(r => r["IsEnable"]).Returns(true);
            _reader.Setup(r => r["ProductID"]).Returns(1);
            _reader.Setup(r => r["ProductCode"]).Returns(null);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            var result = await _repository.GetEnableBetLimitProducts("TestSite", "PROD");

            Assert.Single(result);
            var product = result.First();
            Assert.Equal(1, product.ProductId);
            Assert.Equal("", product.ProductCode); // ToString() converts null to empty string
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_HandlesBooleanConversions()
        {
            // Setup the reader to have 3 rows with different boolean representations
            var readCalls = 0;
            _reader.Setup(r => r.Read()).Returns(() =>
            {
                readCalls++;
                return readCalls <= 3; // Returns true for first 3 calls, false for the 4th
            });

            // Setup the data for each row
            _reader.Setup(r => r["IsEnable"]).Returns(() =>
            {
                switch (readCalls)
                {
                    case 1: return true;    // First row enabled (boolean true)
                    case 2: return false;   // Second row disabled (boolean false)
                    case 3: return "True";  // Third row enabled (string "True")
                    default: return false;
                }
            });

            _reader.Setup(r => r["ProductID"]).Returns(() =>
            {
                switch (readCalls)
                {
                    case 1: return 1;
                    case 2: return 2;
                    case 3: return 3;
                    default: return 0;
                }
            });

            _reader.Setup(r => r["ProductCode"]).Returns(() =>
            {
                switch (readCalls)
                {
                    case 1: return "PROD1";
                    case 2: return "PROD2";
                    case 3: return "PROD3";
                    default: return "";
                }
            });

            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            var result = await _repository.GetEnableBetLimitProducts("TestSite", "PROD");

            Assert.Equal(2, result.Count()); // Only true and "True" should be considered enabled
            var products = result.ToList();
            Assert.Equal(1, products[0].ProductId);
            Assert.Equal("PROD1", products[0].ProductCode);
            Assert.Equal(3, products[1].ProductId);
            Assert.Equal("PROD3", products[1].ProductCode);
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_HandlesCaseInsensitiveEnvironment()
        {
            _reader.Setup(r => r.Read()).Returns(false);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            await _repository.GetEnableBetLimitProducts("TestSite", "local");

            _db.Verify(x => x.SetParameterValue("SettingStatus", 0), Times.Once); // "local" != "LOCAL"
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_HandlesMixedCaseEnvironment()
        {
            _reader.Setup(r => r.Read()).Returns(false);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            await _repository.GetEnableBetLimitProducts("TestSite", "Local");

            _db.Verify(x => x.SetParameterValue("SettingStatus", 0), Times.Once); // "Local" != "LOCAL"
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_HandlesSingleEnabledProduct()
        {
            _reader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            _reader.Setup(r => r["IsEnable"]).Returns(true);
            _reader.Setup(r => r["ProductID"]).Returns(5);
            _reader.Setup(r => r["ProductCode"]).Returns("SINGLE");
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            var result = await _repository.GetEnableBetLimitProducts("TestSite", "LOCAL");

            Assert.Single(result);
            var product = result.First();
            Assert.Equal(5, product.ProductId);
            Assert.Equal("SINGLE", product.ProductCode);
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_HandlesLargeProductId()
        {
            _reader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            _reader.Setup(r => r["IsEnable"]).Returns(true);
            _reader.Setup(r => r["ProductID"]).Returns(int.MaxValue);
            _reader.Setup(r => r["ProductCode"]).Returns("LARGE_ID");
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            var result = await _repository.GetEnableBetLimitProducts("TestSite", "SIT");

            Assert.Single(result);
            var product = result.First();
            Assert.Equal(int.MaxValue, product.ProductId);
            Assert.Equal("LARGE_ID", product.ProductCode);
        }

        [Fact]
        public async Task GetEnableBetLimitProducts_HandlesLongProductCode()
        {
            var longProductCode = new string('A', 1000);
            _reader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            _reader.Setup(r => r["IsEnable"]).Returns(true);
            _reader.Setup(r => r["ProductID"]).Returns(1);
            _reader.Setup(r => r["ProductCode"]).Returns(longProductCode);
            _db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(_reader.Object);

            var result = await _repository.GetEnableBetLimitProducts("TestSite", "UAT");

            Assert.Single(result);
            var product = result.First();
            Assert.Equal(1, product.ProductId);
            Assert.Equal(longProductCode, product.ProductCode);
        }
    }
}