namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.ViewModels
{
    using System.Data;
    using AutoFixture;
    using Commission.ViewModels;
    using Moq;

    public class PlayerCommissionSettingViewModelTests
    {
        private readonly Fixture _fixture;

        public PlayerCommissionSettingViewModelTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Properties_ShouldBeSettableAndGettable_WhenAssigned()
        {
            // Arrange
            var sut = new PlayerCommissionSettingViewModel();
            var groupCommId = _fixture.Create<int>();
            var sporttype = _fixture.Create<int>();
            var bettype = _fixture.Create<int>();
            var discount = _fixture.Create<decimal>();
            var productCode = _fixture.Create<string>();
            var productName = _fixture.Create<string>();

            // Act
            sut.GroupCommId = groupCommId;
            sut.Sporttype = sporttype;
            sut.Bettype = bettype;
            sut.Discount = discount;
            sut.ProductCode = productCode;
            sut.ProductName = productName;

            // Assert
            Assert.Equal(groupCommId, sut.GroupCommId);
            Assert.Equal(sporttype, sut.Sporttype);
            Assert.Equal(bettype, sut.Bettype);
            Assert.Equal(discount, sut.Discount);
            Assert.Equal(productCode, sut.ProductCode);
            Assert.Equal(productName, sut.ProductName);
        }

        [Fact]
        public void MapData_ShouldMapAllProperties_WhenReaderProvided()
        {
            // Arrange
            var sut = new PlayerCommissionSettingViewModel();
            var reader = new Mock<IDataReader>();
            var expectedSporttype = 1;
            var expectedBettype = 2;
            var expectedDiscount = 0.05m;
            var expectedProductCode = "SB";
            var expectedGroupCommId = 123;

            reader.Setup(r => r["Sporttype"]).Returns(expectedSporttype);
            reader.Setup(r => r["Bettype"]).Returns(expectedBettype);
            reader.Setup(r => r["Discount"]).Returns(expectedDiscount);
            reader.Setup(r => r["ProductCode"]).Returns(expectedProductCode);
            reader.Setup(r => r["GroupDiscountID"]).Returns(expectedGroupCommId);

            // Act
            sut.MapData(reader.Object);

            // Assert
            Assert.Equal(expectedSporttype, sut.Sporttype);
            Assert.Equal(expectedBettype, sut.Bettype);
            Assert.Equal(expectedDiscount, sut.Discount);
            Assert.Equal(expectedProductCode, sut.ProductCode);
            Assert.Equal(expectedGroupCommId, sut.GroupCommId);
        }

        [Fact]
        public void GetComparerValue_ShouldReturnProductListComparerResult_WhenCalled()
        {
            // Arrange
            var setting = new PlayerCommissionSettingViewModel
            {
                ProductCode = "SB"
            };

            // Act
            var result = PlayerCommissionSettingViewModel.GetComparerValue(setting);

            // Assert
            Assert.Equal(0, result); // SB is first in the ProductSettingOrder
        }

        [Theory]
        [InlineData("SB", 0)]
        [InlineData("Parlay", 1)]
        [InlineData("NG", 2)]
        [InlineData("UnknownProduct", int.MaxValue)]
        public void GetComparerValue_ShouldReturnExpectedIndex_WhenDifferentProductCodesProvided(string productCode, int expectedIndex)
        {
            // Arrange
            var setting = new PlayerCommissionSettingViewModel
            {
                ProductCode = productCode
            };

            // Act
            var result = PlayerCommissionSettingViewModel.GetComparerValue(setting);

            // Assert
            Assert.Equal(expectedIndex, result);
        }

        [Fact]
        public void MapData_ShouldHandleStringConversion_WhenReaderContainsStringValues()
        {
            // Arrange
            var sut = new PlayerCommissionSettingViewModel();
            var reader = new Mock<IDataReader>();

            reader.Setup(r => r["Sporttype"]).Returns("1");
            reader.Setup(r => r["Bettype"]).Returns("2");
            reader.Setup(r => r["Discount"]).Returns("0.05");
            reader.Setup(r => r["ProductCode"]).Returns("SB");
            reader.Setup(r => r["GroupDiscountID"]).Returns("123");

            // Act
            sut.MapData(reader.Object);

            // Assert
            Assert.Equal(1, sut.Sporttype);
            Assert.Equal(2, sut.Bettype);
            Assert.Equal(0.05m, sut.Discount);
            Assert.Equal("SB", sut.ProductCode);
            Assert.Equal(123, sut.GroupCommId);
        }

        [Fact]
        public void MapData_ShouldHandleNullProductCode_WhenReaderContainsNull()
        {
            // Arrange
            var sut = new PlayerCommissionSettingViewModel();
            var reader = new Mock<IDataReader>();

            reader.Setup(r => r["Sporttype"]).Returns(1);
            reader.Setup(r => r["Bettype"]).Returns(2);
            reader.Setup(r => r["Discount"]).Returns(0.05m);
            reader.Setup(r => r["ProductCode"]).Returns(null);
            reader.Setup(r => r["GroupDiscountID"]).Returns(123);

            // Act
            sut.MapData(reader.Object);

            // Assert
            Assert.Null(sut.ProductCode);
        }

        [Fact]
        public void DefaultValues_ShouldBeZeroOrNull_WhenCreated()
        {
            // Arrange & Act
            var sut = new PlayerCommissionSettingViewModel();

            // Assert
            Assert.Equal(0, sut.GroupCommId);
            Assert.Equal(0, sut.Sporttype);
            Assert.Equal(0, sut.Bettype);
            Assert.Equal(0, sut.Discount);
            Assert.Null(sut.ProductCode);
            Assert.Null(sut.ProductName);
        }
    }

    public class GroupCommSettingComparerTests
    {
        [Fact]
        public void Compare_ShouldReturnZero_WhenBothSettingsHaveSameProductCode()
        {
            // Arrange
            var comparer = new GroupCommSettingComparer();
            var setting1 = new PlayerCommissionSettingViewModel { ProductCode = "SB" };
            var setting2 = new PlayerCommissionSettingViewModel { ProductCode = "SB" };

            // Act
            var result = comparer.Compare(setting1, setting2);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Compare_ShouldReturnNegativeOne_WhenFirstSettingHasLowerIndex()
        {
            // Arrange
            var comparer = new GroupCommSettingComparer();
            var setting1 = new PlayerCommissionSettingViewModel { ProductCode = "SB" }; // Index 0
            var setting2 = new PlayerCommissionSettingViewModel { ProductCode = "NG" }; // Index 2

            // Act
            var result = comparer.Compare(setting1, setting2);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Compare_ShouldReturnPositiveOne_WhenFirstSettingHasHigherIndex()
        {
            // Arrange
            var comparer = new GroupCommSettingComparer();
            var setting1 = new PlayerCommissionSettingViewModel { ProductCode = "NG" }; // Index 2
            var setting2 = new PlayerCommissionSettingViewModel { ProductCode = "SB" }; // Index 0

            // Act
            var result = comparer.Compare(setting1, setting2);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Compare_ShouldHandleUnknownProductCodes_WhenCalled()
        {
            // Arrange
            var comparer = new GroupCommSettingComparer();
            var setting1 = new PlayerCommissionSettingViewModel { ProductCode = "SB" }; // Index 0
            var setting2 = new PlayerCommissionSettingViewModel { ProductCode = "Unknown" }; // Index int.MaxValue

            // Act
            var result = comparer.Compare(setting1, setting2);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Compare_ShouldReturnZero_WhenBothSettingsHaveUnknownProductCodes()
        {
            // Arrange
            var comparer = new GroupCommSettingComparer();
            var setting1 = new PlayerCommissionSettingViewModel { ProductCode = "Unknown1" }; // Index int.MaxValue
            var setting2 = new PlayerCommissionSettingViewModel { ProductCode = "Unknown2" }; // Index int.MaxValue

            // Act
            var result = comparer.Compare(setting1, setting2);

            // Assert
            Assert.Equal(0, result);
        }
    }
}