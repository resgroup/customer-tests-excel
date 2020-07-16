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
                    new GivenClass(
                        "ThingToSetup",
                        new List<IGivenClassProperty> {
                            new GivenClassComplexProperty("Root1", "Root1ClassName"),
                            new GivenClassComplexProperty("Root2", "Root2ClassName")
                        },
                        new List<IVisitedGivenSimpleProperty>(),
                        new List<IVisitedGivenComplexProperty>(),
                        new List<IVisitedGivenFunction>(),
                        new List<IVisitedGivenListProperty>(),
                        new List<IVisitedGivenTableProperty>()
                    )
                );

                CollectionAssert.Contains(
                    givenClassRecorder.Classes,
                    new GivenClass("Root1ClassName",
                    new List<IGivenClassProperty> {
                        new GivenClassComplexProperty("Child1", "Child1ClassName")
                        },
                        new List<IVisitedGivenSimpleProperty>(),
                        new List<IVisitedGivenComplexProperty>(),
                        new List<IVisitedGivenFunction>(),
                        new List<IVisitedGivenListProperty>(),
                        new List<IVisitedGivenTableProperty>()
                    )
                );

                CollectionAssert.Contains(
                    givenClassRecorder.Classes,
                    new GivenClass("Child1ClassName",
                    new List<IGivenClassProperty> {
                        new GivenClassSimpleProperty("Property", ExcelPropertyType.String)
                        },
                        new List<IVisitedGivenSimpleProperty>(),
                        new List<IVisitedGivenComplexProperty>(),
                        new List<IVisitedGivenFunction>(),
                        new List<IVisitedGivenListProperty>(),
                        new List<IVisitedGivenTableProperty>()
                    )
                );

                CollectionAssert.Contains(
                    givenClassRecorder.Classes,
                    new GivenClass("Root2ClassName",
                    new List<IGivenClassProperty> {
                        new GivenClassSimpleProperty("Property", ExcelPropertyType.String)
                        },
                        new List<IVisitedGivenSimpleProperty>(),
                        new List<IVisitedGivenComplexProperty>(),
                        new List<IVisitedGivenFunction>(),
                        new List<IVisitedGivenListProperty>(),
                        new List<IVisitedGivenTableProperty>()
                    )
                );
            }
        }

    }
}
