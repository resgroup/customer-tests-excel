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
            // The values aren't exactly the same as the ones in the spreadsheet, as they are formatted by both
            // ExcelToCode and CodeNameToExcelNameConverter in to the strings that are used in the code.
            // This is a shame, it would be good to make this more loosely coupled, so that we could use a 
            // test formatter, and make the assertions exactly the same as the excel.
            var expected = new List<String>
            {
                // the indendation is just to make it read more easily
                "thingToSetup, SpecificationSpecificThingToSetup",
                    "root1ClassName, SpecificationSpecificRoot1ClassName",
                        "child1ClassName, SpecificationSpecificChild1ClassName",
                        "Finalisation",
                    "Finalisation",
                    "root2ClassName, SpecificationSpecificRoot2ClassName",
                    "Finalisation",
                "Finalisation"
            };

            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));
            sheetConverter.AddVisitor(visitRecorder);

            using (var workbook = Workbook(@"TestExcelFiles\VisitGivenComplexProperties.xlsx"))
            {
                sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                CollectionAssert.AreEqual(expected, visitRecorder.RecordedComplexProperties);
            }
        }
    }
}
