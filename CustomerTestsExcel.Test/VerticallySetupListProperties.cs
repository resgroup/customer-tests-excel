﻿using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class VerticallySetupListProperties : TestBase
    {
        [Test]
        public void ListOfSyntaxCreatesListOfPropertiesWithSameType()
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));

            using (var workbook = Workbook(@"TestExcelFiles\VerticallySetupListProperties.xlsx"))
            {
                string generatedCode = sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME).Code;
                
                // This doesn't guarantee that everything is generated correctly, but it should catch most problems
                // The End To End tests in the SampleTests projects that get generated and run during the build should catch anything that gets through the gaps.
                StringAssert.Contains(".ListProperty_list_of(", generatedCode);
                StringAssert.Contains("\"SpecificationSpecificListType\",", generatedCode);
                StringAssert.Contains("new FluentList<SpecificationSpecificListType>()", generatedCode);
                StringAssert.Contains(".FluentAdd(", generatedCode);
                StringAssert.Contains("new SpecificationSpecificListType()", generatedCode);
                StringAssert.Contains(".Property1_of(1.1)", generatedCode);
                StringAssert.Contains(".Property2_of(1.2)", generatedCode);
                StringAssert.Contains(".Property1_of(2.1)", generatedCode);
                StringAssert.Contains(".Property2_of(2.2)", generatedCode);
            }
        }
    }
}
