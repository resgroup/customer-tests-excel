using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class MissingListOfForList : TestBase
    {
        [Test]
        public void SheetConverterShowsErrorIfListOfMissingForList()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            using (var workbook = Workbook(@"TestExcelFiles\MissingListOfForList\MissingListOfForList.xlsx"))
            {
                string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                StringAssert.Contains("trying to set up a list property, starting at cell C5", generatedCode);
            }
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfListOfMissingForList()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\MissingListOfForList\");

            Assert.AreNotEqual(0, results.ErrorCode);

            StringAssert.Contains("Workbook 'MissingListOfForList'", results.LogMessages);

            StringAssert.Contains("Worksheet 'MissingListOfForList'", results.LogMessages);

            StringAssert.Contains("trying to set up a list property, starting at cell C5", results.LogMessages);
        }
    }
}
