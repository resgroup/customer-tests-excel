using System;
using System.Collections.Generic;
using System.Linq;
using CustomerTestsExcel.ExcelToCode;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    [TestFixture]
    public class ListProperties : TestBase
    {
        [Test]
        public void GivenClassRecorderRecordsListProperties()
        {
            var givenClassRecorder = new GivenClassRecorder();

            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter(ANY_STRING));
            sheetConverter.AddVisitor(givenClassRecorder);

            using (var workbook = Workbook(@"TestExcelFiles\VisitGivenListProperties.xlsx"))
            {
                sheetConverter.GenerateCSharpTestCode(NO_USINGS, workbook.GetPage(0), ANY_ROOT_NAMESPACE, ANY_WORKBOOKNAME);

                CollectionAssert.Contains(
                    givenClassRecorder.Classes,
                    new GivenClass(
                        "ThingToSetup",
                        new List<IGivenClassProperty> {
                            new GivenClassComplexListProperty("RootList1", "RootList1ClassName"),
                            new GivenClassComplexListProperty("RootList2", "RootList2ClassName")
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
                    new GivenClass(
                        "RootList1ClassName",
                        new List<IGivenClassProperty> {
                            new GivenClassComplexListProperty("ChildList1", "ChildList1ClassName")
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
                    new GivenClass(
                        "ChildList1ClassName",
                        new List<IGivenClassProperty> {
                            new GivenClassSimpleProperty("Property", ExcelPropertyType.String)
                        },
                        new List<IVisitedGivenSimpleProperty>(),
                        new List<IVisitedGivenComplexProperty>(),
                        new List<IVisitedGivenFunction>(),
                        new List<IVisitedGivenListProperty>(),
                        new List<IVisitedGivenTableProperty>())
                );

                CollectionAssert.Contains(
                    givenClassRecorder.Classes,
                    new GivenClass(
                        "RootList2ClassName",
                        new List<IGivenClassProperty> {
                            new GivenClassSimpleProperty("Property", ExcelPropertyType.String)
                        },
                        new List<IVisitedGivenSimpleProperty>(),
                        new List<IVisitedGivenComplexProperty>(),
                        new List<IVisitedGivenFunction>(),
                        new List<IVisitedGivenListProperty>(),
                        new List<IVisitedGivenTableProperty>())
                );
            }
        }

    }
}
