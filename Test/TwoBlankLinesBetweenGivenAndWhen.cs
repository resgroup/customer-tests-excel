using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class TwoBlankLinesBetweenGivenAndWhen : TestBase
    {
        [Test]
        public void SheetConverterAddsWarningCommentIfMultipleBlankLinesBetweenGivenAndWhen()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            var worksheet = FirstWorksheet(@"TestExcelFiles\TwoBlankLinesBetweenGivenAndWhen\TwoBlankLinesBetweenGivenAndWhen.xlsx");

            string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, worksheet, ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

            StringAssert.Contains("RoundTrippable() => false", generatedCode);

            StringAssert.Contains(
                "There should be exactly one blank line, but there are 2, between the end of the Given section (Row 5) and the start of the When section (Row 8) in the Excel test, tab 'TwoBlankLinesBetweenGivenWhen'",
                generatedCode);
        }

        [Test]
        public void TestProjectCreatorReturnsWarningIfMultipleBlankLinesBetweenGivenAndWhen()
        {
            var logger = GenerateTestsAndReturnLog(@"TestExcelFiles\TwoBlankLinesBetweenGivenAndWhen\");

            StringAssert.Contains("TwoBlankLinesBetweenGivenAndWhen", logger.Log);
            StringAssert.Contains("TwoBlankLinesBetweenGivenWhen", logger.Log); // worksheet name
            StringAssert.Contains(
                "There should be exactly one blank line, but there are 2, between the end of the Given section (Row 5) and the start of the When section (Row 8) in the Excel test, tab 'TwoBlankLinesBetweenGivenWhen'",
                logger.Log);
        }
    }
}
