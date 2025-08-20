namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Settings
{
    using Core.Settings;

    public class FunctionAllowedProductsSettingTests
    {
        [Fact]
        public void Properties_AreSetAndRetrieved()
        {
            var setting = new FunctionAllowedProductsSetting();
            var customerWinLoss = new[] { "Product1", "Product2" };
            var commissionGroups = new[] { "Group1", "Group2", "Group3" };
            var bettingTicketCenter = new[] { "BTC1", "BTC2" };
            var winLossAnalysis = new[] { "WLA1", "WLA2", "WLA3", "WLA4" };
            var defaultProducts = new[] { "Default1", "Default2", "Default3" };

            setting.CustomerWinLoss = customerWinLoss;
            setting.CommissionGroups = commissionGroups;
            setting.BettingTicketCenter = bettingTicketCenter;
            setting.WinLossAnalysis = winLossAnalysis;
            setting.Default = defaultProducts;

            Assert.Equal(customerWinLoss, setting.CustomerWinLoss);
            Assert.Equal(commissionGroups, setting.CommissionGroups);
            Assert.Equal(bettingTicketCenter, setting.BettingTicketCenter);
            Assert.Equal(winLossAnalysis, setting.WinLossAnalysis);
            Assert.Equal(defaultProducts, setting.Default);
        }

        [Fact]
        public void Properties_InitiallyNull()
        {
            var setting = new FunctionAllowedProductsSetting();

            Assert.Null(setting.CustomerWinLoss);
            Assert.Null(setting.CommissionGroups);
            Assert.Null(setting.BettingTicketCenter);
            Assert.Null(setting.WinLossAnalysis);
            Assert.Null(setting.Default);
        }

        [Fact]
        public void Properties_CanBeSetToEmptyArrays()
        {
            var setting = new FunctionAllowedProductsSetting();
            var emptyArray = new string[0];

            setting.CustomerWinLoss = emptyArray;
            setting.CommissionGroups = emptyArray;
            setting.BettingTicketCenter = emptyArray;
            setting.WinLossAnalysis = emptyArray;
            setting.Default = emptyArray;

            Assert.Equal(emptyArray, setting.CustomerWinLoss);
            Assert.Equal(emptyArray, setting.CommissionGroups);
            Assert.Equal(emptyArray, setting.BettingTicketCenter);
            Assert.Equal(emptyArray, setting.WinLossAnalysis);
            Assert.Equal(emptyArray, setting.Default);
        }

        [Fact]
        public void Properties_CanBeSetToNull()
        {
            var setting = new FunctionAllowedProductsSetting();
            
            // Set to non-null values first
            setting.CustomerWinLoss = new[] { "test" };
            setting.CommissionGroups = new[] { "test" };
            setting.BettingTicketCenter = new[] { "test" };
            setting.WinLossAnalysis = new[] { "test" };
            setting.Default = new[] { "test" };

            // Then set back to null
            setting.CustomerWinLoss = null;
            setting.CommissionGroups = null;
            setting.BettingTicketCenter = null;
            setting.WinLossAnalysis = null;
            setting.Default = null;

            Assert.Null(setting.CustomerWinLoss);
            Assert.Null(setting.CommissionGroups);
            Assert.Null(setting.BettingTicketCenter);
            Assert.Null(setting.WinLossAnalysis);
            Assert.Null(setting.Default);
        }

        [Fact]
        public void Properties_HandlesLargeArrays()
        {
            var setting = new FunctionAllowedProductsSetting();
            var largeArray = new string[1000];
            for (int i = 0; i < 1000; i++)
            {
                largeArray[i] = $"Product{i}";
            }

            setting.CustomerWinLoss = largeArray;
            setting.CommissionGroups = largeArray;
            setting.BettingTicketCenter = largeArray;
            setting.WinLossAnalysis = largeArray;
            setting.Default = largeArray;

            Assert.Equal(largeArray, setting.CustomerWinLoss);
            Assert.Equal(largeArray, setting.CommissionGroups);
            Assert.Equal(largeArray, setting.BettingTicketCenter);
            Assert.Equal(largeArray, setting.WinLossAnalysis);
            Assert.Equal(largeArray, setting.Default);
            Assert.Equal(1000, setting.CustomerWinLoss.Length);
            Assert.Equal(1000, setting.CommissionGroups.Length);
        }

        [Fact]
        public void Properties_HandlesArraysWithNullElements()
        {
            var setting = new FunctionAllowedProductsSetting();
            var arrayWithNulls = new[] { "Product1", null, "Product3", null };

            setting.CustomerWinLoss = arrayWithNulls;
            setting.CommissionGroups = arrayWithNulls;
            setting.BettingTicketCenter = arrayWithNulls;
            setting.WinLossAnalysis = arrayWithNulls;
            setting.Default = arrayWithNulls;

            Assert.Equal(arrayWithNulls, setting.CustomerWinLoss);
            Assert.Equal(arrayWithNulls, setting.CommissionGroups);
            Assert.Equal(arrayWithNulls, setting.BettingTicketCenter);
            Assert.Equal(arrayWithNulls, setting.WinLossAnalysis);
            Assert.Equal(arrayWithNulls, setting.Default);
            Assert.Null(setting.CustomerWinLoss[1]);
            Assert.Null(setting.CommissionGroups[3]);
        }

        [Fact]
        public void Properties_HandlesArraysWithEmptyStrings()
        {
            var setting = new FunctionAllowedProductsSetting();
            var arrayWithEmpties = new[] { "Product1", "", "Product3", string.Empty };

            setting.CustomerWinLoss = arrayWithEmpties;
            setting.CommissionGroups = arrayWithEmpties;
            setting.BettingTicketCenter = arrayWithEmpties;
            setting.WinLossAnalysis = arrayWithEmpties;
            setting.Default = arrayWithEmpties;

            Assert.Equal(arrayWithEmpties, setting.CustomerWinLoss);
            Assert.Equal(arrayWithEmpties, setting.CommissionGroups);
            Assert.Equal(arrayWithEmpties, setting.BettingTicketCenter);
            Assert.Equal(arrayWithEmpties, setting.WinLossAnalysis);
            Assert.Equal(arrayWithEmpties, setting.Default);
            Assert.Equal("", setting.CustomerWinLoss[1]);
            Assert.Equal(string.Empty, setting.CommissionGroups[3]);
        }
    }
}