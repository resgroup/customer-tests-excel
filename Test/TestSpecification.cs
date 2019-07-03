using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomerTestsExcel.Assertions;

namespace CustomerTestsExcel.Test
{
    public class TestSpecification : SpecificationBase<TestReportsSpecificationSetup>
    {
        public TestReportsSpecificationSetup GivenProperties { get; }

        public TestSpecification(TestReportsSpecificationSetup givenProperties)
        {
            GivenProperties = givenProperties;
        }

        public override TestReportsSpecificationSetup Given() =>
            GivenProperties;

        public override string When(TestReportsSpecificationSetup sut) =>
            "";

        public override IEnumerable<IAssertion<TestReportsSpecificationSetup>> Assertions() =>
            new List<IAssertion<TestReportsSpecificationSetup>>();

        public override string Description() => "";

        protected override string AssertionClassPrefixAddedByGenerator => "";
    }
}
