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
using SampleTests.IgnoreOnGeneration.Vermeulen_Near_Wake_Length;
using SampleSystemUnderTest.VermeulenNearWakeLength;

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
                var vermeulenNearWakeLengthInputs_table = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificVermeulenNearWakeLengthInput>();
                {
                    var vermeulenNearWakeLengthInput = new SpecificationSpecificVermeulenNearWakeLengthInput();
                    vermeulenNearWakeLengthInput.Velocity_of(10);
                    vermeulenNearWakeLengthInput.Ambient_Turbulence_of(0.15);
                    vermeulenNearWakeLengthInput.RevolutionsPerMinute_of(15);
                    vermeulenNearWakeLengthInput.Thrust_Coefficient_of(0.7);
                    var vermeulenNearWakeLengthInput_TurbineGeometry = new SpecificationSpecificTurbineGeometry();
                    vermeulenNearWakeLengthInput_TurbineGeometry.NumberOfBlades_of(3);
                    vermeulenNearWakeLengthInput_TurbineGeometry.Diameter_of(76);
                    vermeulenNearWakeLengthInput.TurbineGeometry_of(vermeulenNearWakeLengthInput_TurbineGeometry);
                    vermeulenNearWakeLengthInputs_table.Add(vermeulenNearWakeLengthInput);
                }
                {
                    var vermeulenNearWakeLengthInput = new SpecificationSpecificVermeulenNearWakeLengthInput();
                    vermeulenNearWakeLengthInput.Velocity_of(10);
                    vermeulenNearWakeLengthInput.Ambient_Turbulence_of(0.15);
                    vermeulenNearWakeLengthInput.RevolutionsPerMinute_of(15);
                    vermeulenNearWakeLengthInput.Thrust_Coefficient_of(0.7);
                    var vermeulenNearWakeLengthInput_TurbineGeometry = new SpecificationSpecificTurbineGeometry();
                    vermeulenNearWakeLengthInput_TurbineGeometry.NumberOfBlades_of(2);
                    vermeulenNearWakeLengthInput_TurbineGeometry.Diameter_of(76);
                    vermeulenNearWakeLengthInput.TurbineGeometry_of(vermeulenNearWakeLengthInput_TurbineGeometry);
                    vermeulenNearWakeLengthInputs_table.Add(vermeulenNearWakeLengthInput);
                }
                {
                    var vermeulenNearWakeLengthInput = new SpecificationSpecificVermeulenNearWakeLengthInput();
                    vermeulenNearWakeLengthInput.Velocity_of(6);
                    vermeulenNearWakeLengthInput.Ambient_Turbulence_of(0.15);
                    vermeulenNearWakeLengthInput.RevolutionsPerMinute_of(15);
                    vermeulenNearWakeLengthInput.Thrust_Coefficient_of(0.7);
                    var vermeulenNearWakeLengthInput_TurbineGeometry = new SpecificationSpecificTurbineGeometry();
                    vermeulenNearWakeLengthInput_TurbineGeometry.NumberOfBlades_of(3);
                    vermeulenNearWakeLengthInput_TurbineGeometry.Diameter_of(76);
                    vermeulenNearWakeLengthInput.TurbineGeometry_of(vermeulenNearWakeLengthInput_TurbineGeometry);
                    vermeulenNearWakeLengthInputs_table.Add(vermeulenNearWakeLengthInput);
                }
                {
                    var vermeulenNearWakeLengthInput = new SpecificationSpecificVermeulenNearWakeLengthInput();
                    vermeulenNearWakeLengthInput.Velocity_of(10);
                    vermeulenNearWakeLengthInput.Ambient_Turbulence_of(0.1);
                    vermeulenNearWakeLengthInput.RevolutionsPerMinute_of(15);
                    vermeulenNearWakeLengthInput.Thrust_Coefficient_of(0.7);
                    var vermeulenNearWakeLengthInput_TurbineGeometry = new SpecificationSpecificTurbineGeometry();
                    vermeulenNearWakeLengthInput_TurbineGeometry.NumberOfBlades_of(3);
                    vermeulenNearWakeLengthInput_TurbineGeometry.Diameter_of(76);
                    vermeulenNearWakeLengthInput.TurbineGeometry_of(vermeulenNearWakeLengthInput_TurbineGeometry);
                    vermeulenNearWakeLengthInputs_table.Add(vermeulenNearWakeLengthInput);
                }
                {
                    var vermeulenNearWakeLengthInput = new SpecificationSpecificVermeulenNearWakeLengthInput();
                    vermeulenNearWakeLengthInput.Velocity_of(10);
                    vermeulenNearWakeLengthInput.Ambient_Turbulence_of(0.15);
                    vermeulenNearWakeLengthInput.RevolutionsPerMinute_of(15);
                    vermeulenNearWakeLengthInput.Thrust_Coefficient_of(0.7);
                    var vermeulenNearWakeLengthInput_TurbineGeometry = new SpecificationSpecificTurbineGeometry();
                    vermeulenNearWakeLengthInput_TurbineGeometry.NumberOfBlades_of(3);
                    vermeulenNearWakeLengthInput_TurbineGeometry.Diameter_of(60);
                    vermeulenNearWakeLengthInput.TurbineGeometry_of(vermeulenNearWakeLengthInput_TurbineGeometry);
                    vermeulenNearWakeLengthInputs_table.Add(vermeulenNearWakeLengthInput);
                }
                {
                    var vermeulenNearWakeLengthInput = new SpecificationSpecificVermeulenNearWakeLengthInput();
                    vermeulenNearWakeLengthInput.Velocity_of(10);
                    vermeulenNearWakeLengthInput.Ambient_Turbulence_of(0.15);
                    vermeulenNearWakeLengthInput.RevolutionsPerMinute_of(20);
                    vermeulenNearWakeLengthInput.Thrust_Coefficient_of(0.7);
                    var vermeulenNearWakeLengthInput_TurbineGeometry = new SpecificationSpecificTurbineGeometry();
                    vermeulenNearWakeLengthInput_TurbineGeometry.NumberOfBlades_of(3);
                    vermeulenNearWakeLengthInput_TurbineGeometry.Diameter_of(76);
                    vermeulenNearWakeLengthInput.TurbineGeometry_of(vermeulenNearWakeLengthInput_TurbineGeometry);
                    vermeulenNearWakeLengthInputs_table.Add(vermeulenNearWakeLengthInput);
                }
                {
                    var vermeulenNearWakeLengthInput = new SpecificationSpecificVermeulenNearWakeLengthInput();
                    vermeulenNearWakeLengthInput.Velocity_of(10);
                    vermeulenNearWakeLengthInput.Ambient_Turbulence_of(0.15);
                    vermeulenNearWakeLengthInput.RevolutionsPerMinute_of(15);
                    vermeulenNearWakeLengthInput.Thrust_Coefficient_of(0.6);
                    var vermeulenNearWakeLengthInput_TurbineGeometry = new SpecificationSpecificTurbineGeometry();
                    vermeulenNearWakeLengthInput_TurbineGeometry.NumberOfBlades_of(3);
                    vermeulenNearWakeLengthInput_TurbineGeometry.Diameter_of(76);
                    vermeulenNearWakeLengthInput.TurbineGeometry_of(vermeulenNearWakeLengthInput_TurbineGeometry);
                    vermeulenNearWakeLengthInputs_table.Add(vermeulenNearWakeLengthInput);
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
                         new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 140.01601451312, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 148.554057802943, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 120.183645217439, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 179.923866116264, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 114.976979961318, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 130.209237531055, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 132.546880858023, 0.001)
                        }
                    }
                )
            };
        }
        
        protected override string AssertionClassPrefixAddedByGenerator => "I";
    }
}
