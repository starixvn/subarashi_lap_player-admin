namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Storages
{
    using System.Security.Claims;
    using Core.Storages;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.Extensions.Caching.Distributed;

    public class RedisCacheTicketStoreTests
    {
        [Fact]
        public async Task StoreRetrieveRemove_Flow_Works()
        {
            var cache = new InMemoryDistributedCache();
            var store = new RedisCacheTicketStore(cache);
            var ticket = CreateTicket();

            var key = await store.StoreAsync(ticket);
            var retrieved = await store.RetrieveAsync(key);
            Assert.NotNull(retrieved);

            await store.RemoveAsync(key);
            var afterRemove = await store.RetrieveAsync(key);
            Assert.Null(afterRemove);
        }

        private static AuthenticationTicket CreateTicket()
        {
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "user") }, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();
            return new AuthenticationTicket(principal, props, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private sealed class InMemoryDistributedCache : IDistributedCache
        {
            private readonly Dictionary<string, byte[]> store = new();
            public byte[] Get(string key) => store.TryGetValue(key, out var v) ? v : null;
            public Task<byte[]> GetAsync(string key, CancellationToken token = default) => Task.FromResult(Get(key));

            public void Refresh(string key)
            {
            }

            public Task RefreshAsync(string key, CancellationToken token = default) => Task.CompletedTask;
            public void Remove(string key) => store.Remove(key);

            public Task RemoveAsync(string key, CancellationToken token = default)
            {
                Remove(key);
                return Task.CompletedTask;
            }

            public void Set(string key, byte[] value, DistributedCacheEntryOptions options) => store[key] = value;

            public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
            {
                Set(key, value, options);
                return Task.CompletedTask;
            }
        }
    }
}