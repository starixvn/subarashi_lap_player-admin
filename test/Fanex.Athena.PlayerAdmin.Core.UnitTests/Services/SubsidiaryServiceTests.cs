namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Services
{
    using System.Data;
    using System.Data.Common;
    using Core.Constants;
    using Core.Repositories;
    using Core.Services;
    using Data;
    using Models;
    using Moq;

    public class SubsidiaryServiceTests
    {
        private readonly Mock<IDbFactory> _mockDbFactory;
        private readonly Mock<IObjectDb> _mockObjectDb;
        private readonly Mock<IDataReader> _mockDataReader;
        private readonly SubsidiaryService _service;

        public SubsidiaryServiceTests()
        {
            _mockDbFactory = new Mock<IDbFactory>();
            _mockObjectDb = new Mock<IObjectDb>();
            _mockDataReader = new Mock<IDataReader>();
            _service = new SubsidiaryService(_mockDbFactory.Object);
        }

        [Theory]
        [InlineData(SubsidiaryViewType.NotManagement, true, "subsidiary1", true)]
        [InlineData(SubsidiaryViewType.NormalManagement, false, "subsidiary1", true)]
        [InlineData(SubsidiaryViewType.NormalManagement, true, "", true)]
        [InlineData(SubsidiaryViewType.NormalManagement, true, null, true)]
        [InlineData(SubsidiaryViewType.NormalManagement, true, "   ", true)]
        [InlineData(SubsidiaryViewType.NormalManagement, true, "subsidiary1", false)]
        [InlineData(SubsidiaryViewType.MasterManagement, true, "subsidiary1", false)]
        public void IsIgnoredSubsidiarySearch_VariousConditions_ReturnsExpected(
            byte subsidiaryViewType, 
            bool subsidiarySearch, 
            string subsidiaryList, 
            bool expected)
        {
            var result = _service.IsIgnoredSubsidiarySearch(subsidiarySearch, subsidiaryList, subsidiaryViewType);
            
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAgentList_ValidInput_ReturnsFilteredAgentList()
        {
            var agentList = new Dictionary<int, int[]>
            {
                { 1, new[] { 100, 101 } },
                { 2, new[] { 200, 201 } }
            };
            var subsidiaryList = "OP1,OP2";
            var siteName = "TestSite";

            var mockSubsidiaryAgents = new List<SubsidiaryAgent>
            {
                new SubsidiaryAgent { AgentId = 100, OperatorId = "OP1", CurrencyId = 1 },
                new SubsidiaryAgent { AgentId = 101, OperatorId = "OP2", CurrencyId = 1 },
                new SubsidiaryAgent { AgentId = 200, OperatorId = "OP1", CurrencyId = 2 },
                new SubsidiaryAgent { AgentId = 999, OperatorId = "OP3", CurrencyId = 3 }
            };

            SetupMockForSubsidiaryAgents(mockSubsidiaryAgents);

            var result = await _service.GetAgentList(siteName, agentList, subsidiaryList);

            Assert.Equal(2, result.Count);
            Assert.True(result.ContainsKey(1));
            Assert.True(result.ContainsKey(2));
            Assert.Equal(2, result[1].Length);
            Assert.Contains(100, result[1]);
            Assert.Contains(101, result[1]);
            Assert.Single(result[2]);
            Assert.Contains(200, result[2]);
        }

        [Fact]
        public async Task GetAgentList_EmptySubsidiaryList_ReturnsEmptyDictionary()
        {
            var agentList = new Dictionary<int, int[]>
            {
                { 1, new[] { 100, 101 } }
            };
            var subsidiaryList = "";
            var siteName = "TestSite";

            SetupMockForSubsidiaryAgents(new List<SubsidiaryAgent>());

            var result = await _service.GetAgentList(siteName, agentList, subsidiaryList);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetSubsidiaryList_NotManagementViewType_ReturnsEmpty()
        {
            var agentList = new Dictionary<int, int[]>();
            var siteName = "TestSite";
            
            var result = await _service.GetSubsidiaryList(siteName, agentList, null, SubsidiaryViewType.NotManagement);
            
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetSubsidiaryList_NormalManagementViewType_ReturnsSubsidiaryTree()
        {
            var agentList = new Dictionary<int, int[]>
            {
                { 1, new[] { 100, 101 } }
            };
            var siteName = "TestSite";

            var mockSubsidiaries = new List<Subsidiary>
            {
                new Subsidiary { OperatorId = "OP1", WhiteLabelID = 1, WhiteLabelName = "WL1" },
                new Subsidiary { OperatorId = "OP2", WhiteLabelID = 1, WhiteLabelName = "WL1" }
            };

            var mockSubsidiaryAgents = new List<SubsidiaryAgent>
            {
                new SubsidiaryAgent { AgentId = 100, OperatorId = "OP1", CurrencyId = 1 },
                new SubsidiaryAgent { AgentId = 101, OperatorId = "OP2", CurrencyId = 1 }
            };

            SetupMockForSubsidiaries(mockSubsidiaries);
            SetupMockForSubsidiaryAgents(mockSubsidiaryAgents);

            var result = await _service.GetSubsidiaryList(siteName, agentList, null, SubsidiaryViewType.NormalManagement);

            Assert.Equal(2, result.Count());
            var resultArray = result.ToArray();
            Assert.Contains(resultArray, x => x.Value == "OP1");
            Assert.Contains(resultArray, x => x.Value == "OP2");
            Assert.All(resultArray, x => Assert.True(x.Checked));
        }

        [Fact]
        public async Task GetSubsidiaryList_MasterManagementViewType_ReturnsWhiteLabelTree()
        {
            var agentList = new Dictionary<int, int[]>
            {
                { 1, new[] { 100, 101 } }
            };
            var siteName = "TestSite";

            var mockSubsidiaries = new List<Subsidiary>
            {
                new Subsidiary { OperatorId = "OP1", WhiteLabelID = 1, WhiteLabelName = "WL1" },
                new Subsidiary { OperatorId = "OP2", WhiteLabelID = 1, WhiteLabelName = "WL1" },
                new Subsidiary { OperatorId = "OP3", WhiteLabelID = null, WhiteLabelName = null }
            };

            var mockSubsidiaryAgents = new List<SubsidiaryAgent>
            {
                new SubsidiaryAgent { AgentId = 100, OperatorId = "OP1", CurrencyId = 1 },
                new SubsidiaryAgent { AgentId = 101, OperatorId = "OP2", CurrencyId = 1 },
                new SubsidiaryAgent { AgentId = 101, OperatorId = "OP3", CurrencyId = 1 }
            };

            SetupMockForSubsidiaries(mockSubsidiaries);
            SetupMockForSubsidiaryAgents(mockSubsidiaryAgents);

            var result = await _service.GetSubsidiaryList(siteName, agentList, null, SubsidiaryViewType.MasterManagement);

            Assert.Equal(2, result.Count());
            var resultArray = result.ToArray();
            
            var whiteLabelGroup = resultArray.FirstOrDefault(x => x.Value == "1");
            Assert.NotNull(whiteLabelGroup);
            Assert.Equal("WL1", whiteLabelGroup.Name);
            Assert.Equal(2, whiteLabelGroup.Children.Count());

            var otherGroup = resultArray.FirstOrDefault(x => x.Value == "-1");
            Assert.NotNull(otherGroup);
            Assert.Single(otherGroup.Children);
        }

        [Fact]
        public async Task GetSubsidiaryList_WithWhiteLabelFilter_ReturnsFilteredResults()
        {
            var agentList = new Dictionary<int, int[]>
            {
                { 1, new[] { 100 } }
            };
            var siteName = "TestSite";
            var whiteLabelId = 1;

            var mockSubsidiaries = new List<Subsidiary>
            {
                new Subsidiary { OperatorId = "OP1", WhiteLabelID = 1, WhiteLabelName = "WL1" },
                new Subsidiary { OperatorId = "OP2", WhiteLabelID = 2, WhiteLabelName = "WL2" }
            };

            var mockSubsidiaryAgents = new List<SubsidiaryAgent>
            {
                new SubsidiaryAgent { AgentId = 100, OperatorId = "OP1", CurrencyId = 1 },
                new SubsidiaryAgent { AgentId = 100, OperatorId = "OP2", CurrencyId = 1 }
            };

            SetupMockForSubsidiaries(mockSubsidiaries);
            SetupMockForSubsidiaryAgents(mockSubsidiaryAgents);

            var result = await _service.GetSubsidiaryList(siteName, agentList, whiteLabelId, SubsidiaryViewType.MasterManagement);

            Assert.Single(result);
            var whiteLabelGroup = result.First();
            Assert.Equal("1", whiteLabelGroup.Value);
            Assert.Equal("WL1", whiteLabelGroup.Name);
        }

        [Fact]
        public async Task GetSubsidiaryList_CaseInsensitiveOperatorIdMatching_ReturnsResults()
        {
            var agentList = new Dictionary<int, int[]>
            {
                { 1, new[] { 100 } }
            };
            var siteName = "TestSite";

            var mockSubsidiaries = new List<Subsidiary>
            {
                new Subsidiary { OperatorId = "OP1", WhiteLabelID = 1, WhiteLabelName = "WL1" }
            };

            var mockSubsidiaryAgents = new List<SubsidiaryAgent>
            {
                new SubsidiaryAgent { AgentId = 100, OperatorId = "op1", CurrencyId = 1 }
            };

            SetupMockForSubsidiaries(mockSubsidiaries);
            SetupMockForSubsidiaryAgents(mockSubsidiaryAgents);

            var result = await _service.GetSubsidiaryList(siteName, agentList, null, SubsidiaryViewType.NormalManagement);

            Assert.Single(result);
            Assert.Equal("OP1", result.First().Value);
        }

        private void SetupMockForSubsidiaries(List<Subsidiary> subsidiaries)
        {
            var readCalls = 0;
            _mockDataReader.Setup(x => x.Read())
                .Returns(() => readCalls < subsidiaries.Count)
                .Callback(() => readCalls++);

            _mockDataReader.Setup(x => x["WhiteLabelID"])
                .Returns(() => readCalls > 0 && readCalls <= subsidiaries.Count ? subsidiaries[readCalls - 1].WhiteLabelID : null);

            _mockDataReader.Setup(x => x["Name"])
                .Returns(() => readCalls > 0 && readCalls <= subsidiaries.Count ? subsidiaries[readCalls - 1].WhiteLabelName : null);

            _mockDataReader.Setup(x => x["OperatorId"])
                .Returns(() => readCalls > 0 && readCalls <= subsidiaries.Count ? subsidiaries[readCalls - 1].OperatorId : null);

            _mockObjectDb.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockDataReader.Object);

            _mockDbFactory.Setup(x => x.GetObjectDb("Portal_Sub_GetSubsidiaryList", It.IsAny<int>()))
                .Returns(_mockObjectDb.Object);
        }

        private void SetupMockForSubsidiaryAgents(List<SubsidiaryAgent> subsidiaryAgents)
        {
            var readCalls = 0;
            var agentReader = new Mock<IDataReader>();
            
            agentReader.Setup(x => x.Read())
                .Returns(() => readCalls < subsidiaryAgents.Count)
                .Callback(() => readCalls++);

            agentReader.Setup(x => x["AgentID"])
                .Returns(() => readCalls > 0 && readCalls <= subsidiaryAgents.Count ? subsidiaryAgents[readCalls - 1].AgentId : 0);

            agentReader.Setup(x => x["OperatorID"])
                .Returns(() => readCalls > 0 && readCalls <= subsidiaryAgents.Count ? subsidiaryAgents[readCalls - 1].OperatorId : null);

            agentReader.Setup(x => x["Currency"])
                .Returns(() => readCalls > 0 && readCalls <= subsidiaryAgents.Count ? subsidiaryAgents[readCalls - 1].CurrencyId : 0);

            var agentObjectDb = new Mock<IObjectDb>();
            agentObjectDb.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(agentReader.Object);

            _mockDbFactory.Setup(x => x.GetObjectDb("Lrf_SubsidiarySel", It.IsAny<int>()))
                .Returns(agentObjectDb.Object);
        }
    }
}