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

            // todo
            StringAssert.Contains(
                "There should be exactly one blank line, but there are 2, between the end of the Given section (Row 5) and the start of the When section (Row 8) in the Excel test, tab 'TwoBlankLinesBetweenGivenWhen'",
                generatedCode);
        }

        [Test]
        public void TestProjectCreatorReturnsWarningIfComplexObjectWithinTable()
        {
            var logger = GenerateTestsAndReturnLog(@"TestExcelFiles\ComplexObjectWithinTable\");

            StringAssert.Contains("ComplexObjectWithinTable", logger.Log);
            StringAssert.Contains("ComplexObjectWithinTable", logger.Log); // worksheet name
            //todo
            StringAssert.Contains(
                "There should be exactly one blank line, but there are 2, between the end of the Given section (Row 5) and the start of the When section (Row 8) in the Excel test, tab 'TwoBlankLinesBetweenGivenWhen'",
                logger.Log);
        }
    }
}
