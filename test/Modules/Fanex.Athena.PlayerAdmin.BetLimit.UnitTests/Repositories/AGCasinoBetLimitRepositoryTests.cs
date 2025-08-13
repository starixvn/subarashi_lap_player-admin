namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Repositories
{
    using System.Data.Common;
    using BetLimit.Repositories;
    using Core.Repositories;
    using Data;
    using MetaModel;
    using Moq;

    public class AGCasinoBetLimitRepositoryTests
    {
        private readonly Mock<IDbFactory> _dbFactory;
        private readonly Mock<IObjectDb> _db;
        private readonly AGCasinoBetLimitRepository _repository;

        public AGCasinoBetLimitRepositoryTests()
        {
            _dbFactory = new Mock<IDbFactory>();
            _db = new Mock<IObjectDb>();
            _dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(_db.Object);
            _repository = new AGCasinoBetLimitRepository(_dbFactory.Object);
        }

        [Fact]
        public async Task GetBetSettingAsync_ReturnsBetSetting()
        {
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("Currency")).Returns("1");
            _db.Setup(x => x.GetParameterValue("AGGroup")).Returns("GroupA");
            _db.Setup(x => x.GetParameterValue("IsEnabled")).Returns("true");
            _db.Setup(x => x.GetParameterValue("IsLineEnabled")).Returns("true");
            _db.Setup(x => x.GetParameterValue("CheckedGroups")).Returns("GroupA,GroupB");

            var result = await _repository.GetBetSettingAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.CurrencyId);
            Assert.Equal("GroupA", result.AgGroup);
            Assert.True(result.IsEnabled);
            Assert.True(result.IsLineEnabled);
            Assert.Equal(new[] { "GroupA", "GroupB" }, result.CheckedGroups);
        }

        [Fact]
        public async Task UpdateSettingAsync_ReturnsSuccess()
        {
            var metaModel = new PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>
            {
                Setting = new AgCasinoBetLimitMetaModel { Group = "GroupA" },
                Ip = "127.0.0.1",
                Language = "en",
                UserId = 1,
                CustId = 1
            };
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            _db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("Success");

            var result = await _repository.UpdateSettingAsync(metaModel);

            Assert.NotNull(result);
            Assert.Equal(0, result.Code);
            Assert.Equal("Success", result.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_ReturnsSuccess()
        {
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                Status = true,
                Ip = "127.0.0.1",
                Language = "en",
                UserId = 1,
                CustId = 1
            };
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            _db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("Success");

            var result = await _repository.UpdateStatusAsync(metaModel);

            Assert.NotNull(result);
            Assert.Equal(0, result.Code);
            Assert.Equal("Success", result.Message);
        }

        [Fact]
        public async Task GetBetSettingAsync_CallsCorrectStoredProcedure()
        {
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("Currency")).Returns("1");
            _db.Setup(x => x.GetParameterValue("AGGroup")).Returns("GroupA");
            _db.Setup(x => x.GetParameterValue("IsEnabled")).Returns("true");
            _db.Setup(x => x.GetParameterValue("IsLineEnabled")).Returns("false");
            _db.Setup(x => x.GetParameterValue("CheckedGroups")).Returns("GroupA");

            await _repository.GetBetSettingAsync(123);

            _dbFactory.Verify(x => x.GetObjectDb("Lrf_AGC_SelCustSetting", It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetBetSettingAsync_SetsCorrectParameters()
        {
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("Currency")).Returns("1");
            _db.Setup(x => x.GetParameterValue("AGGroup")).Returns("GroupA");
            _db.Setup(x => x.GetParameterValue("IsEnabled")).Returns("true");
            _db.Setup(x => x.GetParameterValue("IsLineEnabled")).Returns("false");
            _db.Setup(x => x.GetParameterValue("CheckedGroups")).Returns("GroupA");

            await _repository.GetBetSettingAsync(456);

            _db.Verify(x => x.SetParameterValue("CustID", 456), Times.Once);
            _db.Verify(x => x.SetParameterValue("Currency", null), Times.Once);
            _db.Verify(x => x.SetParameterValue("AGGroup", null), Times.Once);
            _db.Verify(x => x.SetParameterValue("IsEnabled", null), Times.Once);
            _db.Verify(x => x.SetParameterValue("IsLineEnabled", null), Times.Once);
            _db.Verify(x => x.SetParameterValue("CheckedGroups", null), Times.Once);
        }

        [Fact]
        public async Task GetBetSettingAsync_HandlesNumericConversions()
        {
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("Currency")).Returns("5");
            _db.Setup(x => x.GetParameterValue("AGGroup")).Returns("TestGroup");
            _db.Setup(x => x.GetParameterValue("IsEnabled")).Returns("false");
            _db.Setup(x => x.GetParameterValue("IsLineEnabled")).Returns("true");
            _db.Setup(x => x.GetParameterValue("CheckedGroups")).Returns("Group1,Group2,Group3");

            var result = await _repository.GetBetSettingAsync(1);

            Assert.Equal(5, result.CurrencyId);
            Assert.Equal("TestGroup", result.AgGroup);
            Assert.False(result.IsEnabled);
            Assert.True(result.IsLineEnabled);
            Assert.Equal(new[] { "Group1", "Group2", "Group3" }, result.CheckedGroups);
        }

        [Fact]
        public async Task GetBetSettingAsync_HandlesSingleGroupInCheckedGroups()
        {
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("Currency")).Returns("1");
            _db.Setup(x => x.GetParameterValue("AGGroup")).Returns("SingleGroup");
            _db.Setup(x => x.GetParameterValue("IsEnabled")).Returns("true");
            _db.Setup(x => x.GetParameterValue("IsLineEnabled")).Returns("false");
            _db.Setup(x => x.GetParameterValue("CheckedGroups")).Returns("SingleGroup");

            var result = await _repository.GetBetSettingAsync(1);

            Assert.Equal(new[] { "SingleGroup" }, result.CheckedGroups);
        }

        [Fact]
        public async Task GetBetSettingAsync_HandlesEmptyCheckedGroups()
        {
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("Currency")).Returns("1");
            _db.Setup(x => x.GetParameterValue("AGGroup")).Returns("TestGroup");
            _db.Setup(x => x.GetParameterValue("IsEnabled")).Returns("true");
            _db.Setup(x => x.GetParameterValue("IsLineEnabled")).Returns("false");
            _db.Setup(x => x.GetParameterValue("CheckedGroups")).Returns("");

            var result = await _repository.GetBetSettingAsync(1);

            Assert.Equal(new[] { "" }, result.CheckedGroups);
        }

        [Fact]
        public async Task UpdateSettingAsync_CallsCorrectStoredProcedure()
        {
            var metaModel = new PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>
            {
                Setting = new AgCasinoBetLimitMetaModel { Group = "TestGroup" },
                Ip = "192.168.1.1",
                Language = "zh-cn",
                UserId = 123,
                CustId = 456
            };
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            _db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");

            await _repository.UpdateSettingAsync(metaModel);

            _dbFactory.Verify(x => x.GetObjectDb("Lrf_AGC_UpdCustSetting", It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_SetsCorrectParameters()
        {
            var metaModel = new PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>
            {
                Setting = new AgCasinoBetLimitMetaModel { Group = "VIP" },
                Ip = "10.0.0.1",
                Language = "ja",
                UserId = 789,
                CustId = 101112
            };
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            _db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");

            await _repository.UpdateSettingAsync(metaModel);

            _db.Verify(x => x.SetParameterValue("aggroup", "VIP"), Times.Once);
            _db.Verify(x => x.SetParameterValue("ip", "10.0.0.1"), Times.Once);
            _db.Verify(x => x.SetParameterValue("language", "ja"), Times.Once);
            _db.Verify(x => x.SetParameterValue("userid", 789), Times.Once);
            _db.Verify(x => x.SetParameterValue("custid", 101112), Times.Once);
            _db.Verify(x => x.SetParameterValue("ErrCode", null), Times.Once);
            _db.Verify(x => x.SetParameterValue("ErrMsg", null), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_ReturnsErrorInfo()
        {
            var metaModel = new PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>
            {
                Setting = new AgCasinoBetLimitMetaModel { Group = "TestGroup" },
                Ip = "127.0.0.1",
                Language = "en",
                UserId = 1,
                CustId = 1
            };
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("ErrCode")).Returns("1");
            _db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("Update failed");

            var result = await _repository.UpdateSettingAsync(metaModel);

            Assert.Equal(1, result.Code);
            Assert.Equal("Update failed", result.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_CallsCorrectStoredProcedure()
        {
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                Status = false,
                Ip = "172.16.0.1",
                Language = "ko",
                UserId = 999,
                CustId = 888
            };
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            _db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");

            await _repository.UpdateStatusAsync(metaModel);

            _dbFactory.Verify(x => x.GetObjectDb("Lrf_AGC_UpdCustStatus", It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_SetsCorrectParameters()
        {
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                Status = false,
                Ip = "203.0.113.1",
                Language = "th",
                UserId = 555,
                CustId = 666
            };
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            _db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");

            await _repository.UpdateStatusAsync(metaModel);

            _db.Verify(x => x.SetParameterValue("isenabled", false), Times.Once);
            _db.Verify(x => x.SetParameterValue("ip", "203.0.113.1"), Times.Once);
            _db.Verify(x => x.SetParameterValue("language", "th"), Times.Once);
            _db.Verify(x => x.SetParameterValue("userid", 555), Times.Once);
            _db.Verify(x => x.SetParameterValue("custid", 666), Times.Once);
            _db.Verify(x => x.SetParameterValue("ErrCode", null), Times.Once);
            _db.Verify(x => x.SetParameterValue("ErrMsg", null), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_ReturnsErrorInfo()
        {
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                Status = true,
                Ip = "127.0.0.1",
                Language = "en",
                UserId = 1,
                CustId = 1
            };
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("ErrCode")).Returns("2");
            _db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("Status update failed");

            var result = await _repository.UpdateStatusAsync(metaModel);

            Assert.Equal(2, result.Code);
            Assert.Equal("Status update failed", result.Message);
        }

        [Fact]
        public async Task UpdateSettingAsync_HandlesNullErrorValues()
        {
            var metaModel = new PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>
            {
                Setting = new AgCasinoBetLimitMetaModel { Group = "TestGroup" },
                Ip = "127.0.0.1",
                Language = "en",
                UserId = 1,
                CustId = 1
            };
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("ErrCode")).Returns(null);
            _db.Setup(x => x.GetParameterValue("ErrMsg")).Returns(null);

            var result = await _repository.UpdateSettingAsync(metaModel);

            Assert.Equal(0, result.Code);
            Assert.Equal("", result.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_HandlesNullErrorValues()
        {
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                Status = true,
                Ip = "127.0.0.1",
                Language = "en",
                UserId = 1,
                CustId = 1
            };
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("ErrCode")).Returns(null);
            _db.Setup(x => x.GetParameterValue("ErrMsg")).Returns(null);

            var result = await _repository.UpdateStatusAsync(metaModel);

            Assert.Equal(0, result.Code);
            Assert.Equal("", result.Message);
        }

        [Fact]
        public async Task GetBetSettingAsync_HandlesNullValues()
        {
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("Currency")).Returns(null);
            _db.Setup(x => x.GetParameterValue("AGGroup")).Returns(null);
            _db.Setup(x => x.GetParameterValue("IsEnabled")).Returns(null);
            _db.Setup(x => x.GetParameterValue("IsLineEnabled")).Returns(null);
            _db.Setup(x => x.GetParameterValue("CheckedGroups")).Returns(null);

            var result = await _repository.GetBetSettingAsync(1);

            Assert.NotNull(result);
            Assert.Equal(0, result.CurrencyId);
            Assert.Equal("", result.AgGroup); // Convert.ToString(null) returns empty string, not null
            Assert.False(result.IsEnabled);
            Assert.False(result.IsLineEnabled);
            Assert.Equal(new[] { "" }, result.CheckedGroups);
        }

        [Fact]
        public async Task UpdateSettingAsync_HandlesEmptyGroup()
        {
            var metaModel = new PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>
            {
                Setting = new AgCasinoBetLimitMetaModel { Group = "" },
                Ip = "127.0.0.1",
                Language = "en",
                UserId = 1,
                CustId = 1
            };
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            _db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");

            var result = await _repository.UpdateSettingAsync(metaModel);

            _db.Verify(x => x.SetParameterValue("aggroup", ""), Times.Once);
            Assert.Equal(0, result.Code);
        }

        [Fact]
        public async Task UpdateSettingAsync_HandlesNullGroup()
        {
            var metaModel = new PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>
            {
                Setting = new AgCasinoBetLimitMetaModel { Group = null },
                Ip = "127.0.0.1",
                Language = "en",
                UserId = 1,
                CustId = 1
            };
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            _db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");

            var result = await _repository.UpdateSettingAsync(metaModel);

            _db.Verify(x => x.SetParameterValue("aggroup", null), Times.Once);
            Assert.Equal(0, result.Code);
        }

        [Fact]
        public async Task UpdateStatusAsync_HandlesEmptyStringValues()
        {
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                Status = true,
                Ip = "",
                Language = "",
                UserId = 1,
                CustId = 1
            };
            _db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            _db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");

            var result = await _repository.UpdateStatusAsync(metaModel);

            _db.Verify(x => x.SetParameterValue("ip", ""), Times.Once);
            _db.Verify(x => x.SetParameterValue("language", ""), Times.Once);
            Assert.Equal(0, result.Code);
        }
    }
}
