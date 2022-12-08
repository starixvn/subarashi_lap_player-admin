using Fanex.Athena.PlayerAdmin.Core.Storages;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Storages
{
    public class RedisCacheTicketStoreTests
    {
        private IDistributedCache _cache;

        [SetUp]
        public void SetUp()
            => _cache = Substitute.For<IDistributedCache>();

        [Test]
        public async Task RemoveAsync_WhenCall_CallRelevantMethod()
        {
            //Arrange
            var key = FakeHelper.Create<string>();
            var storage = CreateStorage();

            //Act
            await storage.RemoveAsync(key);

            //Assert
            _cache.Received(1).Remove(key);
        }

        [Test]
        public async Task RetrieveAsync_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var key = FakeHelper.Create<string>();
            var ticket = FakeHelper.Create<AuthenticationTicket>();
            var bytes = TicketSerializer.Default.Serialize(ticket);
            _cache.Get(key).Returns(bytes);

            var store = CreateStorage();

            //Act
            var result = await store.RetrieveAsync(key);

            //Assert
            Assert.True(Enumerable.SequenceEqual(TicketSerializer.Default.Serialize(result), bytes));
        }

        [Test]
        public async Task RetrieveAsync_WhenCacheReturnNull_ReturnNull()
        {
            //Arrange
            var key = FakeHelper.Create<string>();
            byte[] bytes = null;
            _cache.Get(key).Returns(bytes);

            var store = CreateStorage();

            //Act
            var result = await store.RetrieveAsync(key);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task RenewAsync_WhenCall_CallRelevantMethod()
        {
            //Arrange
            var key = FakeHelper.Create<string>();
            var ticket = FakeHelper.Create<AuthenticationTicket>();
            ticket.Properties.ExpiresUtc = FakeHelper.Create<DateTimeOffset?>();
            var bytes = TicketSerializer.Default.Serialize(ticket);

            var store = CreateStorage();

            //Act
            await store.RenewAsync(key, ticket);


            //Assert
            _cache.Received(1).Set(key, Arg.Is<byte[]>(_ => Enumerable.SequenceEqual(_, bytes)), Arg.Any<DistributedCacheEntryOptions>());
        }

        [Test]
        public async Task StoreAsync_WhenCall_CallRelevantMethod()
        {
            //Arrange
            const string keyPrefix = "Cookies:";
            var ticket = FakeHelper.Create<AuthenticationTicket>();
            var bytes = TicketSerializer.Default.Serialize(ticket);

            var storage = CreateStorage();

            //Act
            var result = await storage.StoreAsync(ticket);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.True(result.Contains(keyPrefix));
                _cache.Received(1).Set(
                    Arg.Is<string>(_ => _.Contains(keyPrefix)),
                    Arg.Is<byte[]>(_ => Enumerable.SequenceEqual(_, bytes)),
                    Arg.Any<DistributedCacheEntryOptions>());
            });
        }


        private RedisCacheTicketStore CreateStorage()
            => new RedisCacheTicketStore(_cache);
    }
}
