﻿using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class NoRowsForAssertionTable : TestBase
    {
        [Test]
        public void SheetConverterShowsErrorIfNoRowsForAssertionTable()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            using (var workbook = Workbook(@"TestExcelFiles\NoRowsForAssertionTable\NoRowsForAssertionTable.xlsx"))
            {
                var generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                StringAssert.Contains("assertion table starting at cell B8 has no rows", generatedCode.Code);

                StringAssert.Contains("row of Property Values starting at D12", generatedCode.Code);
            }
        }

        [Test]
        public void TestProjectCreatorShowsErrorIfNoRowsForAssertionTable()
        {
            var results = GenerateTestsAndReturnResults(@"TestExcelFiles\NoRowsForAssertionTable\");

            Assert.AreNotEqual(false, results.HasErrors);

            StringAssert.Contains("Workbook 'NoRowsForAssertionTable'", results.LogMessages); 

            StringAssert.Contains("Worksheet 'NoRowsForAssertionTable'", results.LogMessages);

            StringAssert.Contains("assertion table starting at cell B8 has no rows", results.LogMessages);

            StringAssert.Contains("row of Property Values starting at D12", results.LogMessages);
        }
    }
}
