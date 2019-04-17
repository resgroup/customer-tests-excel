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

            using (var workbook = Workbook(@"TestExcelFiles\NoBlankLineBetweenGivenAndWhen\NoBlankLineBetweenGivenAndWhen.xlsx"))
            {
                string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                StringAssert.Contains("RoundTrippable() => false", generatedCode);

                StringAssert.Contains(
                    "no blank line between the end of the Given section (Row 5) and the start of the When section (Row 6)",
                    generatedCode);
            }
        }

        [Test]
        public void TestProjectCreatorReturnsWarningIfNoBlankLineBetweenGivenAndWhen()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\NoBlankLineBetweenGivenAndWhen\");

            StringAssert.Contains("Workbook 'NoBlankLineBetweenGivenAndWhen'", results.LogMessages);

            StringAssert.Contains("Worksheet 'NoBlankLineBetweenGivenAndWhen'", results.LogMessages);

            StringAssert.Contains(
                "no blank line between the end of the Given section (Row 5) and the start of the When section (Row 6)",
                results.LogMessages);
        }
    }
}
