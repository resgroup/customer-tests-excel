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
        public void ExcelToCodeVisitsTableSimpleProperties()
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
                    "Table [{ PropertyName: SimpleProperty1 of, EndRow: 7, EndColumn: 4, IsRoundTrippable: True },{ PropertyName: SimpleProperty2 of, EndRow: 7, EndColumn: 5, IsRoundTrippable: True }]",
                        "RowDeclaration 0",
                            "Cell(0, 0)",
                                "SimpleProperty1_of, \"SimpleProperty1Value1\", String",
                            "CellFinalisation",
                            "Cell(0, 1)",
                                "SimpleProperty2_of, \"SimpleProperty2Value1\", String",
                            "CellFinalisation",
                        "RowFinalisation",
                        "RowDeclaration 1",
                            "Cell(1, 0)",
                                "SimpleProperty1_of, \"SimpleProperty1Value2\", String",
                            "CellFinalisation",
                            "Cell(1, 1)",
                                "SimpleProperty2_of, \"SimpleProperty2Value2\", String",
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

                CollectionAssert.AreEqual(expected, visitRecorder.RecordedTableProperties);
            }
        }

        [Test]
        public void ExcelToCodeVisitsTableComplexProperties()
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
                    "Table [{ PropertyName: ComplexProperty_of, EndRow: 10, EndColumn: 5, IsRoundTrippable: False, SubClassName: ComplexClass, StartRow: 7, FullSubClassName: SpecificationSpecificComplexClass, PropertiesStartColumn: 4, PropertiesEndColumn: 5, Headers: [ {[4, { PropertyName: ComplexProperty1 of, EndRow: 10, EndColumn: 4, IsRoundTrippable: True }]},{[5, { PropertyName: ComplexProperty2 of, EndRow: 10, EndColumn: 5, IsRoundTrippable: True }]} ] },{ PropertyName: SimpleProperty of, EndRow: 7, EndColumn: 6, IsRoundTrippable: True }]",
                        "RowDeclaration 0",
                            "Cell(0, 0)",
                                "ComplexProperty_of, SpecificationSpecificComplexClass",
                                    "Cell(0, 0)",
                                        "ComplexProperty1_of, \"ComplexProperty1Value1\", String",
                                    "CellFinalisation",
                                    "Cell(0, 1)",
                                        "ComplexProperty2_of, \"ComplexProperty2Value1\", String",
                                    "CellFinalisation",
                                "ComplexPropertyFinalisation",
                            "CellFinalisation",
                            "Cell(0, 2)",
                                "SimpleProperty_of, \"SimplePropertyValue1\", String",
                            "CellFinalisation",
                        "RowFinalisation",
                        "RowDeclaration 1",
                            "Cell(1, 0)",
                                "ComplexProperty_of, SpecificationSpecificComplexClass",
                                    "Cell(1, 0)",
                                        "ComplexProperty1_of, \"ComplexProperty1Value2\", String",
                                    "CellFinalisation",
                                    "Cell(1, 1)",
                                        "ComplexProperty2_of, \"ComplexProperty2Value2\", String",
                                    "CellFinalisation",
                                "ComplexPropertyFinalisation",
                            "CellFinalisation",
                            "Cell(1, 2)",
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
                sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(1), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                CollectionAssert.AreEqual(expected, visitRecorder.RecordedTableProperties);
            }
        }
    }
}
