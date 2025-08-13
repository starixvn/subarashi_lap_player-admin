namespace Fanex.Athena.PlayerAdmin.WebHost.UnitTests.DbSettingConfig
{
    using Core.Services;
    using Moq;

    public class ShardInfoAccessorTests
    {
        private readonly Mock<IContextService> _contextService;
        private readonly ShardInfoAccessor _shardInfoAccessor;

        public ShardInfoAccessorTests()
        {
            _contextService = new Mock<IContextService>();
            _shardInfoAccessor = new ShardInfoAccessor(_contextService.Object);
        }

        [Theory]
        [InlineData("shard_test")]
        public void TryUseShardDB_ShouldReturnTrue_WhenKeyIsShardingAndSiteDBServerIsNotNull(string key)
        {
            _contextService.Setup(x => x.SiteDBServer).Returns("TestServer");

            var result = _shardInfoAccessor.TryUseShardDB(key, out var dbServer);

            Assert.True(result);
            Assert.Equal("TestServer", dbServer);
        }

        [Theory]
        [InlineData("test")]
        public void TryUseShardDB_ShouldReturnFalse_WhenKeyIsNotSharding(string key)
        {
            _contextService.Setup(x => x.SiteDBServer).Returns("TestServer");

            var result = _shardInfoAccessor.TryUseShardDB(key, out var dbServer);

            Assert.False(result);
            Assert.Null(dbServer);
        }

        [Theory]
        [InlineData("shard_test")]
        public void TryUseShardDB_ShouldReturnFalse_WhenSiteDBServerIsNull(string key)
        {
            _contextService.Setup(x => x.SiteDBServer).Returns((string)null);

            var result = _shardInfoAccessor.TryUseShardDB(key, out var dbServer);

            Assert.False(result);
            Assert.Null(dbServer);
        }

        [Theory]
        [InlineData("shard_test", "SiteDB_test")]
        public void GetShardSPKey_ShouldReturnCorrectKey(string key, string expected)
        {
            var result = _shardInfoAccessor.GetShardSPKey(key);

            Assert.Equal(expected, result);
        }
    }
}
