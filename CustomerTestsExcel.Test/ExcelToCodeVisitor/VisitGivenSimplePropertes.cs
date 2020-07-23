using System;
using System.Collections.Generic;
using CustomerTestsExcel.ExcelToCode;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    [TestFixture]
    public class VisitGivenSimpleProperties : TestBase
    {
        [Test]
        public void ExcelToCodeVisitsSimpleProperties()
        {
            var visitRecorder = new GivenSimplePropertyVisitRecorder();

            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));
            sheetConverter.AddVisitor(visitRecorder);

            using (var workbook = Workbook(@"TestExcelFiles\PropertyTypes.xlsx"))
            {
                sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                AssertContains(visitRecorder, "Null", "null", ExcelPropertyType.Null);
                AssertContains(visitRecorder, "Null", "null", ExcelPropertyType.Null);
                AssertContains(visitRecorder, "DateTime", "DateTime.Parse(\"2019-01-01T00:00:00\")", ExcelPropertyType.DateTime);
                AssertContains(visitRecorder, "Enum", "EnumType.EnumValue", ExcelPropertyType.Enum);
                AssertContains(visitRecorder, "Number", "1", ExcelPropertyType.Number);
                AssertContains(visitRecorder, "Decimal", "1m", ExcelPropertyType.Decimal);
                AssertContains(visitRecorder, "False", "false", ExcelPropertyType.Boolean);
                AssertContains(visitRecorder, "True", "true", ExcelPropertyType.Boolean);
                AssertContains(visitRecorder, "String", "\"hello\"", ExcelPropertyType.String);
                AssertContains(visitRecorder, "QuotedString", "\"1\"", ExcelPropertyType.String);
            }
        }

        static void AssertContains(
            GivenSimplePropertyVisitRecorder visitRecorder,
            string propertyOrFunctionName,
            string cSharpCodeRepresentation,
            ExcelPropertyType excelPropertyType)
            =>
            CollectionAssert.Contains(
                visitRecorder.RecordedSimpleProperties,
                new VisitedGivenSimpleProperty(
                    propertyOrFunctionName,
                    cSharpCodeRepresentation,
                    excelPropertyType
                )
           );
    }
}
