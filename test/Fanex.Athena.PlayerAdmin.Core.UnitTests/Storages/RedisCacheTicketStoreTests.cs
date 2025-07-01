using System;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.Storages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Storages
{
    public class RedisCacheTicketStoreTests
    {
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly RedisCacheTicketStore _store;
        private readonly AuthenticationTicket _ticket;
        private readonly string _key;

        public RedisCacheTicketStoreTests()
        {
            _cacheMock = new Mock<IDistributedCache>();
            _store = new RedisCacheTicketStore(_cacheMock.Object);
            var identity = new System.Security.Claims.ClaimsIdentity();
            var principal = new System.Security.Claims.ClaimsPrincipal(identity);
            _ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), CookieAuthenticationDefaults.AuthenticationScheme);
            _key = "Cookies:test-key";
        }

        [Fact]
        public async Task StoreAsync_CallsRenewAndReturnsKey()
        {
            // Arrange
            var store = new RedisCacheTicketStore(_cacheMock.Object);
            var ticket = _ticket;

            // Act
            var key = await store.StoreAsync(ticket);

            // Assert
            Assert.StartsWith("Cookies:", key);
            _cacheMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>()), Times.Once);
        }

        [Fact]
        public async Task RenewAsync_WithExpiresUtc_SetsAbsoluteExpiration()
        {
            // Arrange
            var ticket = new AuthenticationTicket(_ticket.Principal, new AuthenticationProperties { ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10) }, CookieAuthenticationDefaults.AuthenticationScheme);

            // Act
            await _store.RenewAsync(_key, ticket);

            // Assert
            _cacheMock.Verify(x => x.Set(_key, It.IsAny<byte[]>(), It.Is<DistributedCacheEntryOptions>(o => o.AbsoluteExpiration.HasValue)), Times.Once);
        }

        [Fact]
        public async Task RenewAsync_WithoutExpiresUtc_DoesNotSetAbsoluteExpiration()
        {
            // Arrange
            var ticket = new AuthenticationTicket(_ticket.Principal, new AuthenticationProperties(), CookieAuthenticationDefaults.AuthenticationScheme);

            // Act
            await _store.RenewAsync(_key, ticket);

            // Assert
            _cacheMock.Verify(x => x.Set(_key, It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>()), Times.Once);
        }

        [Fact]
        public async Task RetrieveAsync_WithExistingKey_ReturnsTicket()
        {
            // Arrange
            var ticket = _ticket;
            var bytes = TicketSerializer.Default.Serialize(ticket);
            _cacheMock.Setup(x => x.Get(_key)).Returns(bytes);

            // Act
            var result = await _store.RetrieveAsync(_key);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ticket.AuthenticationScheme, result.AuthenticationScheme);
        }

        [Fact]
        public async Task RetrieveAsync_WithMissingKey_ReturnsNull()
        {
            // Arrange
            _cacheMock.Setup(x => x.Get(_key)).Returns((byte[])null);

            // Act
            var result = await _store.RetrieveAsync(_key);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveAsync_CallsCacheRemove()
        {
            // Act
            await _store.RemoveAsync(_key);

            // Assert
            _cacheMock.Verify(x => x.Remove(_key), Times.Once);
        }
    }
} 