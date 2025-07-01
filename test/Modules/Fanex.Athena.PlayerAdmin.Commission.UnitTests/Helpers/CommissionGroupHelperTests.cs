using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Athena.PlayerAdmin.Commission.Helpers;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Helpers
{
    public class CommissionGroupHelperTests
    {
        [Fact]
        public void ProductListComparer_WithValidProductCode_ReturnsCorrectIndex()
        {
            // Arrange
            var productCode = ProductCode.SB;

            // Act
            var result = CommissionGroupHelper.ProductListComparer(productCode);

            // Assert
            Assert.Equal(0, result); // SB should be at index 0
        }

        [Fact]
        public void ProductListComparer_WithParlayProductCode_ReturnsCorrectIndex()
        {
            // Arrange
            var productCode = "Parlay";

            // Act
            var result = CommissionGroupHelper.ProductListComparer(productCode);

            // Assert
            Assert.Equal(1, result); // Parlay should be at index 1
        }

        [Fact]
        public void ProductListComparer_WithValidProductCodeInMiddle_ReturnsCorrectIndex()
        {
            // Arrange
            var productCode = ProductCode.NG;

            // Act
            var result = CommissionGroupHelper.ProductListComparer(productCode);

            // Assert
            Assert.Equal(2, result); // NG should be at index 2
        }

        [Fact]
        public void ProductListComparer_WithValidProductCodeAtEnd_ReturnsCorrectIndex()
        {
            // Arrange
            var productCode = ProductCode.WLC;

            // Act
            var result = CommissionGroupHelper.ProductListComparer(productCode);

            // Assert
            Assert.Equal(49, result); // WLC should be at index 49 (last in the array)
        }

        [Fact]
        public void ProductListComparer_WithInvalidProductCode_ReturnsMaxValue()
        {
            // Arrange
            var productCode = "INVALID_PRODUCT";

            // Act
            var result = CommissionGroupHelper.ProductListComparer(productCode);

            // Assert
            Assert.Equal(int.MaxValue, result);
        }

        [Fact]
        public void ProductListComparer_WithNullProductCode_ReturnsMaxValue()
        {
            // Arrange
            string productCode = null;

            // Act
            var result = CommissionGroupHelper.ProductListComparer(productCode);

            // Assert
            Assert.Equal(int.MaxValue, result);
        }

        [Fact]
        public void ProductListComparer_WithEmptyProductCode_ReturnsMaxValue()
        {
            // Arrange
            var productCode = string.Empty;

            // Act
            var result = CommissionGroupHelper.ProductListComparer(productCode);

            // Assert
            Assert.Equal(int.MaxValue, result);
        }

        [Theory]
        [InlineData(ProductCode.SB, 0)]
        [InlineData("Parlay", 1)]
        [InlineData(ProductCode.NG, 2)]
        [InlineData(ProductCode.CS, 3)]
        [InlineData(ProductCode.VB, 4)]
        [InlineData(ProductCode.PP, 5)]
        [InlineData(ProductCode.SXG, 6)]
        [InlineData(ProductCode.SG, 7)]
        [InlineData(ProductCode.KN, 8)]
        [InlineData(ProductCode.LT, 9)]
        [InlineData(ProductCode.TG, 10)]
        [InlineData(ProductCode.VS, 11)]
        [InlineData(ProductCode.VRG, 12)]
        [InlineData(ProductCode.CC, 13)]
        [InlineData(ProductCode.SBC, 14)]
        [InlineData(ProductCode.SAG, 15)]
        [InlineData(ProductCode.PGS, 16)]
        [InlineData(ProductCode.AB, 17)]
        [InlineData(ProductCode.WM, 18)]
        [InlineData(ProductCode.BGG, 19)]
        [InlineData(ProductCode.JK, 20)]
        [InlineData(ProductCode.HB, 21)]
        [InlineData(ProductCode.VGA, 22)]
        [InlineData(ProductCode.CG, 23)]
        [InlineData(ProductCode.AP, 24)]
        [InlineData(ProductCode.APM, 25)]
        [InlineData(ProductCode.FGGL, 26)]
        [InlineData(ProductCode.MXG, 27)]
        [InlineData(ProductCode.JIG, 28)]
        [InlineData(ProductCode.YB, 29)]
        [InlineData(ProductCode.MGRNG, 30)]
        [InlineData(ProductCode.WE, 31)]
        [InlineData(ProductCode.SEA, 32)]
        [InlineData(ProductCode.L22, 33)]
        [InlineData(ProductCode.UUSL, 34)]
        [InlineData(ProductCode.FS, 35)]
        [InlineData(ProductCode.FAC, 36)]
        [InlineData(ProductCode.NS, 37)]
        [InlineData(ProductCode.ON, 38)]
        [InlineData(ProductCode.GPIRNG, 39)]
        [InlineData(ProductCode.PTRNG, 40)]
        [InlineData(ProductCode.GPILC, 41)]
        [InlineData(ProductCode.PS, 42)]
        [InlineData(ProductCode.ASK, 43)]
        [InlineData(ProductCode.FG, 44)]
        [InlineData(ProductCode.HOT, 45)]
        [InlineData(ProductCode.MGLC, 46)]
        [InlineData(ProductCode.PTLC, 47)]
        [InlineData(ProductCode.RGRNG, 48)]
        [InlineData(ProductCode.WLC, 49)]
        public void ProductListComparer_WithAllValidProductCodes_ReturnsCorrectIndexes(string productCode, int expectedIndex)
        {
            // Act
            var result = CommissionGroupHelper.ProductListComparer(productCode);

            // Assert
            Assert.Equal(expectedIndex, result);
        }

        [Fact]
        public void GetProductPermissions_WithParlayProductCode_ChecksSBPermission()
        {
            // Arrange
            var productCode = "Parlay";
            var productAccess = CreateProductAccessWithProducts(ProductCode.SB);

            // Act
            var result = CommissionGroupHelper.GetProductPermissions(productAccess, productCode);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetProductPermissions_WithParlayProductCode_WhenSBNotAllowed_ReturnsFalse()
        {
            // Arrange
            var productCode = "Parlay";
            var productAccess = CreateProductAccessWithProducts(ProductCode.NG); // SB not included

            // Act
            var result = CommissionGroupHelper.GetProductPermissions(productAccess, productCode);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetProductPermissions_WithRegularProductCode_ChecksDirectPermission()
        {
            // Arrange
            var productCode = ProductCode.NG;
            var productAccess = CreateProductAccessWithProducts(ProductCode.NG);

            // Act
            var result = CommissionGroupHelper.GetProductPermissions(productAccess, productCode);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetProductPermissions_WithRegularProductCode_WhenNotAllowed_ReturnsFalse()
        {
            // Arrange
            var productCode = ProductCode.NG;
            var productAccess = CreateProductAccessWithProducts(ProductCode.SB); // NG not included

            // Act
            var result = CommissionGroupHelper.GetProductPermissions(productAccess, productCode);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetProductPermissions_WithNullProductAccess_ThrowsNullReferenceException()
        {
            // Arrange
            var productCode = ProductCode.SB;
            ProductAccess productAccess = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => CommissionGroupHelper.GetProductPermissions(productAccess, productCode));
        }

        [Fact]
        public void GetProductPermissions_WithNullProductCode_ThrowsArgumentNullException()
        {
            // Arrange
            string productCode = null;
            var productAccess = CreateProductAccessWithProducts(ProductCode.SB);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => CommissionGroupHelper.GetProductPermissions(productAccess, productCode));
        }

        [Fact]
        public void GetProductPermissions_WithEmptyProductCode_ReturnsFalse()
        {
            // Arrange
            var productCode = string.Empty;
            var productAccess = CreateProductAccessWithProducts(ProductCode.SB);

            // Act
            var result = CommissionGroupHelper.GetProductPermissions(productAccess, productCode);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(0.1234, 1, 12)] // 0.1234 * 10000 = 1234, position 1 = 1234 / 100 = 12
        [InlineData(0.1234, 2, 3)]  // 0.1234 * 10000 = 1234, position 2 = (1234 % 100) / 10 = 34 / 10 = 3
        [InlineData(0.1234, 3, 4)]  // 0.1234 * 10000 = 1234, position 3 = 1234 % 10 = 4
        [InlineData(0.5678, 1, 56)] // 0.5678 * 10000 = 5678, position 1 = 5678 / 100 = 56
        [InlineData(0.5678, 2, 7)]  // 0.5678 * 10000 = 5678, position 2 = (5678 % 100) / 10 = 78 / 10 = 7
        [InlineData(0.5678, 3, 8)]  // 0.5678 * 10000 = 5678, position 3 = 5678 % 10 = 8
        [InlineData(0.0100, 1, 1)]  // 0.0100 * 10000 = 100, position 1 = 100 / 100 = 1
        [InlineData(0.0100, 2, 0)]  // 0.0100 * 10000 = 100, position 2 = (100 % 100) / 10 = 0 / 10 = 0
        [InlineData(0.0100, 3, 0)]  // 0.0100 * 10000 = 100, position 3 = 100 % 10 = 0
        [InlineData(0.9999, 1, 99)] // 0.9999 * 10000 = 9999, position 1 = 9999 / 100 = 99
        [InlineData(0.9999, 2, 9)]  // 0.9999 * 10000 = 9999, position 2 = (9999 % 100) / 10 = 99 / 10 = 9
        [InlineData(0.9999, 3, 9)]  // 0.9999 * 10000 = 9999, position 3 = 9999 % 10 = 9
        public void GetCommPart_WithValidInputs_ReturnsCorrectValues(decimal comm, int position, int expected)
        {
            // Act
            var result = CommissionGroupHelper.GetCommPart(comm, position);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0.1234, 0)] // Invalid position
        [InlineData(0.1234, 4)] // Invalid position
        [InlineData(0.1234, -1)] // Invalid position
        [InlineData(0.1234, 100)] // Invalid position
        public void GetCommPart_WithInvalidPosition_ReturnsZero(decimal comm, int position)
        {
            // Act
            var result = CommissionGroupHelper.GetCommPart(comm, position);

            // Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(0.0, 1, 0)]
        [InlineData(0.0, 2, 0)]
        [InlineData(0.0, 3, 0)]
        [InlineData(1.0, 1, 100)]
        [InlineData(1.0, 2, 0)]
        [InlineData(1.0, 3, 0)]
        [InlineData(0.01, 1, 1)]
        [InlineData(0.01, 2, 0)]
        [InlineData(0.01, 3, 0)]
        [InlineData(0.001, 1, 0)]
        [InlineData(0.001, 2, 1)]
        [InlineData(0.001, 3, 0)]
        public void GetCommPart_WithEdgeCases_ReturnsCorrectValues(decimal comm, int position, int expected)
        {
            // Act
            var result = CommissionGroupHelper.GetCommPart(comm, position);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0.1234, 1, 12)]
        [InlineData(0.1234, 2, 3)]
        [InlineData(0.1234, 3, 4)]
        public void GetCommPart_WithNegativeCommission_ReturnsCorrectValues(decimal comm, int position, int expected)
        {
            // Arrange
            var negativeComm = -comm;

            // Act
            var result = CommissionGroupHelper.GetCommPart(negativeComm, position);

            // Assert
            Assert.Equal(-expected, result);
        }

        private static ProductAccess CreateProductAccessWithProducts(params string[] productCodes)
        {
            var products = productCodes.Select(code => new Product { ProductCode = code }).ToList();
            return new ProductAccess(products);
        }
    }
} 