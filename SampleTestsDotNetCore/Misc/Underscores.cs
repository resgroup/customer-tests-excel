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

namespace SampleTests.Misc
{
    [TestFixture]
    public class Underscores : SpecificationBase<SpecificationSpecificUnderscore>, ISpecification<SpecificationSpecificUnderscore>
    {
        public override string Description()
        {
            return "Underscores in names are converted to underscores in C#";
        }
        
        public override SpecificationSpecificUnderscore Given()
        {
            return
                new SpecificationSpecificUnderscore()
                .A_Property_of("Anything")
                .Table_Property_table_of(
                    new ReportSpecificationSetupClassUsingTable<SpecificationSpecificA_Table>()
                    .Add(
                        new SpecificationSpecificA_Table()
                        .A_Property_of("Anything")
                    )
                )
                
                .List_Property_list_of(
                    "SpecificationSpecificA_Table", 
                    new FluentList<SpecificationSpecificA_Table>()
                    .FluentAdd(
                        new SpecificationSpecificA_Table()
                        .A_Property_of("Anything")
                    )
                )
            ;
        }
        
        public override string When(SpecificationSpecificUnderscore underscore)
        {
            underscore.Do_Nothing();
            return "Do Nothing";
        }
        
        public override IEnumerable<IAssertion<SpecificationSpecificUnderscore>> Assertions()
        {
            return new List<IAssertion<SpecificationSpecificUnderscore>>
            {
                  new EqualityAssertion<SpecificationSpecificUnderscore>(underscore => underscore.A_Result, 1)
            };
        }
        
        protected override string AssertionClassPrefixAddedByGenerator => "I";
    }
}
