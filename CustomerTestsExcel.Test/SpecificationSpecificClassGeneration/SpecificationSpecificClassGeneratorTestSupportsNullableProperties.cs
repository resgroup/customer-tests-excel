using CustomerTestsExcel.ExcelToCode;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using NUnit.Framework;
using System.Collections.Generic;

namespace CustomerTestsExcel.Test.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificClassGeneratorTestSupportsNullableProperties : TestBase
    {
        interface ITarget
        {
            double? NullableDoubleProperty { get; }
        }

        [Test]
        public void SupportsNullableDoubleProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassSimpleProperty("NullableDoubleProperty", ExcelPropertyType.Number)
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
            @"internal SpecificationSpecificTarget NullableDoubleProperty_of(Double? nullableDoubleProperty)
        {
            AddValueProperty(GetCurrentMethod(), nullableDoubleProperty);

            target.Setup(m => m.NullableDoubleProperty).Returns(nullableDoubleProperty);

            return this;
        }";

            StringAssert.Contains(expectedSetter, actual);
        }

        [Test]
        public void SupportsNullDoubleProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassSimpleProperty("NullableDoubleProperty", ExcelPropertyType.Null)
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
            @"internal SpecificationSpecificTarget NullableDoubleProperty_of(Double? nullableDoubleProperty)
        {
            AddValueProperty(GetCurrentMethod(), nullableDoubleProperty);

            target.Setup(m => m.NullableDoubleProperty).Returns(nullableDoubleProperty);

            return this;
        }";

            StringAssert.Contains(expectedSetter, actual);
        }
    }
}
