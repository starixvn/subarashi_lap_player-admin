using Fanex.Data;
using Fanex.Data.SqlServer;

namespace Fanex.Athena.PlayerAdmin.WebHost
{
    public class ShardDbSettingProvider
    {
        private readonly IShardInfoAccessor _shardInfoAccessor;

        public ShardDbSettingProvider(IShardInfoAccessor shardInfoAccessor)
        {
            _shardInfoAccessor = shardInfoAccessor;
        }

        public static string ProviderName => nameof(ShardDbSettingProvider);

        public IObjectDb CreateObjectDbFunc(string settingKey, int? commandTimeout, string serverIp)
        {
            if (_shardInfoAccessor.TryUseShardDB(settingKey, out var dbServer))
            {
                return new SqlObjectDb(_shardInfoAccessor.GetShardSPKey(settingKey), commandTimeout, dbServer);
            }

            return new SqlObjectDb(settingKey, commandTimeout, serverIp);
        }
    }
}