namespace Fanex.Athena.Excel.Base.UnitTests
{
    using Fanex.Athena.PlayerAdmin.Core.Excel;
    using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
    using NPOI.SS.UserModel;

    using NSubstitute;

    using NUnit.Framework;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class BaseExcelTest
    {
        [Test]
        public void Export_WhenNotMultiSheet_AssignDataMethodIsCalled()
        {
            // Arrange
            var report = Substitute.ForPartsOf<BaseExcel>();
            report.When(x => x.AssignData()).DoNotCallBase();
            report.When(x => x.AssignDataMultiSheet()).DoNotCallBase();
            report.When(x => x.LoadTemplate()).DoNotCallBase();
            report.When(x => x.RemoveTemplate()).DoNotCallBase();
            report.IsMultiSheet = false;

            // Action
            report.Export();

            // Assert
            report.Received().LoadTemplate();
            report.Received().AssignData();
            report.Received().RemoveTemplate();
        }

        [Test]
        public void Export_WhenHaveMultiSheet_AssignDataMultiSheetMethodIsCalled()
        {
            // Arrange
            var report = Substitute.ForPartsOf<BaseExcel>();
            report.When(x => x.AssignData()).DoNotCallBase();
            report.When(x => x.AssignDataMultiSheet()).DoNotCallBase();
            report.When(x => x.LoadTemplate()).DoNotCallBase();
            report.When(x => x.RemoveTemplate()).DoNotCallBase();
            report.IsMultiSheet = true;

            // Action
            report.Export();

            // Assert
            report.Received().LoadTemplate();
            report.Received().AssignDataMultiSheet();
            report.Received().RemoveTemplate();
        }

        [Test]
        public void ClearTemplate_WhenCall_CallReleventMethods()
        {
            // Arrange
            var sheet = Substitute.For<ISheet>();
            sheet.LastRowNum.Returns(1);

            // Action
            BaseExcel.ClearTemplate(sheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                sheet.Received(1).GetRow(0);
                sheet.Received(1).GetRow(1);
                sheet.Received(2).RemoveRow(Arg.Any<IRow>());
            });
        }

        [Test]
        public void AssignData_TotalRowsIsZeno_CallReleventMethods()
        {
            // Arrange
            const int TotalRows = 0;
            var report = Substitute.ForPartsOf<BaseExcel>();
            report.SetData(string.Empty, TotalRows, 0);
            report.Workbook.Returns(Substitute.For<IWorkbook>());

            // Action
            report.AssignData();

            // Assert
            AssertNoInfoAssignSheet(report);
        }

        [Test]
        public void AssignData_TotalRowsGreaterThanZeno_CallAssignSheetData()
        {
            // Arrange
            const int TotalRows = 1;
            var report = Substitute.ForPartsOf<BaseExcel>();
            report.SetData(string.Empty, TotalRows, 0);
            report.Workbook.Returns(Substitute.For<IWorkbook>());

            // Action
            report.AssignData();

            // Assert
            Assert.Multiple(() =>
            {
                report.DidNotReceive().AssignNoInfoRow(Arg.Any<ISheet>());
                report.Received(1).AssignSheetData(Arg.Any<ISheet>());
            });
        }

        [Test]
        public void AssignDataMultiSheet_TotalRowsIsZeno_CallReleventMethods()
        {
            // Arrange
            const int TotalRows = 0;
            var report = Substitute.ForPartsOf<BaseExcel>();
            report.SetData(string.Empty, TotalRows, 0);
            report.Workbook.Returns(Substitute.For<IWorkbook>());

            // Action
            report.AssignDataMultiSheet();

            // Assert
            AssertNoInfoAssignSheet(report);
        }

        [Test]
        public void AssignDataMultiSheet_TotalRowsGreaterThanZero_CallReleventMethods()
        {
            // Arrange
            const int TotalRows = 3;
            var report = Substitute.ForPartsOf<BaseExcel>();
            report.SetData(string.Empty, TotalRows, 2);
            report.Workbook.Returns(Substitute.For<IWorkbook>());

            // Action
            report.AssignDataMultiSheet();

            // Assert
            Assert.Multiple(() =>
            {
                report.Received(2).AssignHeader(Arg.Any<ISheet>());
                report.Received(2).AssignTitle(Arg.Any<ISheet>());
                report.Received(2).ProcessUI(Arg.Any<ISheet>());
                report.Received(1).AssignSheetData(Arg.Any<ISheet>(), 0);
                report.Received(1).AssignSheetData(Arg.Any<ISheet>(), 1);
            });
        }

        [Test]
        public void SetTextRowHeader_WhenCall_CallRelevantMethod()
        {
            //Arrange
            var row = Substitute.For<IRow>();
            var headerTextList = FakeHelper.CreateMany<string>();
            SubstituteRowCells(row, headerTextList.Count());
            var cell = row.Cells[0];

            //Act
            BaseExcel.SetTextRowHeader(row, headerTextList);

            //Assert
            cell.ReceivedWithAnyArgs(headerTextList.Count()).SetCellValue(default(string));
        }

        [Test]
        public void RemoveSheet_WhenNotExistSheet_NotCallRelevantMethod()
        {
            //Arrange
            var report = Substitute.ForPartsOf<BaseExcel>();
            var workBook = Substitute.For<IWorkbook>();
            report.Workbook.Returns(workBook);
            workBook.GetSheet(default).ReturnsForAnyArgs((ISheet)null);

            //Act
            report.RemoveTemplate();

            //Assert
            workBook.DidNotReceiveWithAnyArgs().RemoveSheetAt(default);
        }

        [Test]
        public void RemoveSheet_WhenExistSheet_CallRelevantMethod()
        {
            //Arrange
            var report = Substitute.ForPartsOf<BaseExcel>();
            var workBook = Substitute.For<IWorkbook>();
            report.Workbook.Returns(workBook);
            workBook.GetSheet(default).ReturnsForAnyArgs(Substitute.For<ISheet>());

            //Act
            report.RemoveTemplate();

            //Assert
            workBook.ReceivedWithAnyArgs().RemoveSheetAt(default);
        }


        private static void AssertNoInfoAssignSheet(BaseExcel report)
        {
            Assert.Multiple(() =>
            {
                report.Received(1).AssignHeader(Arg.Any<ISheet>());
                report.Received(1).AssignTitle(Arg.Any<ISheet>());
                report.Received(1).AssignNoInfoRow(Arg.Any<ISheet>());
                report.Received(1).ProcessUI(Arg.Any<ISheet>());
                report.DidNotReceive().AssignSheetData(Arg.Any<ISheet>());
            });
        }

        private void SubstituteRowCells(IRow row, int columnCount)
        {
            var cells = new List<ICell>();
            var cell = Substitute.For<ICell>();
            for (int i = 0; i < columnCount; i++)
            {
                cells.Add(cell);
            }
            row.Cells.Returns(cells);
        }
    }
}
