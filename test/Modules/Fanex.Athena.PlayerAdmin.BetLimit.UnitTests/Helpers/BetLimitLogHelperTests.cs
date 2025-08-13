namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Helpers
{
    using AutoFixture;
    using BetLimit.Helpers;
    using Core.Constants;
    using DataDomains;
    using MetaModel;

    public class BetLimitLogHelperTests
    {
        private readonly IFixture _fixture;

        public BetLimitLogHelperTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void CreateLogEvent_ShouldReturnEvent_WhenCalledWithPlayerBetLimitMetaModel()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            var logDetailIds = new[] { LogActionDetailId.UpdatingMinBet, LogActionDetailId.UpdatingMaxBet };


            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.CreateLogEvent(metaModel, getSportTypeFunc, true, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(metaModel.CustId, result.CustID);
            Assert.Equal(metaModel.UserId, result.UserId);
            Assert.Equal(metaModel.SiteName, result.SiteName);
            Assert.Equal(metaModel.LicenseeCustId, result.LicenseeCustId);
            Assert.NotNull(result.Detail);
        }

        [Fact]
        public void CreateLogEvent_ShouldReturnEvent_WhenCalledWithAgCasinoBetLimitMetaModel()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>>();

            // Act
            var result = BetLimitLogHelper.CreateLogEvent(metaModel, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(metaModel.CustId, result.CustID);
            Assert.Equal(metaModel.UserId, result.UserId);
            Assert.Equal(metaModel.SiteName, result.SiteName);
            Assert.Equal(metaModel.LicenseeCustId, result.LicenseeCustId);
            Assert.NotNull(result.Detail);
            Assert.Single(result.Detail);
        }

        [Fact]
        public void CreateLogEvent_ShouldReturnNull_WhenNoDetailProvided()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>>();
            var emptyDetail = Enumerable.Empty<PlayerBetLimitDetailedLog>();

            // Act
            var result = BetLimitLogHelper.CreateLogEvent(metaModel, emptyDetail);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void BuildBetLimitLogDetail_ShouldReturnDetails_WhenItemsAreUpdated()
        {
            // Arrange
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = "1_1",
                    SportType = "1",

                    BetType = 1,

                    IsUpdated = true,
                    MinBet = 10,
                    OldMinBet = 5,
                    MaxBet = 100,
                    OldMaxBet = 50
                }
            };
            var logDetailIds = new[] { LogActionDetailId.UpdatingMinBet, LogActionDetailId.UpdatingMaxBet };


            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.BuildBetLimitLogDetail(settings, getSportTypeFunc, true, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // One for MinBet, one for MaxBet

            var minBetLog = result.FirstOrDefault(x => x.DetailedId == LogActionDetailId.UpdatingMinBet);
            Assert.NotNull(minBetLog);
            Assert.Equal("1", minBetLog.SportType);
            Assert.Equal(BetLimitLogHelper.Success, minBetLog.Status);
            Assert.Equal("5", minBetLog.Old);
            Assert.Equal("10", minBetLog.New);
        }

        [Fact]
        public void BuildBetLimitLogDetail_ShouldReturnEmptyDetails_WhenNoItemsAreUpdated()
        {
            // Arrange
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = "1_1",
                    SportType = "1",

                    BetType = 1,

                    IsUpdated = false
                }
            };
            var logDetailIds = new[] { LogActionDetailId.UpdatingMinBet };


            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.BuildBetLimitLogDetail(settings, getSportTypeFunc, true, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void BuildBetLimitLogDetail_ShouldSetStatusToFalse_WhenNotSuccessful()
        {
            // Arrange
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = "1_1",
                    SportType = "1",

                    BetType = 1,

                    IsUpdated = true,
                    MinBet = 10,
                    OldMinBet = 5
                }
            };
            var logDetailIds = new[] { LogActionDetailId.UpdatingMinBet };


            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.BuildBetLimitLogDetail(settings, getSportTypeFunc, false, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            Assert.Equal(BetLimitLogHelper.False, result.First().Status);
        }

        [Theory]
        [InlineData("1_1", "1")]
        [InlineData("2_3", "2")]
        [InlineData("5", "5")]
        public void BuildBetLimitLogDetail_ShouldGenerateCorrectSportId_WhenDifferentFormats(string inputId, string expectedSportId)
        {
            // Arrange
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = inputId,
                    SportType = "1",

                    BetType = 1,

                    IsUpdated = true,
                    MinBet = 10,
                    OldMinBet = 5
                }
            };
            var logDetailIds = new[] { LogActionDetailId.UpdatingMinBet };


            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.BuildBetLimitLogDetail(settings, getSportTypeFunc, true, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            Assert.Equal(expectedSportId, result.First().SportType);
        }

        [Theory]
        [InlineData(SportType.Soccer, BetType.Parlay, SportTypeExtend.SoccerParlay)]
        [InlineData(SportType.OtherSports, BetType.Parlay, SportTypeExtend.MixSportsParlay)]
        [InlineData(SportType.BasketBall, BetType.Parlay, "2")]
        public void BuildBetLimitLogDetail_ShouldGenerateCorrectSportIdForParlay_WhenParlayBetType(int sportType, int betType, string expectedSportId)
        {
            // Arrange
            var inputId = $"{sportType}_{betType}";
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = inputId,
                    SportType = sportType.ToString(),

                    BetType = betType,

                    IsUpdated = true,
                    MinBet = 10,
                    OldMinBet = 5
                }
            };
            var logDetailIds = new[] { LogActionDetailId.UpdatingMinBet };


            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.BuildBetLimitLogDetail(settings, getSportTypeFunc, true, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            Assert.Equal(expectedSportId, result.First().SportType);
        }

        [Fact]
        public void IsMaxPayoutPerMatch_ShouldReturnTrue_WhenMaxPayoutPerMatchChanged()
        {
            // Arrange
            var item = new PlayerBetLimitMetaModel
            {
                MaxPayoutPerMatch = 1000,
                OldMaxPayoutPerMatch = 500
            };

            // Act
            var result = BetLimitLogHelper.IsMaxPayoutPerMatch(LogActionDetailId.UpdatingMaxPayoutPerMatch, item);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsMaxPayoutPerMatch_ShouldReturnFalse_WhenMaxPayoutPerMatchNotChanged()
        {
            // Arrange
            var item = new PlayerBetLimitMetaModel
            {
                MaxPayoutPerMatch = 1000,
                OldMaxPayoutPerMatch = 1000
            };

            // Act
            var result = BetLimitLogHelper.IsMaxPayoutPerMatch(LogActionDetailId.UpdatingMaxPayoutPerMatch, item);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsMaxPayoutPerMatch_ShouldReturnFalse_WhenWrongLogDetailId()
        {
            // Arrange
            var item = new PlayerBetLimitMetaModel
            {
                MaxPayoutPerMatch = 1000,
                OldMaxPayoutPerMatch = 500
            };

            // Act
            var result = BetLimitLogHelper.IsMaxPayoutPerMatch(LogActionDetailId.UpdatingMinBet, item);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void BuildBetLimitLogDetail_ShouldHandleNullGetSportTypeFunc_WhenCalled()
        {
            // Arrange
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = "1_1",
                    SportType = "1",
                    BetType = 1,
                    IsUpdated = true,
                    MinBet = 10,
                    OldMinBet = 5
                }
            };
            var logDetailIds = new[] { LogActionDetailId.UpdatingMinBet };

            // Act & Assert - Should throw NullReferenceException when getSportTypeFunc is null
            Assert.Throws<NullReferenceException>(() => 
                BetLimitLogHelper.BuildBetLimitLogDetail(settings, null, true, logDetailIds).ToList());
        }

        [Fact]
        public void BuildBetLimitLogDetail_ShouldReturnEmptyCollection_WhenEmptyLogDetailIds()
        {
            // Arrange
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = "1_1",
                    SportType = "1",
                    BetType = 1,
                    IsUpdated = true,
                    MinBet = 10,
                    OldMinBet = 5
                }
            };
            var logDetailIds = Array.Empty<int>();
            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.BuildBetLimitLogDetail(settings, getSportTypeFunc, true, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void BuildBetLimitLogDetail_ShouldReturnEmptyCollection_WhenEmptySettings()
        {
            // Arrange
            var settings = new List<PlayerBetLimitMetaModel>();
            var logDetailIds = new[] { LogActionDetailId.UpdatingMinBet };
            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.BuildBetLimitLogDetail(settings, getSportTypeFunc, true, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void BuildBetLimitLogDetail_ShouldHandleMaxPerBallAction_WhenMaxPerBallChanged()
        {
            // Arrange
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = "1_1",
                    SportType = "1",
                    BetType = 1,
                    IsUpdated = true,
                    MaxPerBall = 500,
                    OldMaxPerBall = 250
                }
            };
            var logDetailIds = new[] { LogActionDetailId.UpdatingMaxPerBall };
            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.BuildBetLimitLogDetail(settings, getSportTypeFunc, true, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var log = result.First();
            Assert.Equal(LogActionDetailId.UpdatingMaxPerBall, log.DetailedId);
            Assert.Equal("1", log.SportType);
            Assert.Equal(BetLimitLogHelper.Success, log.Status);
            Assert.Equal("250", log.Old);
            Assert.Equal("500", log.New);
        }

        [Fact]
        public void BuildBetLimitLogDetail_ShouldHandleMaxBetPerDrawAction_WhenMaxPerMatchChanged()
        {
            // Arrange
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = "1_1",
                    SportType = "1",
                    BetType = 1,
                    IsUpdated = true,
                    MaxPerMatch = 1000,
                    OldMaxPerMatch = 750
                }
            };
            var logDetailIds = new[] { LogActionDetailId.UpdatingMaxBetPerDraw };
            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.BuildBetLimitLogDetail(settings, getSportTypeFunc, true, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var log = result.First();
            Assert.Equal(LogActionDetailId.UpdatingMaxBetPerDraw, log.DetailedId);
            Assert.Equal("1", log.SportType);
            Assert.Equal(BetLimitLogHelper.Success, log.Status);
            Assert.Equal("750", log.Old);
            Assert.Equal("1000", log.New);
        }

        [Fact]
        public void BuildBetLimitLogDetail_ShouldReturnNull_WhenInvalidLogActionDetailId()
        {
            // Arrange
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = "1_1",
                    SportType = "1",
                    BetType = 1,
                    IsUpdated = true,
                    MinBet = 10,
                    OldMinBet = 5
                }
            };
            var logDetailIds = new[] { 999 }; // Invalid LogActionDetailId
            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.BuildBetLimitLogDetail(settings, getSportTypeFunc, true, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result); // Should be empty because CreateLogDetailItemByAction returns null
        }

        [Fact]
        public void BuildBetLimitLogDetail_ShouldSkipItems_WhenValuesUnchanged()
        {
            // Arrange
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = "1_1",
                    SportType = "1",
                    BetType = 1,
                    IsUpdated = true,
                    MinBet = 10,
                    OldMinBet = 10, // Same value - should be skipped
                    MaxBet = 100,
                    OldMaxBet = 100, // Same value - should be skipped
                    MaxPerMatch = 500,
                    OldMaxPerMatch = 500, // Same value - should be skipped
                    MaxPerBall = 250,
                    OldMaxPerBall = 250, // Same value - should be skipped
                    MaxPayoutPerMatch = 1000,
                    OldMaxPayoutPerMatch = 1000 // Same value - should be skipped
                }
            };
            var logDetailIds = new[] 
            { 
                LogActionDetailId.UpdatingMinBet,
                LogActionDetailId.UpdatingMaxBet,
                LogActionDetailId.UpdatingMaxPerMatch,
                LogActionDetailId.UpdatingMaxPerBall,
                LogActionDetailId.UpdatingMaxPayoutPerMatch
            };
            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.BuildBetLimitLogDetail(settings, getSportTypeFunc, true, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result); // Should be empty because no values actually changed
        }

        [Fact]
        public void BuildBetLimitLogDetail_ShouldHandleMixedUpdatedAndUnchangedItems_WhenCalled()
        {
            // Arrange
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = "1_1",
                    SportType = "1",
                    BetType = 1,
                    IsUpdated = true,
                    MinBet = 10,
                    OldMinBet = 5, // Changed
                    MaxBet = 100,
                    OldMaxBet = 100 // Unchanged
                },
                new()
                {
                    Id = "2_1",
                    SportType = "2",
                    BetType = 1,
                    IsUpdated = false, // Not updated - should be skipped entirely
                    MinBet = 20,
                    OldMinBet = 15
                }
            };
            var logDetailIds = new[] { LogActionDetailId.UpdatingMinBet, LogActionDetailId.UpdatingMaxBet };
            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.BuildBetLimitLogDetail(settings, getSportTypeFunc, true, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Only one log entry - MinBet change for first item
            var log = result.First();
            Assert.Equal(LogActionDetailId.UpdatingMinBet, log.DetailedId);
            Assert.Equal("1", log.SportType);
            Assert.Equal(BetLimitLogHelper.Success, log.Status);
            Assert.Equal("5", log.Old);
            Assert.Equal("10", log.New);
        }

        [Theory]
        [InlineData("invalid_format")]
        [InlineData("abc_def")]
        public void BuildBetLimitLogDetail_ShouldHandleInvalidSportIdFormats_WhenCalled(string invalidId)
        {
            // Arrange
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = invalidId,
                    SportType = "1",
                    BetType = 1,
                    IsUpdated = true,
                    MinBet = 10,
                    OldMinBet = 5
                }
            };
            var logDetailIds = new[] { LogActionDetailId.UpdatingMinBet };
            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => BetLimitLogHelper.BuildBetLimitLogDetail(settings, getSportTypeFunc, true, logDetailIds).ToList());
        }

        [Fact]
        public void CreateLogEvent_ShouldReturnEvent_WhenAgCasinoWithFailure()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>>();

            // Act
            var result = BetLimitLogHelper.CreateLogEvent(metaModel, false);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(metaModel.CustId, result.CustID);
            Assert.Equal(metaModel.UserId, result.UserId);
            Assert.Equal(metaModel.SiteName, result.SiteName);
            Assert.Equal(metaModel.LicenseeCustId, result.LicenseeCustId);
            Assert.NotNull(result.Detail);
            Assert.Single(result.Detail);
            
            var detail = result.Detail.First();
            Assert.Equal(LogActionDetailId.UpdatingAGGroup, detail.DetailedId);
            Assert.Equal(SportType.AgCasino.ToString(), detail.SportType);
            Assert.Equal(BetLimitLogHelper.False, detail.Status);
            Assert.Equal(metaModel.Setting.OldGroup, detail.Old);
            Assert.Equal(metaModel.Setting.Group, detail.New);
        }

        [Fact]
        public void CreateLogEvent_ShouldHandleDecimalFormatting_WhenCreatingLogDetails()
        {
            // Arrange
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = "1_1",
                    SportType = "1",
                    BetType = 1,
                    IsUpdated = true,
                    MinBet = 10.50m,
                    OldMinBet = 5.25m,
                    MaxBet = 1000.75m,
                    OldMaxBet = 500.125m
                }
            };
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                Setting = settings,
                CustId = 123,
                UserId = 456,
                SiteName = "TestSite",
                LicenseeCustId = 789
            };
            var logDetailIds = new[] { LogActionDetailId.UpdatingMinBet, LogActionDetailId.UpdatingMaxBet };
            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.CreateLogEvent(metaModel, getSportTypeFunc, true, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Detail);
            Assert.Equal(2, result.Detail.Count());

            var minBetLog = result.Detail.FirstOrDefault(x => x.DetailedId == LogActionDetailId.UpdatingMinBet);
            Assert.NotNull(minBetLog);
            Assert.Equal("5.25", minBetLog.Old);
            Assert.Equal("10.50", minBetLog.New);

            var maxBetLog = result.Detail.FirstOrDefault(x => x.DetailedId == LogActionDetailId.UpdatingMaxBet);
            Assert.NotNull(maxBetLog);
            Assert.Equal("500.125", maxBetLog.Old);
            Assert.Equal("1000.75", maxBetLog.New);
        }

        [Theory]
        [InlineData(1, 9, "1MP")] // Soccer Parlay
        [InlineData(99, 9, "99MP")] // Other Sports Parlay
        [InlineData(1, 1, "1")] // Soccer Default
        [InlineData(2, 9, "2")] // Basketball Parlay (non-soccer/other)
        [InlineData(3, 1, "3")] // Football Default
        public void BuildBetLimitLogDetail_ShouldGenerateCorrectSportIdForComplexFormats_WhenCalled(
            int sportType, int betType, string expectedSportId)
        {
            // Arrange
            var inputId = $"{sportType}_{betType}";
            var settings = new List<PlayerBetLimitMetaModel>
            {
                new()
                {
                    Id = inputId,
                    SportType = sportType.ToString(),
                    BetType = betType,
                    IsUpdated = true,
                    MinBet = 10,
                    OldMinBet = 5
                }
            };
            var logDetailIds = new[] { LogActionDetailId.UpdatingMinBet };
            string getSportTypeFunc(PlayerBetLimitMetaModel m) => m.Id;

            // Act
            var result = BetLimitLogHelper.BuildBetLimitLogDetail(settings, getSportTypeFunc, true, logDetailIds);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(expectedSportId, result.First().SportType);
        }
    }
}