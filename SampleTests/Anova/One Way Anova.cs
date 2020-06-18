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

namespace SampleTests.Anova
{
    [TestFixture]
    public class One_Way_Anova : SpecificationBase<SpecificationSpecificAnovaCalculator>, ISpecification<SpecificationSpecificAnovaCalculator>
    {
        public override string Description()
        {
            return "Calculate One Way Anova";
        }
        
        public override SpecificationSpecificAnovaCalculator Given()
        {
            return
                new SpecificationSpecificAnovaCalculator()
                .VariableDescription_of("IQ")
                
                .Groups_list_of(
                    "SpecificationSpecificGroup", 
                    new FluentList<SpecificationSpecificGroup>()
                    .FluentAdd(
                        new SpecificationSpecificGroup()
                        .Name_of("Langley School")
                        .Floats_table_of(
                            new ReportSpecificationSetupClassUsingTable<SpecificationSpecificFloat>()
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(90)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(87)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(93)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(115)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(97)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(85)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(102)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(110)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(111)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(102)
                            )
                        )
                    )
                    .FluentAdd(
                        new SpecificationSpecificGroup()
                        .Name_of("Ninestiles School")
                        .Floats_table_of(
                            new ReportSpecificationSetupClassUsingTable<SpecificationSpecificFloat>()
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(135)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(125)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(107)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(96)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(114)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(125)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(94)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(123)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(111)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(96)
                            )
                        )
                    )
                    .FluentAdd(
                        new SpecificationSpecificGroup()
                        .Name_of("Alderbrook School")
                        .Floats_table_of(
                            new ReportSpecificationSetupClassUsingTable<SpecificationSpecificFloat>()
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(93)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(101)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(74)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(87)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(76)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(87)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(98)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(108)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(113)
                            )
                            .Add(
                                new SpecificationSpecificFloat()
                                .Float_of(96)
                            )
                        )
                    )
                )
            ;
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
