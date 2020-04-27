using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class BadIndentationForAssertionTable : TestBase
    {
        [Test]
        public void SheetConverterShowsErrorIfIndentationBadForAssertionTable()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            using (var workbook = Workbook(@"TestExcelFiles\BadIndentationForAssertionTable\BadIndentationForAssertionTable.xlsx"))
            {
                string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                StringAssert.Contains("assertion table starting at B8", generatedCode);

                StringAssert.Contains("properties start on column E, but they should start one to the left, on column D", generatedCode);
            }
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfIndentationBadForAssertionTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\BadIndentationForAssertionTable\");

            Assert.AreNotEqual(false, results.HasErrors);

            StringAssert.Contains("Workbook 'BadIndentationForAssertionTable'", results.LogMessages); 

            StringAssert.Contains("Worksheet 'BadIndentationForAssertionTable'", results.LogMessages);

            StringAssert.Contains("assertion table starting at B8", results.LogMessages);

            StringAssert.Contains("properties start on column E, but they should start one to the left, on column D", results.LogMessages);
        }
    }
}
