namespace Fanex.Athena.PlayerAdmin.Core.Storages
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.Extensions.Caching.Distributed;

    public class RedisCacheTicketStore : ITicketStore
    {
        private const string KeyPrefix = "Cookies:";
        private readonly IDistributedCache _cache;

        public RedisCacheTicketStore(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var guid = Guid.NewGuid();
            var key = KeyPrefix + guid;
            await RenewAsync(key, ticket);
            return key;
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var options = new DistributedCacheEntryOptions();
            var expiresUtc = ticket.Properties.ExpiresUtc;
            if (expiresUtc.HasValue)
            {
                options.SetAbsoluteExpiration(expiresUtc.Value);
            }

            byte[] val = SerializeToBytes(ticket);
            _cache.Set(key, val, options);
            return Task.FromResult(0);
        }

        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            AuthenticationTicket ticket;
            byte[] bytes = _cache.Get(key);
            ticket = DeserializeFromBytes(bytes);
            return Task.FromResult(ticket);
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.FromResult(0);
        }

        private static byte[] SerializeToBytes(AuthenticationTicket source)
            => TicketSerializer.Default.Serialize(source);

        private static AuthenticationTicket DeserializeFromBytes(byte[] source)
            => source == null ? null : TicketSerializer.Default.Deserialize(source);
    }
}