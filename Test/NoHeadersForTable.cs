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

            using (var workbook = Workbook(@"TestExcelFiles\NoHeadersForTable\NoHeadersForTable.xlsx"))
            {
                string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                StringAssert.Contains("table starting at cell C5 has no headers", generatedCode);
            }
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfNoHeadersForTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\NoHeadersForTable\");

            Assert.AreNotEqual(false, results.HasErrors);

            StringAssert.Contains("Workbook 'NoHeadersForTable'", results.LogMessages); 

            StringAssert.Contains("Worksheet 'NoHeadersForTable'", results.LogMessages);

            StringAssert.Contains("table starting at cell C5 has no headers", results.LogMessages);
        }
    }
}
