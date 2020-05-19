using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class MissingWithItemForList : TestBase
    {
        [Test]
        public void SheetConverterShowsErrorIfWithItemMissingForList()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            using (var workbook = Workbook(@"TestExcelFiles\MissingWithItemForList\MissingWithItemForList.xlsx"))
            {
                string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME).Code;

                StringAssert.Contains("list property starting at C5", generatedCode);

                StringAssert.Contains("D6 should be 'With Item', but is ''", generatedCode);
            }
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfWithItemMissingForList()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\MissingWithItemForList\");

            Assert.AreNotEqual(false, results.HasErrors);

            StringAssert.Contains("Workbook 'MissingWithItemForList'", results.LogMessages);

            StringAssert.Contains("Worksheet 'MissingWithItemForList'", results.LogMessages);

            StringAssert.Contains("list property starting at C5", results.LogMessages);

            StringAssert.Contains("D6 should be 'With Item', but is ''", results.LogMessages);
        }
    }
}
