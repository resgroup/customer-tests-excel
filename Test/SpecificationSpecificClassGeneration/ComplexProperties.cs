using System;
using System.Collections.Generic;
using System.Linq;
using CustomerTestsExcel.ExcelToCode;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    [TestFixture]
    public class ComplexProperties : TestBase
    {
        [Test]
        public void GivenClassRecorderRecordsComplexProperties()
        {
            var givenClassRecorder = new GivenClassRecorder();

            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));
            sheetConverter.AddVisitor(givenClassRecorder);

            using (var workbook = Workbook(@"TestExcelFiles\VisitGivenComplexProperties.xlsx"))
            {
                sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                CollectionAssert.Contains(
                    givenClassRecorder.Classes,
                    new GivenClass("ThingToSetup",
                    new List<GivenClassProperty> {
                        new GivenClassProperty("Root1 of", ExcelPropertyType.Object, "Root1ClassName"),
                        new GivenClassProperty("Root2 of", ExcelPropertyType.Object, "Root2ClassName")
                    })
                );

                CollectionAssert.Contains(
                    givenClassRecorder.Classes,
                    new GivenClass("Root1ClassName",
                    new List<GivenClassProperty> {
                        new GivenClassProperty("Child1 of", ExcelPropertyType.Object, "Child1ClassName")
                    })
                );

                CollectionAssert.Contains(
                    givenClassRecorder.Classes,
                    new GivenClass("Child1ClassName",
                    new List<GivenClassProperty> {
                        new GivenClassProperty("Property of", ExcelPropertyType.String)
                    })
                );

                CollectionAssert.Contains(
                    givenClassRecorder.Classes,
                    new GivenClass("Root2ClassName",
                    new List<GivenClassProperty> {
                        new GivenClassProperty("Property of", ExcelPropertyType.String)
                    })
                );
            }
        }

    }
}
