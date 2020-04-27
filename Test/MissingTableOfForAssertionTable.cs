using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class MissingTableOfForAssertionTable : TestBase
    {
        [Test]
        public void SheetConverterShowsErrorIfTableOfMissingForAssertionTable()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            using (var workbook = Workbook(@"TestExcelFiles\MissingTableOfForAssertionTable\MissingTableOfForAssertionTable.xlsx"))
            {
                string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                StringAssert.Contains("trying to set up a table assertion, starting at cell B8", generatedCode);

                StringAssert.Contains("make sure that cell C8 is 'table of'", generatedCode);
            }
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfTableOfMissingForAssertionTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\MissingTableOfForAssertionTable\");

            Assert.AreNotEqual(false, results.HasErrors);

            StringAssert.Contains("Workbook 'MissingTableOfForAssertionTable'", results.LogMessages); 

            StringAssert.Contains("Worksheet 'MissingTableOfForAssertionTable'", results.LogMessages);

            StringAssert.Contains("trying to set up a table assertion, starting at cell B8", results.LogMessages);

            StringAssert.Contains("make sure that cell C8 is 'table of'", results.LogMessages);

        }
    }
}
