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

namespace SampleTests.PrimitiveLists
{
    [TestFixture]
    public class PrimitiveLists : SpecificationBase<SpecificationSpecificObjectWithPrimiiveLists>, ISpecification<SpecificationSpecificObjectWithPrimiiveLists>
    {
        public override string Description()
        {
            return "Primtive lists are supported";
        }
        
        // arrange
        public override SpecificationSpecificObjectWithPrimiiveLists Given()
        {
            return
                new SpecificationSpecificObjectWithPrimiiveLists()
                .IntegerTableSyntax_table_of(
                    new ReportSpecificationSetupClassUsingTable<SpecificationSpecificInteger>()
                    .Add(
                        new SpecificationSpecificInteger()
                        .Integer_of(1)
                    )
                    .Add(
                        new SpecificationSpecificInteger()
                        .Integer_of(2)
                    )
                )
                
                .IntegerListSyntax_list_of(
                    "SpecificationSpecificInteger", 
                    new FluentList<SpecificationSpecificInteger>()
                    .FluentAdd(
                        new SpecificationSpecificInteger()
                        .Integer_of(3)
                    )
                    .FluentAdd(
                        new SpecificationSpecificInteger()
                        .Integer_of(3)
                    )
                )
                .FloatTableSyntax_table_of(
                    new ReportSpecificationSetupClassUsingTable<SpecificationSpecificFloat>()
                    .Add(
                        new SpecificationSpecificFloat()
                        .Float_of(1.1)
                    )
                    .Add(
                        new SpecificationSpecificFloat()
                        .Float_of(2.2)
                    )
                )
                
                .FloatListSyntax_list_of(
                    "SpecificationSpecificFloat", 
                    new FluentList<SpecificationSpecificFloat>()
                    .FluentAdd(
                        new SpecificationSpecificFloat()
                        .Float_of(3.3)
                    )
                    .FluentAdd(
                        new SpecificationSpecificFloat()
                        .Float_of(3.3)
                    )
                )
                .StringTableSyntax_table_of(
                    new ReportSpecificationSetupClassUsingTable<SpecificationSpecificString>()
                    .Add(
                        new SpecificationSpecificString()
                        .String_of("s1")
                    )
                    .Add(
                        new SpecificationSpecificString()
                        .String_of("s2")
                    )
                )
                
                .StringListSyntax_list_of(
                    "SpecificationSpecificString", 
                    new FluentList<SpecificationSpecificString>()
                    .FluentAdd(
                        new SpecificationSpecificString()
                        .String_of("s3")
                    )
                    .FluentAdd(
                        new SpecificationSpecificString()
                        .String_of("s4")
                    )
                )
                .DateTimeTableSyntax_table_of(
                    new ReportSpecificationSetupClassUsingTable<SpecificationSpecificDateTime>()
                    .Add(
                        new SpecificationSpecificDateTime()
                        .DateTime_of(DateTime.Parse("2020-01-01T00:00:00"))
                    )
                    .Add(
                        new SpecificationSpecificDateTime()
                        .DateTime_of(DateTime.Parse("2020-01-02T00:00:00"))
                    )
                )
                
                .DateTimeListSyntax_list_of(
                    "SpecificationSpecificDateTime", 
                    new FluentList<SpecificationSpecificDateTime>()
                    .FluentAdd(
                        new SpecificationSpecificDateTime()
                        .DateTime_of(DateTime.Parse("2020-01-03T00:00:00"))
                    )
                    .FluentAdd(
                        new SpecificationSpecificDateTime()
                        .DateTime_of(DateTime.Parse("2020-01-04T00:00:00"))
                    )
                )
            ;
            
        }
        
        public override string When(SpecificationSpecificObjectWithPrimiiveLists objectWithPrimiiveLists)
        {
            objectWithPrimiiveLists.Do_Nothing();
            return "Do Nothing";
        }
        
        public override IEnumerable<IAssertion<SpecificationSpecificObjectWithPrimiiveLists>> Assertions()
        {
            return new List<IAssertion<SpecificationSpecificObjectWithPrimiiveLists>>
            {
                  new EqualityAssertion<SpecificationSpecificObjectWithPrimiiveLists>(objectWithPrimiiveLists => objectWithPrimiiveLists.Result, 1)
            };
        }
        
        protected override string AssertionClassPrefixAddedByGenerator => "I";
    }
}
