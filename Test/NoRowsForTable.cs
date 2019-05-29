using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class NoRowsForTable : TestBase
    {
        [Test]
        public void SheetConverterShowsErrorIfNoRowsForTable()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            using (var workbook = Workbook(@"TestExcelFiles\NoRowsForTable\NoRowsForTable.xlsx"))
            {
                string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                StringAssert.Contains("table starting at cell C5 has no rows", generatedCode);

                StringAssert.Contains("row of Property Values starting at D8", generatedCode);
            }
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfNoRowsForTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\NoRowsForTable\");

            Assert.AreNotEqual(0, results.ErrorCode);

            StringAssert.Contains("Workbook 'NoRowsForTable'", results.LogMessages); 

            StringAssert.Contains("Worksheet 'NoRowsForTable'", results.LogMessages);

            StringAssert.Contains("table starting at cell C5 has no rows", results.LogMessages);

            StringAssert.Contains("row of Property Values starting at D8", results.LogMessages);
        }
    }
}
