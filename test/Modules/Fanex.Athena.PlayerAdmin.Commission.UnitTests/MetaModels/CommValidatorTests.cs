namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.MetaModels
{
    using Commission.MetaModels;

    public class CommValidatorTests
    {
        public static IEnumerable<object[]> ValidCommissionData =>
            new List<object[]>
            {
                new object[] { new[] { 0.0000m, 0.0005m, 0.0010m } },
                new object[] { new[] { 0.0500m, 0.1000m, 0.1500m } }
            };

        public static IEnumerable<object[]> InvalidCommissionData =>
            new List<object[]>
            {
                new object[] { new[] { 0.0000m, 0.0001m, 0.0010m } },
                new object[] { new[] { 0.0500m, 1.0000m, 0.1500m } }
            };

        [Theory]
        [InlineData(0.0000)]
        [InlineData(0.0005)]
        [InlineData(0.0010)]
        [InlineData(0.0500)]
        [InlineData(0.1000)]
        [InlineData(0.1500)]
        public void IsValid_ShouldReturnTrue_WhenValidCommissionValueProvided(decimal commission)
        {
            // Act
            var result = CommValidator.IsValid(commission);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(-0.0001)]
        [InlineData(0.1501)]
        [InlineData(0.0001)]
        [InlineData(0.0003)]
        [InlineData(1.0000)]
        public void IsValid_ShouldReturnFalse_WhenInvalidCommissionValueProvided(decimal commission)
        {
            // Act
            var result = CommValidator.IsValid(commission);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenAllCommissionValuesAreValid()
        {
            // Arrange
            var commissions = new[] { 0.0000m, 0.0005m, 0.0010m, 0.0500m, 0.1000m, 0.1500m };

            // Act
            var result = CommValidator.IsValid(commissions);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenAnyCommissionValueIsInvalid()
        {
            // Arrange
            var commissions = new[] { 0.0000m, 0.0005m, 0.0001m, 0.0500m };

            // Act
            var result = CommValidator.IsValid(commissions);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenEmptyArrayProvided()
        {
            // Arrange
            var commissions = new decimal[0];

            // Act
            var result = CommValidator.IsValid(commissions);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_ShouldHandleMultipleCalls_WhenCalledRepeatedly()
        {
            // Arrange
            var validCommission = 0.0500m;
            var invalidCommission = 0.0001m;

            // Act & Assert
            Assert.True(CommValidator.IsValid(validCommission));
            Assert.False(CommValidator.IsValid(invalidCommission));
            Assert.True(CommValidator.IsValid(validCommission));
        }

        [Theory]
        [MemberData(nameof(ValidCommissionData))]
        public void IsValid_ShouldReturnTrue_WhenAllMultipleValidValuesProvided(decimal[] commissions)
        {
            // Act
            var result = CommValidator.IsValid(commissions);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(InvalidCommissionData))]
        public void IsValid_ShouldReturnFalse_WhenAnyMultipleValueIsInvalid(decimal[] commissions)
        {
            // Act
            var result = CommValidator.IsValid(commissions);

            // Assert
            Assert.False(result);
        }
    }
}