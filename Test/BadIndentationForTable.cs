using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class BadIndentationForTable : TestBase
    {
        [Test]
        public void SheetConverterShowsErrorIfIndentationBadForTable()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            var worksheet = FirstWorksheet(@"TestExcelFiles\BadIndentationForTable\BadIndentationForTable.xlsx");

            string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, worksheet, ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

            StringAssert.Contains("properties start on column E, whereas they should start start one to the left, on column D", generatedCode);
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfIndentationBadForTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\BadIndentationForTable\");

            Assert.AreNotEqual(0, results.ErrorCode);

            StringAssert.Contains("Workbook 'BadIndentationForTable'", results.LogMessages); 

            StringAssert.Contains("Worksheet 'BadIndentationForTable'", results.LogMessages);

            StringAssert.Contains("properties start on column E, whereas they should start start one to the left, on column D", results.LogMessages);
        }
    }
}
