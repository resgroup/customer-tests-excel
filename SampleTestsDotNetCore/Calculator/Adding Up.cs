using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CustomerTestsExcel;
using CustomerTestsExcel.Assertions;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using System.Linq.Expressions;
using SampleTests;
using SampleTests.Setup;

using SampleSystemUnderTest;
using SampleSystemUnderTest.AnovaCalculator;
using SampleSystemUnderTest.Routing;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleSystemUnderTest.Calculator;
using SampleSystemUnderTest.CustomProperties;
using SampleSystemUnderTest.Misc;

namespace SampleTests.Calculator
{
    [TestFixture]
    public class Adding_Up : SpecificationBase<SpecificationSpecificCalculator>, ISpecification<SpecificationSpecificCalculator>
    {
        public override string Description()
        {
            return "Add";
        }
        
        public override SpecificationSpecificCalculator Given()
        {
            return
                new SpecificationSpecificCalculator()
                .FirstValue_of(1)
                .SecondValue_of(2)
                .Operation_of(Operation.Add)
            ;
        }
        
        public override string When(SpecificationSpecificCalculator calculator)
        {
            calculator.Perform_Operation();
            return "Perform Operation";
        }
        
        public override IEnumerable<IAssertion<SpecificationSpecificCalculator>> Assertions()
        {
            return new List<IAssertion<SpecificationSpecificCalculator>>
            {
                  new EqualityAssertion<SpecificationSpecificCalculator>(calculator => calculator.Result, 3)
            };
        }
        
        protected override string AssertionClassPrefixAddedByGenerator => "I";
    }
}
