using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class MispeltTableOfForAssertionTable : TestBase
    {
        [Test]
        public void SheetConverterShowsErrorIfTableOfMispeltForAssertionTable()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            using (var workbook = Workbook(@"TestExcelFiles\MispeltTableOfForAssertionTable\MispeltTableOfForAssertionTable.xlsx"))
            {
                string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                StringAssert.Contains("trying to set up a table assertion, starting at cell B8", generatedCode);

                StringAssert.Contains("make sure that cell C8 is 'table of'", generatedCode);
            }
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfTableOfMispeltForAssertionTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\MispeltTableOfForAssertionTable\");

            Assert.AreNotEqual(0, results.ErrorCode);

            StringAssert.Contains("Workbook 'MispeltTableOfForAssertionTable'", results.LogMessages); 

            StringAssert.Contains("Worksheet 'MispeltTableOfForAssertionTable'", results.LogMessages);

            StringAssert.Contains("trying to set up a table assertion, starting at cell B8", results.LogMessages);

            StringAssert.Contains("make sure that cell C8 is 'table of'", results.LogMessages);

        }
    }
}
