namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Repositories
{
    using System.Data;
    using System.Data.Common;
    using BetLimit.Repositories;
    using Core.Constants;
    using Core.Repositories;
    using Data;
    using MetaModel;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class DcsBetLimitRepositoryTests
    {
        [Fact]
        public async Task GetSettingAsync_ReturnsMappedData()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            reader.Setup(r => r["minbet"]).Returns(1m);
            reader.Setup(r => r["minbetmin"]).Returns(2m);
            reader.Setup(r => r["maxbet"]).Returns(3m);
            reader.Setup(r => r["maxbetmax"]).Returns(4m);
            reader.Setup(r => r["maxpermatch"]).Returns(5m);
            reader.Setup(r => r["maxpermatchmax"]).Returns(6m);
            reader.Setup(r => r["sporttype"]).Returns("220");
            reader.Setup(r => r["bettype"]).Returns(1021);
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(),It.IsAny<DbTransaction>(),It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.GetSettingAsync(1, ProductTypes.Lottery);
            var item = Assert.Single(result);
            Assert.Equal(1m, item.MinBet);
            Assert.Equal(2m, item.BetValueMin);
            Assert.Equal(3m, item.MaxBet);
            Assert.Equal(4m, item.BetValueMax);
            Assert.Equal(5m, item.MaxPerMatch);
            Assert.Equal(6m, item.MaxPerMatchMax);
            Assert.Equal("220", item.SportType);
            Assert.Equal(1021, item.BetType);
        }

        [Fact]
        public async Task GetStatusAsync_ReturnsTrueWhenEnabled()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            reader.Setup(r => r["IsDisabled"]).Returns(0);
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.GetStatusAsync(1, ProductTypes.Lottery);
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateSettingAsync_ReturnsMessageFromXml()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("1");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("<root Msg=\"Failed\" />");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.UpdateSettingAsync(new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> { ProductId = ProductTypes.Lottery, Setting = new[] { new PlayerBetLimitMetaModel { SportType = "220", BetType = 1021, MinBet = 1, MaxBet = 2, MaxPerMatch = 3 } } });
            Assert.Equal(1, result.Code);
            Assert.Equal("Failed", result.Message);
        }

        [Fact]
        public async Task UpdateSettingAsync_BuildsBetLimitXmlIncludingMatchingSportsOnly()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            string xml = null;
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.SetParameterValue("betlimit", It.IsAny<object>())).Callback<string, object>((_, v) => xml = v?.ToString());
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var setting = new[]
            {
                new PlayerBetLimitMetaModel { SportType = "220", BetType = 1021, MinBet = 1, MaxBet = 2, MaxPerMatch = 3 },
                new PlayerBetLimitMetaModel { SportType = "999", BetType = 1, MinBet = 1, MaxBet = 2, MaxPerMatch = 3 }
            };
            await repo.UpdateSettingAsync(new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> { ProductId = ProductTypes.Lottery, Setting = setting });
            Assert.Contains("SportType=\"220\"", xml);
            Assert.DoesNotContain("SportType=\"999\"", xml);
        }

        [Fact]
        public async Task UpdateStatusAsync_SetsCorrectXml()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            string xml = null;
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.SetParameterValue("IsDisabled", It.IsAny<object>())).Callback<string, object>((_, v) => xml = v?.ToString());
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            await repo.UpdateStatusAsync(new PlayerBetLimitUpdateStatusMetaModel { ProductId = ProductTypes.SLT, Status = true });
            Assert.Contains("SportType=\"169\"", xml);
            Assert.Contains("BetType=\"3611\"", xml);
            Assert.Contains("IsDisabled=\"0\"", xml);
        }

        [Fact]
        public async Task GetStatusAsync_ReturnsFalseWhenDisabled()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            reader.Setup(r => r["IsDisabled"]).Returns(1); // Disabled
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.GetStatusAsync(1, ProductTypes.Lottery);
            Assert.False(result);
        }

        [Fact]
        public async Task GetStatusAsync_ReturnsFalseWhenNoDataFound()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(false); // No data
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.GetStatusAsync(1, ProductTypes.Lottery);
            Assert.False(result);
        }

        [Fact]
        public async Task GetStatusAsync_SetsCorrectParameters()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(false);
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            await repo.GetStatusAsync(123, ProductTypes.VirtualSports);
            db.Verify(x => x.SetParameterValue("CustID", 123), Times.Once);
            db.Verify(x => x.SetParameterValue("ProductId", ProductTypes.VirtualSports), Times.Once);
            db.Verify(x => x.SetParameterValue("SettingType", 4), Times.Once);
            db.Verify(x => x.SetParameterValue("RoleID", 1), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_SetsCorrectParameters()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel 
            { 
                CustId = 123, 
                UserId = 456, 
                Ip = "127.0.0.1", 
                ProductId = ProductTypes.SLT, 
                Status = false 
            };
            await repo.UpdateStatusAsync(metaModel);
            db.Verify(x => x.SetParameterValue("custid", 123), Times.Once);
            db.Verify(x => x.SetParameterValue("userid", 456), Times.Once);
            db.Verify(x => x.SetParameterValue("ip", "127.0.0.1"), Times.Once);
            db.Verify(x => x.SetParameterValue("ErrCode", null), Times.Once);
            db.Verify(x => x.SetParameterValue("ErrMsg", null), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_SetsDisabledXmlForFalseStatus()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            string xml = null;
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.SetParameterValue("IsDisabled", It.IsAny<object>())).Callback<string, object>((_, v) => xml = v?.ToString());
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            await repo.UpdateStatusAsync(new PlayerBetLimitUpdateStatusMetaModel { ProductId = ProductTypes.CG, Status = false });
            Assert.Contains("SportType=\"167\"", xml);
            Assert.Contains("BetType=\"7301\"", xml);
            Assert.Contains("IsDisabled=\"1\"", xml); // 1 = disabled
        }

        [Fact]
        public async Task UpdateStatusAsync_ReturnsResultWithErrorInfo()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("1");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("Test error");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.UpdateStatusAsync(new PlayerBetLimitUpdateStatusMetaModel { ProductId = ProductTypes.CG, Status = true });
            Assert.Equal(1, result.Code);
            Assert.Equal("Test error", result.Message);
        }

        [Fact]
        public async Task GetSettingAsync_HandlesEmptyResult()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(false); // No rows
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.GetSettingAsync(1, ProductTypes.Lottery);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetSettingAsync_MapsAllPropertiesFromDataReader()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            reader.Setup(r => r["minbet"]).Returns(5.5m);
            reader.Setup(r => r["minbetmin"]).Returns(1.0m);
            reader.Setup(r => r["maxbet"]).Returns(1000.75m);
            reader.Setup(r => r["maxbetmax"]).Returns(2000.25m);
            reader.Setup(r => r["maxpermatch"]).Returns(5000.50m);
            reader.Setup(r => r["maxpermatchmax"]).Returns(10000.99m);
            reader.Setup(r => r["sporttype"]).Returns("999");
            reader.Setup(r => r["bettype"]).Returns(12345);
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.GetSettingAsync(1, ProductTypes.Lottery);
            var item = Assert.Single(result);
            Assert.Equal(5.5m, item.MinBet);
            Assert.Equal(1.0m, item.BetValueMin);
            Assert.Equal(1000.75m, item.MaxBet);
            Assert.Equal(2000.25m, item.BetValueMax);
            Assert.Equal(5000.50m, item.MaxPerMatch);
            Assert.Equal(10000.99m, item.MaxPerMatchMax);
            Assert.Equal("999", item.SportType);
            Assert.Equal(12345, item.BetType);
        }

        [Fact]
        public async Task GetSettingAsync_HandlesMultipleRows()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.SetupSequence(r => r.Read()).Returns(true).Returns(true).Returns(false);
            reader.SetupSequence(r => r["minbet"]).Returns(10m).Returns(20m);
            reader.SetupSequence(r => r["sporttype"]).Returns("220").Returns("221");
            reader.SetupSequence(r => r["bettype"]).Returns(1021).Returns(8001);
            reader.Setup(r => r["minbetmin"]).Returns(1m);
            reader.Setup(r => r["maxbet"]).Returns(100m);
            reader.Setup(r => r["maxbetmax"]).Returns(200m);
            reader.Setup(r => r["maxpermatch"]).Returns(500m);
            reader.Setup(r => r["maxpermatchmax"]).Returns(1000m);
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.GetSettingAsync(1, ProductTypes.Lottery);
            Assert.Equal(2, result.Count());
            var items = result.ToList();
            Assert.Equal(10m, items[0].MinBet);
            Assert.Equal(20m, items[1].MinBet);
            Assert.Equal("220", items[0].SportType);
            Assert.Equal("221", items[1].SportType);
            Assert.Equal(1021, items[0].BetType);
            Assert.Equal(8001, items[1].BetType);
        }

        [Theory]
        [InlineData(ProductTypes.VirtualSports, "9908")]
        [InlineData(ProductTypes.Lottery, "220")]
        [InlineData(ProductTypes.Sportsbooks2, "43")]
        [InlineData(ProductTypes.TG, "222")]
        [InlineData(ProductTypes.I4D, "223")]
        [InlineData(ProductTypes.SLT, "169")]
        [InlineData(ProductTypes.AP, "172")]
        [InlineData(ProductTypes.APM, "174")]
        [InlineData(ProductTypes.BTC, "175")]
        public async Task UpdateSettingAsync_FiltersSettingsBasedOnProductSportList(int productId, string allowedSport)
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            string xml = null;
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.SetParameterValue("betlimit", It.IsAny<object>())).Callback<string, object>((_, v) => xml = v?.ToString());
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var setting = new[]
            {
                new PlayerBetLimitMetaModel { SportType = allowedSport, BetType = 1, MinBet = 1, MaxBet = 2, MaxPerMatch = 3 },
                new PlayerBetLimitMetaModel { SportType = "999", BetType = 1, MinBet = 1, MaxBet = 2, MaxPerMatch = 3 }
            };
            await repo.UpdateSettingAsync(new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> { ProductId = productId, Setting = setting });
            Assert.Contains($"SportType=\"{allowedSport}\"", xml);
            Assert.DoesNotContain("SportType=\"999\"", xml);
        }

        [Fact]
        public async Task UpdateSettingAsync_IncludesAllSettingsWhenProductNotInSportList()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            string xml = null;
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.SetParameterValue("betlimit", It.IsAny<object>())).Callback<string, object>((_, v) => xml = v?.ToString());
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var setting = new[]
            {
                new PlayerBetLimitMetaModel { SportType = "100", BetType = 1, MinBet = 1, MaxBet = 2, MaxPerMatch = 3 },
                new PlayerBetLimitMetaModel { SportType = "200", BetType = 2, MinBet = 4, MaxBet = 5, MaxPerMatch = 6 }
            };
            // Use a product not in ProductSportList
            await repo.UpdateSettingAsync(new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> { ProductId = ProductTypes.CG, Setting = setting });
            Assert.Contains("SportType=\"100\"", xml);
            Assert.Contains("SportType=\"200\"", xml);
        }

        [Fact]
        public async Task UpdateStatusAsync_HandlesXmlErrorMessage()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("1");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("<root Msg=\"XML error message\" />");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.UpdateStatusAsync(new PlayerBetLimitUpdateStatusMetaModel { ProductId = ProductTypes.CG, Status = true });
            Assert.Equal(1, result.Code);
            Assert.Equal("XML error message", result.Message);
        }

        [Fact]
        public async Task UpdateStatusAsync_HandlesInvalidXmlErrorMessage()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var logger = new Mock<ILogger<DcsBetLimitRepository>>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("1");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("<invalid xml>");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsBetLimitRepository(dbFactory.Object, logger.Object);
            var result = await repo.UpdateStatusAsync(new PlayerBetLimitUpdateStatusMetaModel { ProductId = ProductTypes.CG, Status = true });
            Assert.Equal(1, result.Code);
            Assert.Equal("<invalid xml>", result.Message);
            logger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Invalid xml content:")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
