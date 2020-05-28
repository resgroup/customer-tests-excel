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
using SampleSystemUnderTest.CustomProperties;

namespace SampleTests.Misc
{
    [TestFixture]
    public class CustomProperties : SpecificationBase<SpecificationSpecificUnmatchedClass>, ISpecification<SpecificationSpecificUnmatchedClass>
    {
        public override string Description()
        {
            return "Underscores in names are converted to underscores in C#";
        }
        
        public override SpecificationSpecificUnmatchedClass Given()
        {
            return
                new SpecificationSpecificUnmatchedClass()
                
                .ClassWithCustomProperty_of(
                    new SpecificationSpecificClassWithCustomProperty()
                    .CustomInt_of(1)
                )
            ;
        }
        
        public override string When(SpecificationSpecificUnmatchedClass unmatchedClass)
        {
            unmatchedClass.Do_Nothing();
            return "Do Nothing";
        }
        
        public override IEnumerable<IAssertion<SpecificationSpecificUnmatchedClass>> Assertions()
        {
            return new List<IAssertion<SpecificationSpecificUnmatchedClass>>
            {
                  new EqualityAssertion<SpecificationSpecificUnmatchedClass>(unmatchedClass => unmatchedClass.A_Result, 1)
            };
        }
        
        protected override string AssertionClassPrefixAddedByGenerator => "I";
    }
}
