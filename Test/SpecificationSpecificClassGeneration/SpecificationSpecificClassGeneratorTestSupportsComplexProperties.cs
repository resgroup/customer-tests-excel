using CustomerTestsExcel.ExcelToCode;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                new ExcelCsharpPropertyMatcher()
                ).cSharpCode(
                    "SampleTests",
                    new List<string>(),
                    typeof(ITarget),
                    excelGivenClass
                );

            var expectedSetter =
            @"internal SpecificationSpecificTarget ComplexProperty_of(SpecificationSpecificTarget complexProperty)
        {
            classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), complexProperty));

            target.Setup(m => m.ComplexProperty).Returns(complexProperty);

            return this;
        }";

            StringAssert.Contains(expectedSetter, actual);
        }

    }
}
