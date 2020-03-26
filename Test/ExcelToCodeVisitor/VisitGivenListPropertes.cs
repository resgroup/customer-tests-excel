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
            var expected = new List<String>
            {
                "RootList1, RootList1ClassName",
                    "ChildList1, ChildList1ClassName", // this indendation is just to make it read more easily
                    "Finalisation",
                "Finalisation",
                "RootList2, RootList2ClassName",
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
