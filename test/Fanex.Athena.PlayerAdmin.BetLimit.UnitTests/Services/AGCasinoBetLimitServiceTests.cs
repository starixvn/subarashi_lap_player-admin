

using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Mediator;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.Services;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Services
{
    public class AGCasinoBetLimitServiceTests
    {
        private ILogger<AGCasinoBetLimitService> _logger;
        private IPublisher _publisher;
        private BetLimitApiSetting _appSetting;
        private IOptions<BetLimitApiSetting> _options;
        private IAGCasinoBetLimitRepository _repository;
        private HttpClient _httpClient;
        private HttpMessageHandler _handler;

        [SetUp]
        public void Setup() 
        {
            _logger = Substitute.For<ILogger<AGCasinoBetLimitService>>();
            _publisher= Substitute.For<IPublisher>();
            _appSetting = FakeHelper.Create<BetLimitApiSetting>(_ => _.AGCasino, "http://localhost.com");
            _options = Substitute.For<IOptions<BetLimitApiSetting>>();
            _options.Value.Returns(_appSetting);
            _repository = Substitute.For<IAGCasinoBetLimitRepository>();
            _handler = Substitute.For<HttpMessageHandler>();
            _httpClient = Substitute.For<HttpClient>(_handler);
            _httpClient.BaseAddress = new System.Uri("http://localhost.com");
        }

        [Test]
        public async Task GetGroupBettingsAsync_WhenThrowExection_ReturnEmptyList()
        {
            //Arrange
            var siteName = FakeHelper.Create<string>();
            var currency = FakeHelper.Create<int>();
            _handler.SubstituteProtected("SendAsync", Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
                .ThrowsForAnyArgs(new Exception("error"));

            var service = CreateService();

            //Act
            var result = await service.GetGroupBettingsAsync(siteName, currency);

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetGroupBettingsAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var siteName = FakeHelper.Create<string>();
            var currency = FakeHelper.Create<int>();
            var expectedResult = FakeHelper.CreateMany<AgCasinoGame>();
            var reponseContent = BuildXml(expectedResult);
            var stringContent = new StringContent(reponseContent);
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content= stringContent };
            _handler.SubstituteProtected("SendAsync", Arg.Any<HttpRequestMessage>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(response));
            
            var service = CreateService();

            //Act
            var result = await service.GetGroupBettingsAsync(siteName, currency);

            //Assert
            Assert.AreEqual(expectedResult.Count(), result.Count());
            Assert.AreEqual(expectedResult.First().Name, result.First().Name);  
        }

        [Test]
        public async Task GetBetSettingAsync_WhenCall_CallRepoMethod()
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var service = CreateService();

            //Act
            await service.GetBetSettingAsync(custId);

            //Assert
            await _repository.Received(1).GetBetSettingAsync(custId);
        }

        [Test]
        public async Task UpdateSettingAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>>();
            var expectedResult = FakeHelper.Create<ResultInfo>();
            expectedResult.Code = 0;
            _repository.UpdateSettingAsync(metaModel).Returns(expectedResult);

            var service = CreateService();

            //Act
            var result = await service.UpdateSettingAsync(metaModel);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreSame(expectedResult, result);
                _publisher.Received(1).Publish(Arg.Any<PlayerUpdatedBetLimitEvent>());
            });
        }

        [Test]
        public async Task UpdateStatusAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _repository.UpdateStatusAsync(metaModel).Returns(expectedResult);

            var service = CreateService();

            //Act
            var result = await service.UpdateStatusAsync(metaModel);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreSame(expectedResult, result);
                _publisher.Received(1).Publish(Arg.Any<PlayerUpdatedProductStatusEvent>());
            });
        }

        private AGCasinoBetLimitService CreateService()
            => new AGCasinoBetLimitService(_logger, _options, _repository, _httpClient, _publisher);

        private static string BuildXml(IEnumerable<AgCasinoGame> games)
        {
            var doc = new XDocument(new XElement("Root"));
            var root = doc.Root;
            foreach (var game in games)
            {
                var elem = new XElement("Game");
                elem.SetAttributeValue("Name", game.Name);
                elem.SetAttributeValue("GameType", game.GameType);
                foreach (var group in game.Groups)
                {
                    var groupElem = new XElement("Group");
                    groupElem.SetAttributeValue("GroupType", group.GroupType);
                    groupElem.Add(new XElement("Min", group.Min));
                    groupElem.Add(new XElement("Max", group.Max));
                    elem.Add(groupElem);
                }
                root.Add(elem);
            }
            return doc.ToString();
        }
    }
}
