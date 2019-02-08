using System;
using System.Collections.Generic;
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

            var worksheet = FirstWorksheet(@"TestExcelFiles\NoBlankLineBetweenGivenAndWhen.xlsx");

            string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, worksheet, ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

            StringAssert.Contains("RoundTrippable() => false", generatedCode);

            StringAssert.Contains(
                "There is no blank line between the end of the Given section (Row 5) and the start of the When section (Row 6) in the Excel test, tab 'NoBlankLineBetweenGivenAndWhen'",
                generatedCode);
        }

        [Test, Ignore("need to do")]
        public void TestProjectCreatorReturnsWarningIfNoBlankLineBetweenGivenAndWhen()
        {
            // this looks at all excel files in folder: Path.Combine(specificationFolder, "ExcelTests")
            // might want to change this a little, maybe give it a list of filenames
            // this pushes some complexity a bit higher, but only a little bit, and the class does too much already anyway.
        }
    }
}
