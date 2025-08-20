namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.MetaModels
{
    using AutoFixture;
    using MetaModel;

    public class PlayerUpdateChargeCommMetaModelTests
    {
        private readonly IFixture _fixture;

        public PlayerUpdateChargeCommMetaModelTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void IsValid_ShouldAlwaysReturnTrue_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();

            // Act
            var result = metaModel.IsValid;

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0.0, 1.0)]
        [InlineData(5.5, 3.2)]
        [InlineData(-1.0, 10.0)]
        [InlineData(100.0, 0.0)]
        public void IsValid_ShouldAlwaysReturnTrue_WhenDifferentCommissionValues(double comm, double oldComm)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();
            metaModel.Comm = comm;
            metaModel.OldComm = oldComm;

            // Act
            var result = metaModel.IsValid;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Properties_ShouldBeSettableAndGettable_WhenCreated()
        {
            // Arrange
            var metaModel = new PlayerUpdateChargeCommMetaModel();
            const int custId = 123;
            const int licenseeCustId = 456;
            const int currencyId = 789;
            const int userId = 101;
            const string siteName = "TestSite";
            const string ip = "192.168.1.1";
            const string language = "en";
            const double comm = 5.5;
            const double oldComm = 3.0;

            // Act
            metaModel.CustId = custId;
            metaModel.LicenseeCustId = licenseeCustId;
            metaModel.CurrencyId = currencyId;
            metaModel.UserId = userId;
            metaModel.SiteName = siteName;
            metaModel.Ip = ip;
            metaModel.Language = language;
            metaModel.Comm = comm;
            metaModel.OldComm = oldComm;

            // Assert
            Assert.Equal(custId, metaModel.CustId);
            Assert.Equal(licenseeCustId, metaModel.LicenseeCustId);
            Assert.Equal(currencyId, metaModel.CurrencyId);
            Assert.Equal(userId, metaModel.UserId);
            Assert.Equal(siteName, metaModel.SiteName);
            Assert.Equal(ip, metaModel.Ip);
            Assert.Equal(language, metaModel.Language);
            Assert.Equal(comm, metaModel.Comm);
            Assert.Equal(oldComm, metaModel.OldComm);
        }

        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues_WhenCreated()
        {
            // Act
            var metaModel = new PlayerUpdateChargeCommMetaModel();

            // Assert
            Assert.Equal(0, metaModel.CustId);
            Assert.Equal(0, metaModel.LicenseeCustId);
            Assert.Equal(0, metaModel.CurrencyId);
            Assert.Equal(0, metaModel.UserId);
            Assert.Null(metaModel.SiteName);
            Assert.Null(metaModel.Ip);
            Assert.Null(metaModel.Language);
            Assert.Equal(0.0, metaModel.Comm);
            Assert.Equal(0.0, metaModel.OldComm);
            Assert.True(metaModel.IsValid);
        }

        [Theory]
        [InlineData(2.5)]
        [InlineData(0.0)]
        [InlineData(10.0)]
        [InlineData(-5.0)]
        public void Comm_ShouldAcceptAnyDoubleValue_WhenSet(double commValue)
        {
            // Arrange
            var metaModel = new PlayerUpdateChargeCommMetaModel();

            // Act
            metaModel.Comm = commValue;

            // Assert
            Assert.Equal(commValue, metaModel.Comm);
        }

        [Theory]
        [InlineData(1.5)]
        [InlineData(0.0)]
        [InlineData(7.5)]
        [InlineData(-2.0)]
        public void OldComm_ShouldAcceptAnyDoubleValue_WhenSet(double oldCommValue)
        {
            // Arrange
            var metaModel = new PlayerUpdateChargeCommMetaModel();

            // Act
            metaModel.OldComm = oldCommValue;

            // Assert
            Assert.Equal(oldCommValue, metaModel.OldComm);
        }
    }
}