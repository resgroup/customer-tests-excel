using System;
using System.Collections.Generic;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class NoBlankLineBetweenGivenAndWhen : TestBase
    {
        [Test]
        public void SheetConverterAddsWarningCommentIfNoBlankLineBetweenGivenAndWhen()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            var worksheet = FirstWorksheet(@"TestExcelFiles\NoBlankLineBetweenGivenAndWhen\NoBlankLineBetweenGivenAndWhen.xlsx");

            string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, worksheet, ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

            StringAssert.Contains("RoundTrippable() => false", generatedCode);

            StringAssert.Contains(
                "There is no blank line between the end of the Given section (Row 5) and the start of the When section (Row 6) in the Excel test, tab 'NoBlankLineBetweenGivenAndWhen'",
                generatedCode);
        }

        [Test]
        public void TestProjectCreatorReturnsWarningIfNoBlankLineBetweenGivenAndWhen()
        {
            var logger = GenerateTestsAndReturnLog(@"TestExcelFiles\NoBlankLineBetweenGivenAndWhen\");

            StringAssert.Contains("NoBlankLineBetweenGivenAndWhen", logger.Log);
            StringAssert.Contains(
                "There is no blank line between the end of the Given section (Row 5) and the start of the When section (Row 6) in the Excel test, tab 'NoBlankLineBetweenGivenAndWhen'",
                logger.Log);
        }
    }
}
