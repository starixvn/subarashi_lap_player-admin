namespace Fanex.Athena.PlayerAdmin.API.UnitTests.ViewModels
{
    using API.DTO;
    using API.ViewModels;
    using Xunit;

    public class PlayerListResultViewModelTest
    {
        [Fact]
        public void PlayerListResultViewModel_DefaultConstructor_CreatesInstance()
        {
            // Act
            var viewModel = new PlayerListResultViewModel();

            // Assert
            Assert.NotNull(viewModel);
            Assert.Null(viewModel.DataList);
            Assert.Equal(0, viewModel.TotalItem);
        }

        [Fact]
        public void PlayerListResultViewModel_SetProperties_StoresValuesCorrectly()
        {
            // Arrange
            var viewModel = new PlayerListResultViewModel();
            var players = new List<Player>
            {
                new() { IbcCustId = 1, UserName = "user1", Currency = "USD" },
                new() { IbcCustId = 2, UserName = "user2", Currency = "EUR" }
            };

            // Act
            viewModel.DataList = players;
            viewModel.TotalItem = 2;

            // Assert
            Assert.Equal(players, viewModel.DataList);
            Assert.Equal(2, viewModel.TotalItem);
        }

        [Fact]
        public void PlayerListResultViewModel_WithEmptyDataList_StoresCorrectly()
        {
            // Arrange
            var viewModel = new PlayerListResultViewModel();
            var emptyPlayers = new List<Player>();

            // Act
            viewModel.DataList = emptyPlayers;
            viewModel.TotalItem = 0;

            // Assert
            Assert.Equal(emptyPlayers, viewModel.DataList);
            Assert.Empty(viewModel.DataList);
            Assert.Equal(0, viewModel.TotalItem);
        }

        [Fact]
        public void PlayerListResultViewModel_WithLargeDataList_StoresCorrectly()
        {
            // Arrange
            var viewModel = new PlayerListResultViewModel();
            var players = new List<Player>();
            for (int i = 1; i <= 1000; i++)
            {
                players.Add(new Player { IbcCustId = i, UserName = $"user{i}", Currency = "USD" });
            }

            // Act
            viewModel.DataList = players;
            viewModel.TotalItem = 1000;

            // Assert
            Assert.Equal(players, viewModel.DataList);
            Assert.Equal(1000, viewModel.DataList.Count());
            Assert.Equal(1000, viewModel.TotalItem);
        }

        [Fact]
        public void PlayerListResultViewModel_WithNegativeTotalItem_StoresCorrectly()
        {
            // Arrange
            var viewModel = new PlayerListResultViewModel();

            // Act
            viewModel.TotalItem = -1;

            // Assert
            Assert.Equal(-1, viewModel.TotalItem);
        }

        [Fact]
        public void PlayerListResultViewModel_WithMaxTotalItem_StoresCorrectly()
        {
            // Arrange
            var viewModel = new PlayerListResultViewModel();

            // Act
            viewModel.TotalItem = int.MaxValue;

            // Assert
            Assert.Equal(int.MaxValue, viewModel.TotalItem);
        }

        [Fact]
        public void PlayerListResultViewModel_WithNullDataList_StoresCorrectly()
        {
            // Arrange
            var viewModel = new PlayerListResultViewModel();

            // Act
            viewModel.DataList = null;
            viewModel.TotalItem = 5;

            // Assert
            Assert.Null(viewModel.DataList);
            Assert.Equal(5, viewModel.TotalItem);
        }

        [Fact]
        public void PlayerListResultViewModel_WithMismatchedTotalItemAndDataListCount_StoresCorrectly()
        {
            // Arrange
            var viewModel = new PlayerListResultViewModel();
            var players = new List<Player>
            {
                new() { IbcCustId = 1, UserName = "user1" },
                new() { IbcCustId = 2, UserName = "user2" }
            };

            // Act
            viewModel.DataList = players;
            viewModel.TotalItem = 10; // Different from actual count

            // Assert
            Assert.Equal(players, viewModel.DataList);
            Assert.Equal(2, viewModel.DataList.Count());
            Assert.Equal(10, viewModel.TotalItem); // Stores the set value, not the actual count
        }

        [Fact]
        public void PlayerListResultViewModel_WithComplexPlayerData_StoresCorrectly()
        {
            // Arrange
            var viewModel = new PlayerListResultViewModel();
            var players = new List<Player>
            {
                new()
                {
                    IbcCustId = 123,
                    UserName = "complex@user.com",
                    Currency = "USD",
                    CurrentBalance = 1000.50m,
                    OutstandingBalance = 250.25m,
                    OddsGroup = "A",
                    SpecialLevel = 5,
                    Status = 1
                }
            };

            // Act
            viewModel.DataList = players;
            viewModel.TotalItem = 1;

            // Assert
            Assert.Equal(players, viewModel.DataList);
            Assert.Single(viewModel.DataList);
            Assert.Equal(1, viewModel.TotalItem);

            var firstPlayer = viewModel.DataList.First();
            Assert.Equal(123, firstPlayer.IbcCustId);
            Assert.Equal("complex@user.com", firstPlayer.UserName);
            Assert.Equal("USD", firstPlayer.Currency);
            Assert.Equal(1000.50m, firstPlayer.CurrentBalance);
        }
    }
}