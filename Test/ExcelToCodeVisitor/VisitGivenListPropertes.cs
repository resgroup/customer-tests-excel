using System;
using System.Collections.Generic;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    [TestFixture]
    public class VisitGivenListPropertes : TestBase
    {
        [Test]
        public void ExcelToCodeVisitsListProperties()
        {
            var visitRecorder = new GivenListPropertyVisitRecorder();

            // The GivenListPropertyVisitRecorder outputs the IGivenListProperty for VisitGivenListPropertyDeclaration,
            // and "Finalisation" for VisitGivenListPropertyFinalisation. Everything else is ignored.
            // The values aren't exactly the same as the ones in the spreadsheet, as they are formatted by both
            // ExcelToCode and CodeNameToExcelNameConverter in to the strings that are used in the code.
            // This is a shame, it would be good to make this more loosely coupled, so that we could use a 
            // test formatter, and make the assertions exactly the same as the excel.
            var expected = new List<String>
            {
                "rootList1List, SpecificationSpecificRootList1ClassName",
                    "childList1List, SpecificationSpecificChildList1ClassName", // this indendation is just to make it read more easily
                    "Finalisation",
                "Finalisation",
                "rootList2List, SpecificationSpecificRootList2ClassName",
                "Finalisation"
            };

            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));
            sheetConverter.AddVisitor(visitRecorder);

            using (var workbook = Workbook(@"TestExcelFiles\VisitGivenListProperties.xlsx"))
            {
                sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                CollectionAssert.AreEqual(expected, visitRecorder.RecordedListProperties);
            }
        }
    }
}
