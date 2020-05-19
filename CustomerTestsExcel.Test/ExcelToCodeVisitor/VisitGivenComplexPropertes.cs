using System;
using System.Collections.Generic;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    [TestFixture]
    public class VisitGivenComplexPropertes : TestBase
    {
        [Test]
        public void ExcelToCodeVisitsComplexProperties()
        {
            var visitRecorder = new GivenComplexPropertyVisitRecorder();

            // The GivenListPropertyVisitRecorder outputs the IGivenListProperty for VisitGivenListPropertyDeclaration,
            // and "Finalisation" for VisitGivenListPropertyFinalisation. Everything else is ignored.
            var expected = new List<String>
            {
                // the indendation is just to make it read more easily
                "ThingToSetup",
                    "Root1, Root1ClassName",
                        "Child1, Child1ClassName",
                        "Finalisation",
                    "Finalisation",
                    "Root2, Root2ClassName",
                    "Finalisation",
                "Finalisation"
            };

            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));
            sheetConverter.log.AddVisitor(visitRecorder);

            using (var workbook = Workbook(@"TestExcelFiles\VisitGivenComplexProperties.xlsx"))
            {
                sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                CollectionAssert.AreEqual(expected, visitRecorder.RecordedComplexProperties);
            }
        }
    }
}
