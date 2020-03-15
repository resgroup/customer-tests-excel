using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CustomerTestsExcel;
using CustomerTestsExcel.Assertions;
using System.Linq.Expressions;
using SampleTests;
using SampleTests.GeneratedSpecificationSpecific;

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

namespace SampleTests.NameConversions
{
    [TestFixture]
    public class Underscores : SpecificationBase<SpecificationSpecificUnderscore>, ISpecification<SpecificationSpecificUnderscore>
    {
        public override string Description()
        {
            return "Underscores in names are converted to underscores in C#";
        }
        
        // arrange
        public override SpecificationSpecificUnderscore Given()
        {
            var underscore = new SpecificationSpecificUnderscore();
            underscore.A_Property_of("Anything");
            {
                var Table_PropertyRow = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificA_Table>();
                {
                    var table_PropertyRow = new SpecificationSpecificA_Table();
                    table_PropertyRow.A_Property_of("Anything");
                    Table_PropertyRow.Add(table_PropertyRow);
                }
                underscore.Table_Property_table_of(Table_PropertyRow);
            }
            
            {
                var list_PropertyList = new List<SpecificationSpecificA_Table>();
                {
                    var list_Property = new SpecificationSpecificA_Table();
                    list_Property.A_Property_of("Anything");
                    list_PropertyList.Add(list_Property);
                }
                underscore.List_Property_list_of(list_PropertyList, "SpecificationSpecificA_Table");
            }
            
            return underscore;
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
