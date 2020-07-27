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
using SampleSystemUnderTest.Misc;

namespace SampleTests.Misc
{
    [TestFixture]
    public class RootNullablePrimitives : SpecificationBase<SpecificationSpecificRootObject>, ISpecification<SpecificationSpecificRootObject>
    {
        public override string Description()
        {
            return "Nullability is calculated from values for generated root setup classes";
        }
        
        public override SpecificationSpecificRootObject Given()
        {
            return
                new SpecificationSpecificRootObject()
                .NullableInt_of(1)
                .NullableInt_of(null)
                .Number_of(2.3)
                .NullableString_of("cedd")
                .NullableString_of(null)
            ;
        }
        
        public override string When(SpecificationSpecificRootObject rootObject)
        {
            rootObject.Do_Nothing();
            return "Do Nothing";
        }
        
        public override IEnumerable<IAssertion<SpecificationSpecificRootObject>> Assertions()
        {
            return new List<IAssertion<SpecificationSpecificRootObject>>
            {
                  new EqualityAssertion<SpecificationSpecificRootObject>(rootObject => rootObject.A_Result, 1)
            };
        }
        
        protected override string AssertionClassPrefixAddedByGenerator => "I";
    }
}
