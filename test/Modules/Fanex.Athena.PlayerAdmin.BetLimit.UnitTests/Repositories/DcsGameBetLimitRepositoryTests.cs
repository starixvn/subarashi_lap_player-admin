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

    public class DcsGameBetLimitRepositoryTests
    {
        [Fact]
        public async Task GetSettingAsync_MapsAllFields_ForSportsbook()
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
            reader.Setup(r => r["maxpayoutpermatch"]).Returns(7m);
            reader.Setup(r => r["maxpayoutpermatchmax"]).Returns(8m);
            reader.Setup(r => r["maxpernumber"]).Returns(0m);
            reader.Setup(r => r["maxpernumbermax"]).Returns(0m);
            reader.Setup(r => r["sporttype"]).Returns("43");
            reader.Setup(r => r["bettype"]).Returns(1611);
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(),It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(),It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.GetSettingAsync(1, ProductTypes.Sportsbook);
            var item = Assert.Single(result);
            Assert.Equal(1m, item.MinBet);
            Assert.Equal(2m, item.BetValueMin);
            Assert.Equal(3m, item.MaxBet);
            Assert.Equal(4m, item.BetValueMax);
            Assert.Equal(5m, item.MaxPerMatch);
            Assert.Equal(6m, item.MaxPerMatchMax);
            Assert.Equal(7m, item.MaxPayoutPerMatch);
            Assert.Equal(8m, item.MaxPayoutPerMatchMax);
            Assert.Equal("43", item.SportType);
            Assert.Equal(1611, item.BetType);
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
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.GetSettingAsync(1, ProductTypes.Sportsbook);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetSettingAsync_MapsCorrectProductForSportsbook()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(false);
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            await repo.GetSettingAsync(123, ProductTypes.Sportsbook);
            db.Verify(x => x.SetParameterValue("custid", 123), Times.Once);
            db.Verify(x => x.SetParameterValue("productid", 1), Times.Once);
            db.Verify(x => x.SetParameterValue("settingtype", 10), Times.Once);
        }

        [Fact]
        public async Task GetSettingAsync_MapsCorrectProductForNumberGame()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.Setup(r => r.Read()).Returns(false);
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            await repo.GetSettingAsync(123, ProductTypes.NumberGame);
            db.Verify(x => x.SetParameterValue("custid", 123), Times.Once);
            db.Verify(x => x.SetParameterValue("productid", 3), Times.Once);
            db.Verify(x => x.SetParameterValue("settingtype", 12), Times.Once);
        }

        [Fact]
        public async Task GetSettingAsync_HandlesMultipleRows()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var reader = new Mock<IDataReader>();
            reader.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(true)
                .Returns(false);
            reader.SetupSequence(r => r["minbet"])
                .Returns(1.00m)
                .Returns(2.00m);
            reader.SetupSequence(r => r["maxbet"])
                .Returns(100.00m)
                .Returns(200.00m);
            reader.SetupSequence(r => r["sporttype"])
                .Returns("1")
                .Returns("2");
            reader.SetupSequence(r => r["bettype"])
                .Returns(1)
                .Returns(2);
            reader.Setup(r => r["minbetmin"]).Returns(0.50m);
            reader.Setup(r => r["maxbetmax"]).Returns(200.00m);
            reader.Setup(r => r["maxpermatch"]).Returns(500.00m);
            reader.Setup(r => r["maxpermatchmax"]).Returns(1000.00m);
            reader.Setup(r => r["maxpayoutpermatch"]).Returns(2000.00m);
            reader.Setup(r => r["maxpayoutpermatchmax"]).Returns(3000.00m);
            reader.Setup(r => r["maxpernumber"]).Returns(50.00m);
            reader.Setup(r => r["maxpernumbermax"]).Returns(100.00m);
            db.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(reader.Object);
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.GetSettingAsync(1, ProductTypes.Sportsbook);
            Assert.Equal(2, result.Count());
            var firstItem = result.First();
            var secondItem = result.Skip(1).First();
            Assert.Equal(1.00m, firstItem.MinBet);
            Assert.Equal(2.00m, secondItem.MinBet);
            Assert.Equal("1", firstItem.SportType);
            Assert.Equal("2", secondItem.SportType);
        }

        [Fact]
        public async Task GetSettingAsync_ThrowsKeyNotFoundExceptionForUnsupportedProduct()
        {
            var dbFactory = new Mock<IDbFactory>();
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => repo.GetSettingAsync(1, 999) // Using unsupported product ID
            );
        }

        [Fact]
        public async Task UpdateSettingAsync_CallsCorrectStoredProcedureWithCorrectParameters()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = 123,
                UserId = 456,
                Ip = "127.0.0.1",
                ProductId = ProductTypes.Sportsbook,
                Setting = new List<PlayerBetLimitMetaModel>
                {
                    new PlayerBetLimitMetaModel
                    {
                        SportType = "1",
                        BetType = 1,
                        MinBet = 1.0m,
                        MaxBet = 100.0m,
                        MaxPerMatch = 500.0m,
                        MaxPayoutPerMatch = 2000.0m
                    }
                }
            };
            await repo.UpdateSettingAsync(metaModel);
            db.Verify(x => x.SetParameterValue("custid", 123), Times.Once);
            db.Verify(x => x.SetParameterValue("userid", 456), Times.Once);
            db.Verify(x => x.SetParameterValue("ip", "127.0.0.1"), Times.Once);
            db.Verify(x => x.SetParameterValue("errcode", null), Times.Once);
            db.Verify(x => x.SetParameterValue("errmsg", null), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_ReturnsResultInfoWithCorrectErrorCode()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("1");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("Test error message");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = 123,
                ProductId = ProductTypes.Sportsbook,
                Setting = new List<PlayerBetLimitMetaModel>()
            };
            var result = await repo.UpdateSettingAsync(metaModel);
            Assert.Equal(1, result.Code);
            Assert.Equal("Test error message", result.Message);
        }

        [Fact]
        public async Task UpdateSettingAsync_BuildsSportsbookXmlCorrectly()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            string capturedXml = null;
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.SetParameterValue("betlimit", It.IsAny<object>()))
                .Callback<string, object>((_, value) => capturedXml = value?.ToString());
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = ProductTypes.Sportsbook,
                Setting = new List<PlayerBetLimitMetaModel>
                {
                    new PlayerBetLimitMetaModel
                    {
                        SportType = "1",
                        BetType = 1,
                        MinBet = 10.0m,
                        MaxBet = 1000.0m,
                        MaxPerMatch = 5000.0m,
                        MaxPayoutPerMatch = 10000.0m
                    }
                }
            };
            await repo.UpdateSettingAsync(metaModel);
            Assert.NotNull(capturedXml);
            Assert.Contains("<ROOT>", capturedXml);
            Assert.Contains("</ROOT>", capturedXml);
            Assert.Contains("SportType=\"1\"", capturedXml);
            Assert.Contains("BetType=\"1\"", capturedXml);
            Assert.Contains("MinBet=\"10.0\"", capturedXml);
            Assert.Contains("MaxBet=\"1000.0\"", capturedXml);
            Assert.Contains("MaxPerMatch=\"5000.0\"", capturedXml);
            Assert.Contains("MaxPayoutPerMatch=\"10000.0\"", capturedXml);
        }

        [Fact]
        public async Task UpdateSettingAsync_BuildsNumberGameXmlCorrectly()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            string capturedXml = null;
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.SetParameterValue("betlimit", It.IsAny<object>()))
                .Callback<string, object>((_, value) => capturedXml = value?.ToString());
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = ProductTypes.NumberGame,
                Setting = new List<PlayerBetLimitMetaModel>
                {
                    new PlayerBetLimitMetaModel
                    {
                        SportType = "1",
                        BetType = 1,
                        MinBet = 5.0m,
                        MaxBet = 500.0m,
                        MaxPerMatch = 2500.0m,
                        MaxPerBall = 100.0m
                    }
                }
            };
            await repo.UpdateSettingAsync(metaModel);
            Assert.NotNull(capturedXml);
            Assert.Contains("<ROOT>", capturedXml);
            Assert.Contains("</ROOT>", capturedXml);
            Assert.Contains("SportType=\"1\"", capturedXml);
            Assert.Contains("BetType=\"1\"", capturedXml);
            Assert.Contains("MinBet=\"5.0\"", capturedXml);
            Assert.Contains("MaxBet=\"500.0\"", capturedXml);
            Assert.Contains("MaxPerMatch=\"2500.0\"", capturedXml);
            Assert.Contains("MaxPerNumber=\"100.0\"", capturedXml);
            Assert.DoesNotContain("MaxPayoutPerMatch", capturedXml);
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
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = ProductTypes.Sportsbook,
                Setting = new List<PlayerBetLimitMetaModel>
                {
                    new PlayerBetLimitMetaModel
                    {
                        SportType = "1",
                        BetType = 1,
                        MinBet = 10.0m,
                        MaxBet = 1000.0m,
                        MaxPerMatch = 5000.0m,
                        MaxPayoutPerMatch = 10000.0m
                    },
                    new PlayerBetLimitMetaModel
                    {
                        SportType = "2",
                        BetType = 2,
                        MinBet = 20.0m,
                        MaxBet = 2000.0m,
                        MaxPerMatch = 10000.0m,
                        MaxPayoutPerMatch = 20000.0m
                    }
                }
            };
            await repo.UpdateSettingAsync(metaModel);
            Assert.NotNull(capturedXml);
            Assert.Contains("SportType=\"1\"", capturedXml);
            Assert.Contains("SportType=\"2\"", capturedXml);
            Assert.Contains("BetType=\"1\"", capturedXml);
            Assert.Contains("BetType=\"2\"", capturedXml);
        }

        [Fact]
        public async Task UpdateSettingAsync_ParsesXmlErrorMessage()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("2");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("<root Msg=\"Bad\" />");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var result = await repo.UpdateSettingAsync(new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> { ProductId = ProductTypes.Sportsbook, Setting = new[] { new PlayerBetLimitMetaModel { SportType = "43", BetType = 1611, MinBet = 1, MaxBet = 2, MaxPerMatch = 3, MaxPayoutPerMatch = 4 } } });
            Assert.Equal(2, result.Code);
            Assert.Equal("Bad", result.Message);
        }

        [Fact]
        public async Task UpdateSettingAsync_ReturnsOriginalMessageWhenXmlParsingFails()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            var logger = new Mock<ILogger<DcsBetLimitRepository>>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("1");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("<invalid xml");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, logger.Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = ProductTypes.Sportsbook,
                Setting = new List<PlayerBetLimitMetaModel>()
            };
            var result = await repo.UpdateSettingAsync(metaModel);
            Assert.Equal("<invalid xml", result.Message);
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
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = ProductTypes.Sportsbook,
                Setting = new List<PlayerBetLimitMetaModel>()
            };
            var result = await repo.UpdateSettingAsync(metaModel);
            Assert.Equal("Plain text error message", result.Message);
        }

        [Fact]
        public async Task UpdateSettingAsync_HandlesEmptyErrorMessage()
        {
            var dbFactory = new Mock<IDbFactory>();
            var db = new Mock<IObjectDb>();
            db.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            db.Setup(x => x.GetParameterValue("ErrCode")).Returns("0");
            db.Setup(x => x.GetParameterValue("ErrMsg")).Returns("");
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(db.Object);
            var repo = new DcsGameBetLimitRepository(dbFactory.Object, new Mock<ILogger<DcsBetLimitRepository>>().Object);
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                ProductId = ProductTypes.Sportsbook,
                Setting = new List<PlayerBetLimitMetaModel>()
            };
            var result = await repo.UpdateSettingAsync(metaModel);
            Assert.Equal("", result.Message);
        }
    }
}
