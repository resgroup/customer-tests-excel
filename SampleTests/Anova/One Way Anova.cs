using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CustomerTestsExcel;
using System.Linq.Expressions;
using SampleTests;

using SampleSystemUnderTest;
using SampleTests.IgnoreOnGeneration.AnovaCalculator;
using SampleSystemUnderTest.AnovaCalculator;

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
                var group = new SpecificationSpecificGroup();
                group.Name_of("Langley School");
                {
                    var values_table = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificValue>();
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(90);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(87);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(93);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(115);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(97);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(85);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(102);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(110);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(111);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(102);
                        values_table.Add(value);
                    }
                    group.Values_table_of(values_table);
                }
                anovaCalculator.Groups_of(group);
            }

            {
                var group = new SpecificationSpecificGroup();
                group.Name_of("Ninestiles School");
                {
                    var values_table = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificValue>();
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(135);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(125);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(107);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(96);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(114);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(125);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(94);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(123);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(111);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(96);
                        values_table.Add(value);
                    }
                    group.Values_table_of(values_table);
                }
                anovaCalculator.Groups_of(group);
            }

            {
                var group = new SpecificationSpecificGroup();
                group.Name_of("Alderbrook School");
                {
                    var values_table = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificValue>();
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(93);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(101);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(74);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(87);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(76);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(87);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(98);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(108);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(113);
                        values_table.Add(value);
                    }
                    {
                        var value = new SpecificationSpecificValue();
                        value.Value_of(96);
                        values_table.Add(value);
                    }
                    group.Values_table_of(values_table);
                }
                anovaCalculator.Groups_of(group);
            }

            return anovaCalculator;
        }

        // act
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
                    returns => returns.Result,
                    new List<IAssertion<IAnovaResult>>
                    {
                         new EqualityAssertion<IAnovaResult>(returns => returns.SS_Between, 1956.2)
                        ,new ExcelFormulaDoesNotMatchCodeAssertion<IAnovaResult>("SS_Between", "1956.2", "1956.2")
                        ,new EqualityAssertion<IAnovaResult>(returns => returns.DF_Between, 2)
                        ,new ExcelFormulaDoesNotMatchCodeAssertion<IAnovaResult>("DF_Between", "2", "2")
                        ,new EqualityAssertion<IAnovaResult>(returns => returns.MS_Between, 978.1)
                        ,new ExcelFormulaDoesNotMatchCodeAssertion<IAnovaResult>("MS_Between", "978.1", "978.1")
                        ,new EqualityAssertion<IAnovaResult>(returns => returns.SS_Within, 4294.1)
                        ,new ExcelFormulaDoesNotMatchCodeAssertion<IAnovaResult>("SS_Within", "4294.1", "4294.1")
                        ,new EqualityAssertion<IAnovaResult>(returns => returns.DF_Within, 57.7159259259259)
                        ,new ExcelFormulaDoesNotMatchCodeAssertion<IAnovaResult>("DF_Within", "57.7159259259259", "57.7159259259259")
                        ,new EqualityAssertion<IAnovaResult>(returns => returns.MS_Within, 74.4006083435473)
                        ,new ExcelFormulaDoesNotMatchCodeAssertion<IAnovaResult>("MS_Within", "74.4006083435473", "74.4006083435473")
                        ,new EqualityAssertion<IAnovaResult>(returns => returns.F, 13.1463978827107)
                        ,new ExcelFormulaDoesNotMatchCodeAssertion<IAnovaResult>("F", "13.1463978827107", "13.1463978827107")
                        ,new EqualityAssertion<IAnovaResult>(returns => returns.StatisticalSignificance, 2.01893044311748E-05)
                        ,new ExcelFormulaDoesNotMatchCodeAssertion<IAnovaResult>("StatisticalSignificance", "2.01893044311748E-05", "2.01893044311748E-05")
                        ,new EqualityAssertion<IAnovaResult>(returns => returns.EffectSize, 0.312976977105099)
                        ,new ExcelFormulaDoesNotMatchCodeAssertion<IAnovaResult>("EffectSize", "0.312976977105099", "0.312976977105099")
                    }
                )
                ,new ExcelFormulaDoesNotMatchCodeAssertion<SpecificationSpecificAnovaCalculator>("Returns", "", "ANOVA TABLE")
            };
        }
        protected override string AssertionClassPrefixAddedByGenerator => "I";
    }
}
