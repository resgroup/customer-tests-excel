using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomerTestsExcel.Assertions;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    public class TestSpecification : SpecificationBase<TestReportsSpecificationSetup>
    {
        public TestSpecification()
        {
        }

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

        [Test, Ignore("This class is used as a test helper, rather than a test")]
        public override void RunTests()
        {
            // This class is used as a test helper, rather than a test
            // This seems a bit weird, maybe it points to a wrong / strange design decision
        }
    }
}
