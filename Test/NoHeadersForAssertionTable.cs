using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class NoHeadersForAssertionTable : TestBase
    {
        [Test]
        public void SheetConverterShowsErrorIfNoHeadersForAssertionTable()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            using (var workbook = Workbook(@"TestExcelFiles\NoHeadersForAssertionTable\NoHeadersForAssertionTable.xlsx"))
            {
                string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                StringAssert.Contains("assertion table starting at cell B8 has no headers", generatedCode);
            }
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfNoHeadersForAssertionTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\NoHeadersForAssertionTable\");

            Assert.AreNotEqual(false, results.HasErrors);

            StringAssert.Contains("Workbook 'NoHeadersForAssertionTable'", results.LogMessages); 

            StringAssert.Contains("Worksheet 'NoHeadersForAssertionTable'", results.LogMessages);

            StringAssert.Contains("assertion table starting at cell B8 has no headers", results.LogMessages);
        }
    }
}
