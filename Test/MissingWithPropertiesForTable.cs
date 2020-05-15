using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class MissingWithPropertiesForTable : TestBase
    {
        [Test]
        public void SheetConverterShowsErrorIfWithPropertiesCellMissingForTable()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            using (var workbook = Workbook(@"TestExcelFiles\MissingWithPropertiesForTable\MissingWithPropertiesForTable.xlsx"))
            {
                string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME).Code;

                StringAssert.Contains("table starting at C5", generatedCode);

                StringAssert.Contains("D6 should be 'With Properties', but is 'AnyProperty of'", generatedCode);
            }
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfWithPropertiesCellMissingForTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\MissingWithPropertiesForTable\");

            Assert.AreNotEqual(false, results.HasErrors);

            StringAssert.Contains("Workbook 'MissingWithPropertiesForTable'", results.LogMessages);

            StringAssert.Contains("Worksheet 'MissingWithPropertiesForTable'", results.LogMessages);

            StringAssert.Contains("table starting at C5", results.LogMessages);

            StringAssert.Contains("D6 should be 'With Properties', but is 'AnyProperty of'", results.LogMessages);
        }
    }
}
