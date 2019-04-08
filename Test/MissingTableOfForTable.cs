using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class MissingTableOfForTable : TestBase
    {
        [Test]
        public void SheetConverterShowsErrorIfTableOfMissingForTable()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            var worksheet = FirstWorksheet(@"TestExcelFiles\MissingTableOfForTable\MissingTableOfForTable.xlsx");

            string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, worksheet, ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

            StringAssert.Contains("trying to set up a table, starting at cell C5", generatedCode);
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfTableOfMissingForTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\MissingTableOfForTable\");

            Assert.AreNotEqual(0, results.ErrorCode);

            StringAssert.Contains("Workbook 'MissingTableOfForTable'", results.LogMessages); 

            StringAssert.Contains("Worksheet 'MissingTableOfForTable'", results.LogMessages);

            StringAssert.Contains("trying to set up a table, starting at cell C5", results.LogMessages);
        }
    }
}
