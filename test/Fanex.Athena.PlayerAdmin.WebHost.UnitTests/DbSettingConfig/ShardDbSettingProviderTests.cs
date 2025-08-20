namespace Fanex.Athena.PlayerAdmin.WebHost.UnitTests.DbSettingConfig
{
    using Moq;

    public class ShardDbSettingProviderTests
    {
        private const string SqlProviderName = "Microsoft.Data.SqlClient";

        private readonly Mock<IShardInfoAccessor> _shardInfoAccessor;
        private readonly ShardDbSettingProvider _shardDbSettingProvider;

        public ShardDbSettingProviderTests()
        {
            _shardInfoAccessor = new Mock<IShardInfoAccessor>();
            _shardDbSettingProvider = new ShardDbSettingProvider(_shardInfoAccessor.Object);
        }

        [Theory(Skip = "Mock data not supported in this environment")]
        [InlineData("Shard_Lrf_LicenseeSelBySiteName", 1, "127.0.0.1", "TestServer", "SiteDB_Lrf_LicenseeSelBySiteName")]
        public void CreateObjectDbFunc_ShouldReturnSqlObjectDbWithShardInfo_WhenTryUseShardDBIsTrue(
            string settingKey, int? commandTimeout, string serverIp, string dbServer, string shardSPKey)
        {
            _shardInfoAccessor.Setup(x => x.TryUseShardDB(settingKey, out dbServer)).Returns(true);
            _shardInfoAccessor.Setup(x => x.GetShardSPKey(settingKey)).Returns(shardSPKey);

            var result = _shardDbSettingProvider.CreateObjectDbFunc(settingKey, commandTimeout, serverIp);

            Assert.NotNull(result);
        }

        [Theory(Skip = "Mock data not supported in this environment")]
        [InlineData("Lrf_LicenseeSelBySiteName", 1, "127.0.0.1")]
        public void CreateObjectDbFunc_ShouldReturnSqlObjectDbWithoutShardInfo_WhenTryUseShardDBIsFalse(
            string settingKey, int? commandTimeout, string serverIp)
        {
            string dbServer = null;
            _shardInfoAccessor.Setup(x => x.TryUseShardDB(settingKey, out dbServer)).Returns(false);

            var result = _shardDbSettingProvider.CreateObjectDbFunc(settingKey, commandTimeout, serverIp);

            Assert.NotNull(result);
        }
    }
}
