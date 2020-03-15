using System;
using System.Collections.Generic;
using System.Linq;
using CustomerTestsExcel.ExcelToCode;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    [TestFixture]
    public class TableProperties : TestBase
    {
        [Test]
        public void GivenClassRecorderRecordsTableProperties()
        {
            var givenClassRecorder = new GivenClassRecorder();

            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));
            sheetConverter.AddVisitor(givenClassRecorder);

            using (var workbook = Workbook(@"TestExcelFiles\VisitGivenTableProperties.xlsx"))
            {
                sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(1), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                CollectionAssert.Contains(
                    givenClassRecorder.Classes,
                    new GivenClass("ThingToSetup",
                    new List<IGivenClassProperty> {
                        new GivenClassComplexListProperty("TableProperty", "TableClass"),
                    })
                );

                CollectionAssert.Contains(
                    givenClassRecorder.Classes,
                    new GivenClass("TableClass",
                    new List<IGivenClassProperty> {
                        new GivenClassComplexProperty("ComplexProperty of", "ComplexClass"),
                        new GivenClassSimpleProperty("SimpleProperty", ExcelPropertyType.String)
                    })
                );

                CollectionAssert.Contains(
                    givenClassRecorder.Classes,
                    new GivenClass("ComplexClass",
                    new List<IGivenClassProperty> {
                        new GivenClassSimpleProperty("ComplexProperty1", ExcelPropertyType.String),
                        new GivenClassSimpleProperty("ComplexProperty2", ExcelPropertyType.String)
                    })
                );
            }
        }

    }
}
