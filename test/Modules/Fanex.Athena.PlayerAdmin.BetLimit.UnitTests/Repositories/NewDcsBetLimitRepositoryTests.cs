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

    public class NewDcsBetLimitRepositoryTests
    {
        [Fact]
        public async Task GetSettingAsync_MapsFields()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.SetupSequence(r => r.Read()).Returns(true).Returns(false);
            reader.Setup(r => r["minbet"]).Returns(1m);
            reader.Setup(r => r["minbetmin"]).Returns(2m);
            reader.Setup(r => r["maxbet"]).Returns(3m);
            reader.Setup(r => r["maxbetmax"]).Returns(4m);
            reader.Setup(r => r["sporttype"]).Returns("231");
            reader.Setup(r => r["bettype"]).Returns(1);
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.GetSettingAsync(1, ProductTypes.VS3);
            var item = Assert.Single(result);
            Assert.Equal(1m, item.MinBet);
            Assert.Equal(2m, item.BetValueMin);
            Assert.Equal(3m, item.MaxBet);
            Assert.Equal(4m, item.BetValueMax);
            Assert.Equal("231", item.SportType);
            Assert.Equal(1, item.BetType);
        }

        [Fact]
        public async Task GetSettingAsync_ReturnsEmptyListWhenNoData()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(false);
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.GetSettingAsync(1, ProductTypes.VS3);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetSettingAsync_SetsCorrectParameters()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(false);
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            await repo.GetSettingAsync(123, ProductTypes.VS3);
            db.Verify(x => x.SetParameterValue("custid", 123), Times.Once);
            db.Verify(x => x.SetParameterValue("productid", ProductTypes.VS3), Times.Once);
        }

        [Fact]
        public async Task GetSettingAsync_HandlesMultipleRows()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.SetupSequence(r => r.Read()).Returns(true).Returns(true).Returns(false);
            reader.SetupSequence(r => r["minbet"]).Returns(1m).Returns(10m);
            reader.SetupSequence(r => r["maxbet"]).Returns(100m).Returns(1000m);
            reader.SetupSequence(r => r["sporttype"]).Returns("231").Returns("232");
            reader.SetupSequence(r => r["bettype"]).Returns(1).Returns(2);
            reader.Setup(r => r["minbetmin"]).Returns(0.5m);
            reader.Setup(r => r["maxbetmax"]).Returns(2000m);
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.GetSettingAsync(1, ProductTypes.VS3);
            Assert.Equal(2, result.Count());
            var items = result.ToList();
            Assert.Equal(1m, items[0].MinBet);
            Assert.Equal(10m, items[1].MinBet);
            Assert.Equal("231", items[0].SportType);
            Assert.Equal("232", items[1].SportType);
            Assert.Equal(1, items[0].BetType);
            Assert.Equal(2, items[1].BetType);
        }

        [Fact]
        public async Task UpdateSettingAsync_CallsCorrectStoredProcedure()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");
            dbFactory.Setup(x => x.GetObjectDb("Acc_Dep_CustSetting_NewBetLimitUpd", It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = 123,
                UserId = 456,
                Ip = "127.0.0.1",
                ProductId = ProductTypes.VS3,
                Setting = new List<PlayerBetLimitMetaModel>()
            };
            await repo.UpdateSettingAsync(metaModel);
            dbFactory.Verify(x => x.GetObjectDb("Acc_Dep_CustSetting_NewBetLimitUpd", It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_SetsCorrectParameters()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = 123,
                UserId = 456,
                Ip = "192.168.1.1",
                ProductId = ProductTypes.VS3,
                Setting = new List<PlayerBetLimitMetaModel>()
            };
            await repo.UpdateSettingAsync(metaModel);
            db.Verify(x => x.SetParameterValue("custid", 123), Times.Once);
            db.Verify(x => x.SetParameterValue("userid", 456), Times.Once);
            db.Verify(x => x.SetParameterValue("ip", "192.168.1.1"), Times.Once);
            db.Verify(x => x.SetParameterValue("errcode", null), Times.Once);
            db.Verify(x => x.SetParameterValue("errmsg", null), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_ReturnsResultInfoWithErrorCode()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("1");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("Test error message");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = ProductTypes.VS3,
                Setting = new List<PlayerBetLimitMetaModel>()
            };
            var result = await repo.UpdateSettingAsync(metaModel);
            Assert.Equal(1, result.Code);
            Assert.Equal("Test error message", result.Message);
        }

        [Fact]
        public async Task UpdateSettingAsync_FiltersToAllowedSports()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            string xml = null;
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
            db.Setup(x => x.SetParameterValue("betlimit", It.IsAny<object>())).Callback<string, object>((_, v) => xml = v?.ToString());
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var settings = new[]
            {
                new PlayerBetLimitMetaModel { SportType = "231", BetType = 1, MinBet = 1, MaxBet = 2 },
                new PlayerBetLimitMetaModel { SportType = "999", BetType = 1, MinBet = 1, MaxBet = 2 }
            };
            await repo.UpdateSettingAsync(new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> { ProductId = ProductTypes.VS3, Setting = settings });
            Assert.Contains("SportType=\"231\"", xml);
            Assert.DoesNotContain("SportType=\"999\"", xml);
        }

        [Fact]
        public async Task UpdateSettingAsync_BuildsCorrectXml()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            string capturedXml = null;
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.SetParameterValue("betlimit", It.IsAny<object>()))
                .Callback<string, object>((_, value) => capturedXml = value?.ToString());
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = ProductTypes.VS3,
                Setting = new List<PlayerBetLimitMetaModel>
                {
                    new PlayerBetLimitMetaModel
                    {
                        SportType = "231",
                        BetType = 1,
                        MinBet = 5.0m,
                        MaxBet = 500.0m
                    }
                }
            };
            await repo.UpdateSettingAsync(metaModel);
            Assert.NotNull(capturedXml);
            Assert.Contains("<ROOT>", capturedXml);
            Assert.Contains("</ROOT>", capturedXml);
            Assert.Contains("SportType=\"231\"", capturedXml);
            Assert.Contains("BetType=\"1\"", capturedXml);
            Assert.Contains("MinBet=\"5.0\"", capturedXml);
            Assert.Contains("MaxBet=\"500.0\"", capturedXml);
        }

        [Fact]
        public async Task UpdateSettingAsync_HandlesEmptySettingsCollection()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            string capturedXml = null;
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.SetParameterValue("betlimit", It.IsAny<object>()))
                .Callback<string, object>((_, value) => capturedXml = value?.ToString());
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = ProductTypes.VS3,
                Setting = new List<PlayerBetLimitMetaModel>() // Empty list
            };
            await repo.UpdateSettingAsync(metaModel);
            Assert.Equal("<ROOT></ROOT>", capturedXml);
        }

        [Fact]
        public async Task UpdateSettingAsync_HandlesMultipleSettings()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            string capturedXml = null;
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.SetParameterValue("betlimit", It.IsAny<object>()))
                .Callback<string, object>((_, value) => capturedXml = value?.ToString());
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = ProductTypes.VS3,
                Setting = new List<PlayerBetLimitMetaModel>
                {
                    new PlayerBetLimitMetaModel
                    {
                        SportType = "231",
                        BetType = 1,
                        MinBet = 5.0m,
                        MaxBet = 500.0m
                    },
                    new PlayerBetLimitMetaModel
                    {
                        SportType = "231",
                        BetType = 2,
                        MinBet = 10.0m,
                        MaxBet = 1000.0m
                    }
                }
            };
            await repo.UpdateSettingAsync(metaModel);
            Assert.NotNull(capturedXml);
            Assert.Contains("BetType=\"1\"", capturedXml);
            Assert.Contains("BetType=\"2\"", capturedXml);
            Assert.Contains("MinBet=\"5.0\"", capturedXml);
            Assert.Contains("MinBet=\"10.0\"", capturedXml);
        }

        [Fact]
        public async Task UpdateSettingAsync_ParsesXmlErrorMessage()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("1");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("<root Msg=\"Custom error message\" />");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = ProductTypes.VS3,
                Setting = new List<PlayerBetLimitMetaModel>()
            };
            var result = await repo.UpdateSettingAsync(metaModel);
            Assert.Equal("Custom error message", result.Message);
        }

        [Fact]
        public async Task UpdateSettingAsync_ReturnsOriginalMessageWhenXmlParsingFails()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var logger = new Mock<ILogger<DcsBetLimitRepository>>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("1");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("<invalid xml>");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, logger.Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = ProductTypes.VS3,
                Setting = new List<PlayerBetLimitMetaModel>()
            };
            var result = await repo.UpdateSettingAsync(metaModel);
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

        [Fact]
        public async Task UpdateSettingAsync_ReturnsOriginalMessageWhenNotXml()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("1");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("Plain text error message");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = ProductTypes.VS3,
                Setting = new List<PlayerBetLimitMetaModel>()
            };
            var result = await repo.UpdateSettingAsync(metaModel);
            Assert.Equal("Plain text error message", result.Message);
        }

        [Fact]
        public async Task UpdateSettingAsync_HandlesNullAndEmptyErrorMessages()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = ProductTypes.VS3,
                Setting = new List<PlayerBetLimitMetaModel>()
            };
            var result = await repo.UpdateSettingAsync(metaModel);
            Assert.Equal("", result.Message);
        }

        [Fact]
        public async Task UpdateSettingAsync_ThrowsNullReferenceExceptionForUnsupportedProduct()
        {
            var dbFactory = new Mock<IDbFactory>();
            var repo = new NewDcsBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = 999, // Unsupported product
                Setting = new List<PlayerBetLimitMetaModel>()
            };
            await Assert.ThrowsAsync<NullReferenceException>(() => repo.UpdateSettingAsync(metaModel));
        }
    }
}
