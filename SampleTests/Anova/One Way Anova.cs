using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CustomerTestsExcel;
using CustomerTestsExcel.Assertions;
using System.Linq.Expressions;
using SampleTests;

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
                        var ValuesRow = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificValue>();
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(90);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(87);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(93);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(115);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(97);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(85);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(102);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(110);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(111);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(102);
                            ValuesRow.Add(valuesRow);
                        }
                        groups.Values_table_of(ValuesRow);
                    }
                    groupsList.Add(groups);
                }
                {
                    var groups = new SpecificationSpecificGroup();
                    groups.Name_of("Ninestiles School");
                    {
                        var ValuesRow = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificValue>();
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(135);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(125);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(107);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(96);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(114);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(125);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(94);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(123);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(111);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(96);
                            ValuesRow.Add(valuesRow);
                        }
                        groups.Values_table_of(ValuesRow);
                    }
                    groupsList.Add(groups);
                }
                {
                    var groups = new SpecificationSpecificGroup();
                    groups.Name_of("Alderbrook School");
                    {
                        var ValuesRow = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificValue>();
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(93);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(101);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(74);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(87);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(76);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(87);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(98);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(108);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(113);
                            ValuesRow.Add(valuesRow);
                        }
                        {
                            var valuesRow = new SpecificationSpecificValue();
                            valuesRow.Value_of(96);
                            ValuesRow.Add(valuesRow);
                        }
                        groups.Values_table_of(ValuesRow);
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
