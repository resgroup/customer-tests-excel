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

            using (var workbook = Workbook(@"TestExcelFiles\BadIndentationForTable\BadIndentationForTable.xlsx"))
            {
                string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                StringAssert.Contains("table starting at C5", generatedCode);

                StringAssert.Contains("properties start on column E, but they should start start one to the left, on column D", generatedCode);
            }
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfIndentationBadForTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\BadIndentationForTable\");

            Assert.AreNotEqual(0, results.ErrorCode);

            StringAssert.Contains("Workbook 'BadIndentationForTable'", results.LogMessages); 

            StringAssert.Contains("Worksheet 'BadIndentationForTable'", results.LogMessages);

            StringAssert.Contains("table starting at C5", results.LogMessages);

            StringAssert.Contains("properties start on column E, but they should start start one to the left, on column D", results.LogMessages);
        }
    }
}
