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

            var worksheet = FirstWorksheet(@"TestExcelFiles\MissingWithPropertiesForTable\MissingWithPropertiesForTable.xlsx");

            string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, worksheet, ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

            StringAssert.Contains("D6 should be 'With Properties', but is 'AnyProperty of'", generatedCode);
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfWithPropertiesCellMissingForTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\MissingWithPropertiesForTable\");

            Assert.AreNotEqual(0, results.ErrorCode);

            StringAssert.Contains("Workbook 'MissingWithPropertiesForTable'", results.LogMessages); 

            StringAssert.Contains("Worksheet 'MissingWithPropertiesForTable'", results.LogMessages); 

            StringAssert.Contains("D6 should be 'With Properties', but is 'AnyProperty of'", results.LogMessages);
            
        }
    }
}
