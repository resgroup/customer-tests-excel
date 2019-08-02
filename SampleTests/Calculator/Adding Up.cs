using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CustomerTestsExcel;
using CustomerTestsExcel.Assertions;
using System.Linq.Expressions;
using SampleTests;

using SampleSystemUnderTest;
using SampleTests.IgnoreOnGeneration.AnovaCalculator;
using SampleSystemUnderTest.AnovaCalculator;
using SampleTests.IgnoreOnGeneration.Routing;
using SampleSystemUnderTest.Routing;
using SampleTests.IgnoreOnGeneration.Vermeulen_Near_Wake_Length;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleTests.IgnoreOnGeneration.Calculator;
using SampleSystemUnderTest.Calculator;
using SampleTests.IgnoreOnGeneration.NameConversions;

namespace SampleTests.Calculator
{
    [TestFixture]
    public class Adding_Up : SpecificationBase<SpecificationSpecificCalculator>, ISpecification<SpecificationSpecificCalculator>
    {
        public override string Description()
        {
            return "Add";
        }
        
        // arrange
        public override SpecificationSpecificCalculator Given()
        {
            var calculator = new SpecificationSpecificCalculator();
            calculator.FirstValue_of(1);
            calculator.SecondValue_of(2);
            calculator.Operation_of(Operation.Add);
            
            return calculator;
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
