using Fanex.Athena.PlayerAdmin.Commission.Helpers;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;

namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Helpers
{
    public class CommissionGroupHelperTests
    {
        [Test]
        public void MapHardcodedNameFromDB_WhenIsUpdateToDB_ReturnCorrectResult()
        {
            //Arrange
            var permission = Permissions;
            var isUpdate = true;

            //Act
            var result = CommissionGroupHelper.MapHardcodedNameFromDB(permission, isUpdate);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(2, result.Count);
                Assert.IsTrue(result.ContainsKey(ProductCode.CS));
                Assert.IsTrue(result.ContainsKey("Test"));
                Assert.IsFalse(result.ContainsKey("SB"));
            });
        }

        [Test]
        public void MapHardcodedNameFromDB_WhenIsNotUpdateToDB_ReturnCorrectResult()
        {
            //Arrange
            var permission = Permissions;
            var isUpdate = false;

            //Act
            var result = CommissionGroupHelper.MapHardcodedNameFromDB(permission, isUpdate);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(permission.Count, result.Count);
                Assert.IsTrue(result.ContainsKey(ProductCode.CS));
                Assert.IsTrue(result.ContainsKey("Test"));
                Assert.IsTrue(result.ContainsKey("SB"));
            });
        }

        [Test]
        public void GetProductColumns_WhenCall_ReturnCorrectResult()
        {
            var hardcodeColumns = new Dictionary<string, string>
            {
                {"test", "test" }
            };
            var reader = Substitute.For<IDataReader>();
            var table = new DataTable();
            table.Columns.Add("ColumnName");
            table.Rows.Add("ColumnName");
            reader.GetSchemaTable().Returns(table);

            //Act
            var result = CommissionGroupHelper.GetProductColumns(reader, hardcodeColumns);

            //Assert
            Assert.AreEqual(hardcodeColumns.Count, result.Count);
        }

        private Dictionary<string, bool> Permissions => new Dictionary<string, bool>
        {
            {ProductCode.CS, true},
            {"SB", true },
            {"Test", true }
        };
    }
}
