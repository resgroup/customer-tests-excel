using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class ComplexObjectWithinTable : TestBase
    {
        [Test]
        public void SheetConverterAddsWarningCommentIfComplexObjectWithinTable()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            var worksheet = FirstWorksheet(@"TestExcelFiles\ComplexObjectWithinTable\ComplexObjectWithinTable.xlsx");

            string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, worksheet, ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

            StringAssert.Contains("RoundTrippable() => false", generatedCode);

            StringAssert.Contains(
                "complex property ('ComplexObject_of', Row 7, Column 4) within a table",
                generatedCode);
        }

        [Test]
        public void TestProjectCreatorReturnsWarningIfComplexObjectWithinTable()
        {
            var logger = GenerateTestsAndReturnLog(@"TestExcelFiles\ComplexObjectWithinTable\");

            StringAssert.Contains("Workbook 'ComplexObjectWithinTable'", logger.LogMessages); 

            StringAssert.Contains("Worksheet 'ComplexObjectWithinTable'", logger.LogMessages); 

            StringAssert.Contains(
                "complex property ('ComplexObject_of', Row 7, Column 4) within a table",
                logger.LogMessages);
        }
    }
}
