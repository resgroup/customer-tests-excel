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
using SampleTests.IgnoreOnGeneration.Routing;
using SampleSystemUnderTest.Routing;
using SampleTests.IgnoreOnGeneration.Vermeulen_Near_Wake_Length;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleTests.IgnoreOnGeneration.Calculator;
using SampleSystemUnderTest.Calculator;
using SampleTests.IgnoreOnGeneration.NameConversions;

namespace SampleTests.Anova
{
    [TestFixture]
    public class One_Way_Anova : SpecificationBase<SpecificationSpecificAnovaCalculator>, ISpecification<SpecificationSpecificAnovaCalculator>
    {
        public override string Description()
        {
            return "Calculate One Way Anova";
        }
        
        // arrange
        public override SpecificationSpecificAnovaCalculator Given()
        {
            var anovaCalculator = new SpecificationSpecificAnovaCalculator();
            anovaCalculator.VariableDescription_of("IQ");
            
            {
                var groupsList = new List<SpecificationSpecificGroup>();
                {
                    var groups = new SpecificationSpecificGroup();
                    groups.Name_of("Langley School");
                    {
                        var FloatsRow = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificFloat>();
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(90);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(87);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(93);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(115);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(97);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(85);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(102);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(110);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(111);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(102);
                            FloatsRow.Add(floatsRow);
                        }
                        groups.Floats_table_of(FloatsRow);
                    }
                    groupsList.Add(groups);
                }
                {
                    var groups = new SpecificationSpecificGroup();
                    groups.Name_of("Ninestiles School");
                    {
                        var FloatsRow = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificFloat>();
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(135);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(125);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(107);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(96);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(114);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(125);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(94);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(123);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(111);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(96);
                            FloatsRow.Add(floatsRow);
                        }
                        groups.Floats_table_of(FloatsRow);
                    }
                    groupsList.Add(groups);
                }
                {
                    var groups = new SpecificationSpecificGroup();
                    groups.Name_of("Alderbrook School");
                    {
                        var FloatsRow = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificFloat>();
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(93);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(101);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(74);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(87);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(76);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(87);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(98);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(108);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(113);
                            FloatsRow.Add(floatsRow);
                        }
                        {
                            var floatsRow = new SpecificationSpecificFloat();
                            floatsRow.Float_of(96);
                            FloatsRow.Add(floatsRow);
                        }
                        groups.Floats_table_of(FloatsRow);
                    }
                    groupsList.Add(groups);
                }
                anovaCalculator.Groups_list_of(groupsList, "SpecificationSpecificGroup");
            }
            
            return anovaCalculator;
        }
        
        public override string When(SpecificationSpecificAnovaCalculator anovaCalculator)
        {
            anovaCalculator.Calculate();
            return "Calculate";
        }
        
        public override IEnumerable<IAssertion<SpecificationSpecificAnovaCalculator>> Assertions()
        {
            return new List<IAssertion<SpecificationSpecificAnovaCalculator>>
            {
                 new ParentAssertion<SpecificationSpecificAnovaCalculator, IAnovaResult>
                (
                    anovaResult => anovaResult.AnovaResult,
                    new List<IAssertion<IAnovaResult>>
                    {
                          new EqualityAssertionWithPercentagePrecision<IAnovaResult>(anovaResult => anovaResult.SS_Between, 1956.2, 0.001)
                        , new EqualityAssertionWithPercentagePrecision<IAnovaResult>(anovaResult => anovaResult.DF_Between, 2, 0.001)
                        , new EqualityAssertionWithPercentagePrecision<IAnovaResult>(anovaResult => anovaResult.MS_Between, 978.1, 0.001)
                        , new EqualityAssertionWithPercentagePrecision<IAnovaResult>(anovaResult => anovaResult.SS_Within, 4294.1, 0.001)
                        , new EqualityAssertionWithPercentagePrecision<IAnovaResult>(anovaResult => anovaResult.DF_Within, 27, 0.001)
                        , new EqualityAssertionWithPercentagePrecision<IAnovaResult>(anovaResult => anovaResult.MS_Within, 159.040740740741, 0.001)
                        , new EqualityAssertionWithPercentagePrecision<IAnovaResult>(anovaResult => anovaResult.F, 6.14999650683496, 0.001)
                        , new EqualityAssertionWithPercentagePrecision<IAnovaResult>(anovaResult => anovaResult.StatisticalSignificance, 0.00629669167874152, 0.001)
                        , new EqualityAssertionWithPercentagePrecision<IAnovaResult>(anovaResult => anovaResult.EffectSize, 0.312976977105099, 0.001)
                    }
                )
            };
        }
        
        protected override string AssertionClassPrefixAddedByGenerator => "I";
    }
}
