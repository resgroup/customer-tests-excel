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
            var objectWithPrimiiveLists = new SpecificationSpecificObjectWithPrimiiveLists();
            {
                var IntegerTableSyntaxRow = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificInteger>();
                {
                    var integerTableSyntaxRow = new SpecificationSpecificInteger();
                    integerTableSyntaxRow.Integer_of(1);
                    IntegerTableSyntaxRow.Add(integerTableSyntaxRow);
                }
                {
                    var integerTableSyntaxRow = new SpecificationSpecificInteger();
                    integerTableSyntaxRow.Integer_of(2);
                    IntegerTableSyntaxRow.Add(integerTableSyntaxRow);
                }
                objectWithPrimiiveLists.IntegerTableSyntax_table_of(IntegerTableSyntaxRow);
            }
            
            {
                var integerListSyntaxList = new List<SpecificationSpecificInteger>();
                {
                    var integerListSyntax = new SpecificationSpecificInteger();
                    integerListSyntax.Integer_of(3);
                    integerListSyntaxList.Add(integerListSyntax);
                }
                {
                    var integerListSyntax = new SpecificationSpecificInteger();
                    integerListSyntax.Integer_of(3);
                    integerListSyntaxList.Add(integerListSyntax);
                }
                objectWithPrimiiveLists.IntegerListSyntax_list_of(integerListSyntaxList, "SpecificationSpecificInteger");
            }
            {
                var FloatTableSyntaxRow = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificFloat>();
                {
                    var floatTableSyntaxRow = new SpecificationSpecificFloat();
                    floatTableSyntaxRow.Float_of(1.1);
                    FloatTableSyntaxRow.Add(floatTableSyntaxRow);
                }
                {
                    var floatTableSyntaxRow = new SpecificationSpecificFloat();
                    floatTableSyntaxRow.Float_of(2.2);
                    FloatTableSyntaxRow.Add(floatTableSyntaxRow);
                }
                objectWithPrimiiveLists.FloatTableSyntax_table_of(FloatTableSyntaxRow);
            }
            
            {
                var floatListSyntaxList = new List<SpecificationSpecificFloat>();
                {
                    var floatListSyntax = new SpecificationSpecificFloat();
                    floatListSyntax.Float_of(3.3);
                    floatListSyntaxList.Add(floatListSyntax);
                }
                {
                    var floatListSyntax = new SpecificationSpecificFloat();
                    floatListSyntax.Float_of(3.3);
                    floatListSyntaxList.Add(floatListSyntax);
                }
                objectWithPrimiiveLists.FloatListSyntax_list_of(floatListSyntaxList, "SpecificationSpecificFloat");
            }
            {
                var StringTableSyntaxRow = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificString>();
                {
                    var stringTableSyntaxRow = new SpecificationSpecificString();
                    stringTableSyntaxRow.String_of("s1");
                    StringTableSyntaxRow.Add(stringTableSyntaxRow);
                }
                {
                    var stringTableSyntaxRow = new SpecificationSpecificString();
                    stringTableSyntaxRow.String_of("s2");
                    StringTableSyntaxRow.Add(stringTableSyntaxRow);
                }
                objectWithPrimiiveLists.StringTableSyntax_table_of(StringTableSyntaxRow);
            }
            
            {
                var stringListSyntaxList = new List<SpecificationSpecificString>();
                {
                    var stringListSyntax = new SpecificationSpecificString();
                    stringListSyntax.String_of("s3");
                    stringListSyntaxList.Add(stringListSyntax);
                }
                {
                    var stringListSyntax = new SpecificationSpecificString();
                    stringListSyntax.String_of("s4");
                    stringListSyntaxList.Add(stringListSyntax);
                }
                objectWithPrimiiveLists.StringListSyntax_list_of(stringListSyntaxList, "SpecificationSpecificString");
            }
            {
                var DateTimeTableSyntaxRow = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificDateTime>();
                {
                    var dateTimeTableSyntaxRow = new SpecificationSpecificDateTime();
                    dateTimeTableSyntaxRow.DateTime_of(DateTime.Parse("2020-01-01T00:00:00"));
                    DateTimeTableSyntaxRow.Add(dateTimeTableSyntaxRow);
                }
                {
                    var dateTimeTableSyntaxRow = new SpecificationSpecificDateTime();
                    dateTimeTableSyntaxRow.DateTime_of(DateTime.Parse("2020-01-02T00:00:00"));
                    DateTimeTableSyntaxRow.Add(dateTimeTableSyntaxRow);
                }
                objectWithPrimiiveLists.DateTimeTableSyntax_table_of(DateTimeTableSyntaxRow);
            }
            
            {
                var dateTimeListSyntaxList = new List<SpecificationSpecificDateTime>();
                {
                    var dateTimeListSyntax = new SpecificationSpecificDateTime();
                    dateTimeListSyntax.DateTime_of(DateTime.Parse("2020-01-03T00:00:00"));
                    dateTimeListSyntaxList.Add(dateTimeListSyntax);
                }
                {
                    var dateTimeListSyntax = new SpecificationSpecificDateTime();
                    dateTimeListSyntax.DateTime_of(DateTime.Parse("2020-01-04T00:00:00"));
                    dateTimeListSyntaxList.Add(dateTimeListSyntax);
                }
                objectWithPrimiiveLists.DateTimeListSyntax_list_of(dateTimeListSyntaxList, "SpecificationSpecificDateTime");
            }
            
            return objectWithPrimiiveLists;
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
