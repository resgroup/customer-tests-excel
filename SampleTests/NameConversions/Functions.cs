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
using SampleTests.GeneratedSpecificationSpecific;

using SampleSystemUnderTest;
using SampleSystemUnderTest.AnovaCalculator;
using SampleSystemUnderTest.Routing;
using SampleSystemUnderTest.VermeulenNearWakeLength;
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
        
        public override SpecificationSpecificValidator Given()
        {
            return
                new SpecificationSpecificValidator()
                .WithValidProperties()
            ;
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
