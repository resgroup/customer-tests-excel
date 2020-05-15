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

            using (var workbook = Workbook(@"TestExcelFiles\ComplexObjectWithinTable\ComplexObjectWithinTable.xlsx"))
            {
                string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME).Code;

                StringAssert.Contains("RoundTrippable() => false", generatedCode);

                StringAssert.Contains(
                    "complex property ('ComplexObject of', cell D7) within a table",
                    generatedCode);
            }
        }

        [Test]
        public void TestProjectCreatorReturnsWarningIfComplexObjectWithinTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\ComplexObjectWithinTable\");

            StringAssert.Contains("Workbook 'ComplexObjectWithinTable'", results.LogMessages);

            StringAssert.Contains("Worksheet 'ComplexObjectWithinTable'", results.LogMessages);

            StringAssert.Contains(
                "complex property ('ComplexObject of', cell D7) within a table",
                results.LogMessages);
        }
    }
}
