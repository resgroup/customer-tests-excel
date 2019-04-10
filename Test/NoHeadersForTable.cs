using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class NoHeadersForTable : TestBase
    {
        [Test]
        public void SheetConverterShowsErrorIfNoHeadersForTable()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            var worksheet = FirstWorksheet(@"TestExcelFiles\NoHeadersForTable\NoHeadersForTable.xlsx");

            string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, worksheet, ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

            StringAssert.Contains("table starting at cell C5 has no headers", generatedCode);
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfNoHeadersForTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\NoHeadersForTable\");

            Assert.AreNotEqual(0, results.ErrorCode);

            StringAssert.Contains("Workbook 'NoHeadersForTable'", results.LogMessages); 

            StringAssert.Contains("Worksheet 'NoHeadersForTable'", results.LogMessages);

            StringAssert.Contains("table starting at cell C5 has no headers", results.LogMessages);
        }
    }
}
