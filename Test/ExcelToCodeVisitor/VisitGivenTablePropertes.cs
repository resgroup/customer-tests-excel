using System;
using System.Collections.Generic;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    [TestFixture]
    public class VisitGivenTablePropertes : TestBase
    {
        [Test]
        public void ExcelToCodeVisitsTableProperties()
        {
            var visitRecorder = new GivenTablePropertyVisitRecorder();

            // The GivenTablePropertyVisitRecorder outputs strings for the various visits encountered,
            // The values aren't exactly the same as the ones in the spreadsheet, as they are formatted by both
            // ExcelToCode and CodeNameToExcelNameConverter in to the strings that are used in the code.
            // This is a shame, it would be good to make this more loosely coupled, so that we could use a 
            // test formatter, and make the assertions exactly the same as the excel.
            var expected = new List<string>
            {
                "thingToSetup, SpecificationSpecificThingToSetup",
                    // need to fix headers.tostring() to make it look better here
                    "Table System.Collections.Generic.Dictionary`2+ValueCollection[System.UInt32,CustomerTestsExcel.ExcelToCode.TableHeader]",
                        "RowDeclaration 0",
                            "Cell(0, 0) CustomerTestsExcel.ExcelToCode.SubClassTableHeader",
                                "complexPropertyOf, SpecificationSpecificComplexClass",
                                    "ComplexProperty1_of, \"ComplexProperty1Value1\", String",
                                    "ComplexProperty2_of, \"ComplexProperty2Value1\", String",
                                "ComplexPropertyFinalisation",
                            "CellFinalisation",
                            "Cell(0, 2) CustomerTestsExcel.ExcelToCode.PropertyTableHeader",
                                "SimpleProperty_of, \"SimplePropertyValue1\", String",
                            "CellFinalisation",
                        "RowFinalisation",
                        "RowDeclaration 1",
                            "Cell(1, 0) CustomerTestsExcel.ExcelToCode.SubClassTableHeader",
                                "complexPropertyOf, SpecificationSpecificComplexClass",
                                    "ComplexProperty1_of, \"ComplexProperty1Value2\", String",
                                    "ComplexProperty2_of, \"ComplexProperty2Value2\", String",
                                "ComplexPropertyFinalisation",
                            "CellFinalisation",
                            "Cell(1, 2) CustomerTestsExcel.ExcelToCode.PropertyTableHeader",
                                "SimpleProperty_of, \"SimplePropertyValue2\", String",
                            "CellFinalisation",
                        "RowFinalisation",
                    "TableFinalisation",
                "ComplexPropertyFinalisation"
            };

            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));
            sheetConverter.AddVisitor(visitRecorder);

            using (var workbook = Workbook(@"TestExcelFiles\VisitGivenTableProperties.xlsx"))
            {
                sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                Assert.AreEqual("", string.Join("\",\n\"", visitRecorder.RecordedTableProperties));
                CollectionAssert.AreEqual(expected, visitRecorder.RecordedTableProperties);
            }
        }
    }
}
