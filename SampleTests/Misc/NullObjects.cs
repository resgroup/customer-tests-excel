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
    public class NullObjects : SpecificationBase<SpecificationSpecificNullObjectTester>, ISpecification<SpecificationSpecificNullObjectTester>
    {
        public override string Description()
        {
            return "Nulls can be passed as objects";
        }
        
        public override SpecificationSpecificNullObjectTester Given()
        {
            return
                new SpecificationSpecificNullObjectTester()
                
                .HasObjectProperty_of(
                    new SpecificationSpecificHasObjectProperty()
                    
                    .Anything_of(
                        new SpecificationSpecificAnything()
                        .AnInteger_of(1)
                    )
                    .Anything_of(null)
                )
                .HasObjectProperty_of(null)
            ;
        }
        
        public override string When(SpecificationSpecificNullObjectTester nullObjectTester)
        {
            nullObjectTester.Do_Nothing();
            return "Do Nothing";
        }
        
        public override IEnumerable<IAssertion<SpecificationSpecificNullObjectTester>> Assertions()
        {
            return new List<IAssertion<SpecificationSpecificNullObjectTester>>
            {
                  new EqualityAssertion<SpecificationSpecificNullObjectTester>(nullObjectTester => nullObjectTester.A_Result, 1)
            };
        }
        
        protected override string AssertionClassPrefixAddedByGenerator => "I";
    }
}
