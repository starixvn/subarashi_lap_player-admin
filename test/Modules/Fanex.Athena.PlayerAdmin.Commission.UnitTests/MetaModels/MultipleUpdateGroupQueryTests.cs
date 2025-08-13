namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.MetaModels
{
    using AutoFixture;
    using Commission.MetaModels;
    using Commission.ViewModels;

    public class MultipleUpdateGroupQueryTests
    {
        private readonly Fixture _fixture;

        public MultipleUpdateGroupQueryTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void CommValues_ShouldBeSettableAndGettable_WhenAssigned()
        {
            // Arrange
            var sut = new MultipleUpdateGroupQuery();
            var commValues = _fixture.Create<IEnumerable<PlayerCommissionSettingViewModel>>();

            // Act
            sut.CommValues = commValues;

            // Assert
            Assert.Equal(commValues, sut.CommValues);
        }

        [Fact]
        public void MultipleUpdateGroupQuery_ShouldInheritFromBasePlayerCommissionUpdateQuery_WhenCreated()
        {
            // Arrange & Act
            var sut = new MultipleUpdateGroupQuery();

            // Assert
            Assert.IsAssignableFrom<BasePlayerCommissionUpdateQuery>(sut);
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenBaseIsValidAndCommValuesAreValid()
        {
            // Arrange
            var commValues = new List<PlayerCommissionSettingViewModel>
            {
                new() { Discount = 0.0000m },
                new() { Discount = 0.0005m },
                new() { Discount = 0.0010m }
            };
            var sut = new MultipleUpdateGroupQuery
            {
                CustIds = "1,2,3",
                CommValues = commValues
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenBaseIsInvalid()
        {
            // Arrange
            var commValues = new List<PlayerCommissionSettingViewModel>
            {
                new() { Discount = 0.0000m }
            };
            var sut = new MultipleUpdateGroupQuery
            {
                CustIds = "1,2,invalid",
                CommValues = commValues
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenCommValuesIsNull()
        {
            // Arrange
            var sut = new MultipleUpdateGroupQuery
            {
                CustIds = "1,2,3",
                CommValues = null
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenCommValuesContainInvalidDiscount()
        {
            // Arrange
            var commValues = new List<PlayerCommissionSettingViewModel>
            {
                new() { Discount = 0.0000m },
                new() { Discount = 0.0001m }, // Invalid discount
                new() { Discount = 0.0010m }
            };
            var sut = new MultipleUpdateGroupQuery
            {
                CustIds = "1,2,3",
                CommValues = commValues
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenCommValuesIsEmpty()
        {
            // Arrange
            var commValues = new List<PlayerCommissionSettingViewModel>();
            var sut = new MultipleUpdateGroupQuery
            {
                CustIds = "1,2,3",
                CommValues = commValues
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Properties_ShouldBeInheritedFromBase_WhenAccessed()
        {
            // Arrange
            var sut = new MultipleUpdateGroupQuery();
            var custIds = _fixture.Create<string>();
            var licenseeId = _fixture.Create<int>();

            // Act
            sut.CustIds = custIds;
            sut.LicenseeId = licenseeId;

            // Assert
            Assert.Equal(custIds, sut.CustIds);
            Assert.Equal(licenseeId, sut.LicenseeId);
        }
    }
}