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
using SampleTests.IgnoreOnGeneration.Routing;
using SampleSystemUnderTest.Routing;
using SampleTests.IgnoreOnGeneration.Vermeulen_Near_Wake_Length; // need to add to generatetests.bat

namespace SampleTests.Vermeulen_Near_Wake_Length
{
    [TestFixture]
    public class VermeulenNearWakeLength : SpecificationBase<SpecificationSpecificVermeulenNearWakeLengthCalculator>, ISpecification<SpecificationSpecificVermeulenNearWakeLengthCalculator>
    {
        public override string Description()
        {
            return "Vermeulen Near Wake Length";
        }
        
        // arrange
        public override SpecificationSpecificVermeulenNearWakeLengthCalculator Given()
        {
            var vermeulenNearWakeLengthCalculator = new SpecificationSpecificVermeulenNearWakeLengthCalculator();
            {
                var vermeulenNearWakeLengthInputs_table = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificVermeulenNearWakeLengthInputs>();
                {
                    var vermeulenNearWakeLengthInputs = new SpecificationSpecificVermeulenNearWakeLengthInputs();
                    vermeulenNearWakeLengthInputs_table.Add(vermeulenNearWakeLengthInputs);
                }
                {
                    var vermeulenNearWakeLengthInputs = new SpecificationSpecificVermeulenNearWakeLengthInputs();
                    vermeulenNearWakeLengthInputs_table.Add(vermeulenNearWakeLengthInputs);
                }
                {
                    var vermeulenNearWakeLengthInputs = new SpecificationSpecificVermeulenNearWakeLengthInputs();
                    vermeulenNearWakeLengthInputs_table.Add(vermeulenNearWakeLengthInputs);
                }
                {
                    var vermeulenNearWakeLengthInputs = new SpecificationSpecificVermeulenNearWakeLengthInputs();
                    vermeulenNearWakeLengthInputs_table.Add(vermeulenNearWakeLengthInputs);
                }
                {
                    var vermeulenNearWakeLengthInputs = new SpecificationSpecificVermeulenNearWakeLengthInputs();
                    vermeulenNearWakeLengthInputs_table.Add(vermeulenNearWakeLengthInputs);
                }
                {
                    var vermeulenNearWakeLengthInputs = new SpecificationSpecificVermeulenNearWakeLengthInputs();
                    vermeulenNearWakeLengthInputs_table.Add(vermeulenNearWakeLengthInputs);
                }
                {
                    var vermeulenNearWakeLengthInputs = new SpecificationSpecificVermeulenNearWakeLengthInputs();
                    vermeulenNearWakeLengthInputs_table.Add(vermeulenNearWakeLengthInputs);
                }
                vermeulenNearWakeLengthCalculator.VermeulenNearWakeLengthInputs_table_of(vermeulenNearWakeLengthInputs_table);
            }
            
            return vermeulenNearWakeLengthCalculator;
        }
        
        // act
        public override string When(SpecificationSpecificVermeulenNearWakeLengthCalculator vermeulenNearWakeLengthCalculator)
        {
            vermeulenNearWakeLengthCalculator.Calculate();
            return "Calculate";
        }
        
        public override IEnumerable<IAssertion<SpecificationSpecificVermeulenNearWakeLengthCalculator>> Assertions()
        {
            return new List<IAssertion<SpecificationSpecificVermeulenNearWakeLengthCalculator>>
            {
                 new TableAssertion<SpecificationSpecificVermeulenNearWakeLengthCalculator, IVermeulenNearWakeLength>
                (
                    vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLengths,
                    new List<List<IAssertion<IVermeulenNearWakeLength>>>
                    {
                    }
                )
            };
        }
        
        protected override string AssertionClassPrefixAddedByGenerator => "I";
    }
}
