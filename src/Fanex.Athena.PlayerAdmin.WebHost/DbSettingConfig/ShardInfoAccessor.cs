using Fanex.Athena.PlayerAdmin.Core.Services;
using System;

namespace Fanex.Athena.PlayerAdmin.WebHost
{
    public interface IShardInfoAccessor
    {
        bool TryUseShardDB(string key, out string dbServer);

        string GetShardSPKey(string key);
    }

    public class ShardInfoAccessor : IShardInfoAccessor
    {
        const string PrefixShardingDefault = "shard_";

        readonly string _prefixSharding;

        private readonly IContextService _contextService;

        public ShardInfoAccessor(IContextService contextService, string prefixSharding = PrefixShardingDefault)
        {
            _contextService = contextService;
            _prefixSharding = prefixSharding;
        }

        public bool TryUseShardDB(string key, out string dbServer)
        {
            if (IsSharding(key) && !string.IsNullOrEmpty(SiteDBServer))
            {
                dbServer = SiteDBServer;
                return true;
            }
            dbServer = null;
            return false;
        }

        public string GetShardSPKey(string key) => $"SiteDB_{key.Substring(_prefixSharding.Length)}";

        private bool IsSharding(string key) => key.StartsWith(_prefixSharding, StringComparison.OrdinalIgnoreCase);

        private string SiteDBServer => _contextService.SiteDBServer;
    }
}
