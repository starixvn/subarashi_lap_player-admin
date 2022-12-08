using AutoFixture;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Services
{
    public class SubsidiaryServiceTests
    {
        private IDbFactory _dbFactory;
        private IObjectDb _objectDb;

        [SetUp]
        public void SetUp()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _objectDb = Substitute.For<IObjectDb>();
            _dbFactory.GetObjectDb(string.Empty).ReturnsForAnyArgs(_objectDb);
        }

        [TestCase(SubsidiaryViewType.NotManagement, false, "", true)]
        [TestCase(SubsidiaryViewType.NormalManagement, false, "", true)]
        [TestCase(SubsidiaryViewType.NormalManagement, true, "", true)]
        [TestCase(SubsidiaryViewType.NormalManagement, true, "1", false)]
        public void IsIgnoredSubsidiarySearch_WhenCall_ReturnCorrectValue(
            byte viewType,
            bool subsidiarySearch,
            string subsidiaryList,
            bool expectedResult)
        {
            //Arrange
            var service = CreateService();

            //Act
            var result = service.IsIgnoredSubsidiarySearch(subsidiarySearch, subsidiaryList, viewType);

            //Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetAgentList_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var subsidiaryAgents = FakeHelper.CreateMany<SubsidiaryAgent>();
            SetUpSubsidiaryAgentReturnFromDB(subsidiaryAgents);
            var agentList = new Dictionary<int, int[]> { { 1, subsidiaryAgents.Select(_ => _.AgentId).ToArray() } };
            var subsidiaryList = string.Join(",", subsidiaryAgents.Select(_ => _.OperatorId));

            var service = CreateService();

            //Act
            var result = await service.GetAgentList("site", agentList, subsidiaryList);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(subsidiaryAgents.Select(_ => _.CurrencyId).Distinct().Count()));
        }

        [Test]
        public async Task GetSubsidiaryList_WhenViewTypeIsNotManagement_ReturnEmptyResult()
        {
            //Arramge
            var service = CreateService();

            //Act
            var result = await service.GetSubsidiaryList("test", null, 1, SubsidiaryViewType.NotManagement);

            //Assert
            Assert.IsEmpty(result);
        }

        [TestCase(null)]
        [TestCase(1)]
        public async Task GetUserSubsidiaries_WhenViewTypeIsMasterManagement_ReturnCorrectValue(int? whiteLabelId)
        {
            //Arrange
            var operatorId = FakeHelper.Create<string>();
            var susidiaryAgent = FakeHelper.Create<SubsidiaryAgent>(_ => _.OperatorId, operatorId);
            var agentList = new Dictionary<int, int[]> { { 1, new int[] { susidiaryAgent.AgentId } } };
            var fixture = new Fixture();
            fixture.Customize<Subsidiary>(_ => _.With(s => s.OperatorId, operatorId)
                                                .With(s => s.WhiteLabelID, whiteLabelId));

            var licenseeSubsidiaries = fixture.CreateMany<Subsidiary>();
            SetUpLicenseeSubsidiaryReturnFromDB(licenseeSubsidiaries);
            SetUpSubsidiaryAgentReturnFromDB(new List<SubsidiaryAgent> { susidiaryAgent });
            var service = CreateService();

            //Act
            var result = await service.GetSubsidiaryList("site", agentList, whiteLabelId, SubsidiaryViewType.MasterManagement);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [TestCase(null)]
        [TestCase(1)]
        public async Task GetUserSubsidiaries_WhenViewTypeIsNormalManagement_ReturnCorrectValue(int? whiteLabelId)
        {
            //Arrange
            var operatorId = FakeHelper.Create<string>();
            var susidiaryAgent = FakeHelper.Create<SubsidiaryAgent>(_ => _.OperatorId, operatorId);
            var agentList = new Dictionary<int, int[]> { { 1, new int[] { susidiaryAgent.AgentId } } };
            var fixture = new Fixture();
            fixture.Customize<Subsidiary>(_ => _.With(s => s.OperatorId, operatorId)
                                                .With(s => s.WhiteLabelID, whiteLabelId));

            var licenseeSubsidiaries = fixture.CreateMany<Subsidiary>();
            SetUpLicenseeSubsidiaryReturnFromDB(licenseeSubsidiaries);
            SetUpSubsidiaryAgentReturnFromDB(new List<SubsidiaryAgent> { susidiaryAgent });
            var service = CreateService();

            //Act
            var result = await service.GetSubsidiaryList("site", agentList, whiteLabelId, SubsidiaryViewType.NormalManagement);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        private SubsidiaryService CreateService()
            => new SubsidiaryService(_dbFactory);

        private void SetUpSubsidiaryAgentReturnFromDB(IEnumerable<SubsidiaryAgent> subsidiaryAgents)
        {
            var reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReaderAsync().Returns(reader);
            reader.FakeNumberOfRead(subsidiaryAgents);
            var firstItem = subsidiaryAgents.First();
            var remainSubsidiaryAgents = subsidiaryAgents.Skip(1);
            reader["AgentID"].Returns(firstItem.AgentId, remainSubsidiaryAgents.Select(_ => (object)_.AgentId).ToArray());
            reader["OperatorID"].Returns(firstItem.OperatorId, remainSubsidiaryAgents.Select(_ => _.OperatorId).ToArray());
            reader["Currency"].Returns(firstItem.CurrencyId, remainSubsidiaryAgents.Select(_ => (object)_.CurrencyId).ToArray());
        }

        private void SetUpLicenseeSubsidiaryReturnFromDB(IEnumerable<Subsidiary> subsidiaries)
        {
            var reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReaderAsync(default).ReturnsForAnyArgs(reader);
            reader.FakeNumberOfRead(subsidiaries);
            var firstItem = subsidiaries.First();
            var remainSubsidiaries = subsidiaries.Skip(1);
            reader["WhiteLabelID"].Returns(firstItem.WhiteLabelID, remainSubsidiaries.Select(_ => (object)_.WhiteLabelID).ToArray());
            reader["Name"].Returns(firstItem.WhiteLabelName, remainSubsidiaries.Select(_ => _.WhiteLabelName).ToArray());
            reader["OperatorId"].Returns(firstItem.OperatorId, remainSubsidiaries.Select(_ => (object)_.OperatorId).ToArray());
        }
    }
}
