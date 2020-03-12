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

namespace SampleTests.NameConversions
{
    [TestFixture]
    public class Functions : SpecificationBase<SpecificationSpecificValidator>, ISpecification<SpecificationSpecificValidator>
    {
        public override string Description()
        {
            return "Functions";
        }
        
        // arrange
        public override SpecificationSpecificValidator Given()
        {
            var validator = new SpecificationSpecificValidator();
            validator.WithValidProperties();
            
            return validator;
        }
        
        public override string When(SpecificationSpecificValidator validator)
        {
            validator.Validate();
            return "Validate";
        }
        
        public override IEnumerable<IAssertion<SpecificationSpecificValidator>> Assertions()
        {
            return new List<IAssertion<SpecificationSpecificValidator>>
            {
                  new EqualityAssertion<SpecificationSpecificValidator>(validator => validator.Valid, true)
            };
        }
        
        protected override string AssertionClassPrefixAddedByGenerator => "I";
    }
}
