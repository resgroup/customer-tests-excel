using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using NUnit.Framework;
using System.Collections.Generic;

namespace CustomerTestsExcel.Test.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificClassGeneratorTestSupportsComplexProperties : TestBase
    {
        interface ITarget
        {
            ITarget ComplexProperty { get; }
        }

        [Test]
        public void SupportsComplexProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexProperty("ComplexProperty", "Target")
            );

            var actual = new SpecificationSpecificClassGenerator(
                new ExcelCsharpPropertyMatcher(),
                excelGivenClass
                ).CsharpCode(
                    "SampleTests",
                    new List<string>(),
                    typeof(ITarget)
                );

            var expectedSetter =
            @"internal SpecificationSpecificTarget ComplexProperty_of(SpecificationSpecificTarget complexProperty)
        {
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), complexProperty));

            target.Setup(m => m.ComplexProperty).Returns(complexProperty.ComplexProperty);

            return this;
        }";

            StringAssert.Contains(expectedSetter, actual);
        }

    }
}
