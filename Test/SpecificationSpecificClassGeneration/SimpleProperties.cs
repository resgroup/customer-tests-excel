using System;
using System.Collections.Generic;
using System.Linq;
using CustomerTestsExcel.ExcelToCode;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    [TestFixture]
    public class SimpleProperties : TestBase
    {
        [Test]
        public void GivenClassRecorderRecordsSimpleProperties()
        {
            var givenClassRecorder = new GivenClassRecorder();

            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));
            sheetConverter.AddVisitor(givenClassRecorder);

            using (var workbook = Workbook(@"TestExcelFiles\PropertyTypes.xlsx"))
            {
                sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                Assert.AreEqual(1, givenClassRecorder.Classes.Count);

                var rootClass = givenClassRecorder.Classes.First();
                // probably want the name with and without the "_of". the sut will have it without the _of, but the specification specific class will have it with the of
                // might want to think about functions as well, but this will probably do for now.
                AssertContains(rootClass.Properties, "Null", ExcelPropertyType.Null);
                AssertContains(rootClass.Properties, "StringNull", ExcelPropertyType.StringNull);
                AssertContains(rootClass.Properties, "DateTime", ExcelPropertyType.DateTime);
                AssertContains(rootClass.Properties, "Enum", ExcelPropertyType.Enum);
                AssertContains(rootClass.Properties, "Number", ExcelPropertyType.Number);
                AssertContains(rootClass.Properties, "Decimal", ExcelPropertyType.Decimal);
                AssertContains(rootClass.Properties, "False", ExcelPropertyType.Boolean);
                AssertContains(rootClass.Properties, "True", ExcelPropertyType.Boolean);
                AssertContains(rootClass.Properties, "String", ExcelPropertyType.String);
                AssertContains(rootClass.Properties, "QuotedString", ExcelPropertyType.String);
            }
        }

        static void AssertContains(
            IEnumerable<IGivenClassProperty> properties,
            string propertyOrFunctionName,
            ExcelPropertyType excelPropertyType)
            =>
            CollectionAssert.Contains(
                properties,
                new GivenClassSimpleProperty(
                    propertyOrFunctionName,
                    excelPropertyType
                )
           );
    }
}
