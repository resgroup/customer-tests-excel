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
                var VermeulenNearWakeLengthInputsRow = new ReportSpecificationSetupClassUsingTable<SpecificationSpecificVermeulenNearWakeLengthInput>();
                {
                    var vermeulenNearWakeLengthInputsRow = new SpecificationSpecificVermeulenNearWakeLengthInput();
                    vermeulenNearWakeLengthInputsRow.Velocity_of(10);
                    vermeulenNearWakeLengthInputsRow.Ambient_Turbulence_of(0.15);
                    vermeulenNearWakeLengthInputsRow.RevolutionsPerMinute_of(15);
                    vermeulenNearWakeLengthInputsRow.Thrust_Coefficient_of(0.7);
                    var vermeulenNearWakeLengthInputsRow_TurbineGeometry = new SpecificationSpecificTurbineGeometry();
                    vermeulenNearWakeLengthInputsRow_TurbineGeometry.NumberOfBlades_of(3);
                    vermeulenNearWakeLengthInputsRow_TurbineGeometry.Diameter_of(76);
                    vermeulenNearWakeLengthInputsRow.TurbineGeometry_of(vermeulenNearWakeLengthInputsRow_TurbineGeometry);
                    VermeulenNearWakeLengthInputsRow.Add(vermeulenNearWakeLengthInputsRow);
                }
                {
                    var vermeulenNearWakeLengthInputsRow = new SpecificationSpecificVermeulenNearWakeLengthInput();
                    vermeulenNearWakeLengthInputsRow.Velocity_of(10);
                    vermeulenNearWakeLengthInputsRow.Ambient_Turbulence_of(0.15);
                    vermeulenNearWakeLengthInputsRow.RevolutionsPerMinute_of(15);
                    vermeulenNearWakeLengthInputsRow.Thrust_Coefficient_of(0.7);
                    var vermeulenNearWakeLengthInputsRow_TurbineGeometry = new SpecificationSpecificTurbineGeometry();
                    vermeulenNearWakeLengthInputsRow_TurbineGeometry.NumberOfBlades_of(2);
                    vermeulenNearWakeLengthInputsRow_TurbineGeometry.Diameter_of(76);
                    vermeulenNearWakeLengthInputsRow.TurbineGeometry_of(vermeulenNearWakeLengthInputsRow_TurbineGeometry);
                    VermeulenNearWakeLengthInputsRow.Add(vermeulenNearWakeLengthInputsRow);
                }
                {
                    var vermeulenNearWakeLengthInputsRow = new SpecificationSpecificVermeulenNearWakeLengthInput();
                    vermeulenNearWakeLengthInputsRow.Velocity_of(6);
                    vermeulenNearWakeLengthInputsRow.Ambient_Turbulence_of(0.15);
                    vermeulenNearWakeLengthInputsRow.RevolutionsPerMinute_of(15);
                    vermeulenNearWakeLengthInputsRow.Thrust_Coefficient_of(0.7);
                    var vermeulenNearWakeLengthInputsRow_TurbineGeometry = new SpecificationSpecificTurbineGeometry();
                    vermeulenNearWakeLengthInputsRow_TurbineGeometry.NumberOfBlades_of(3);
                    vermeulenNearWakeLengthInputsRow_TurbineGeometry.Diameter_of(76);
                    vermeulenNearWakeLengthInputsRow.TurbineGeometry_of(vermeulenNearWakeLengthInputsRow_TurbineGeometry);
                    VermeulenNearWakeLengthInputsRow.Add(vermeulenNearWakeLengthInputsRow);
                }
                {
                    var vermeulenNearWakeLengthInputsRow = new SpecificationSpecificVermeulenNearWakeLengthInput();
                    vermeulenNearWakeLengthInputsRow.Velocity_of(10);
                    vermeulenNearWakeLengthInputsRow.Ambient_Turbulence_of(0.1);
                    vermeulenNearWakeLengthInputsRow.RevolutionsPerMinute_of(15);
                    vermeulenNearWakeLengthInputsRow.Thrust_Coefficient_of(0.7);
                    var vermeulenNearWakeLengthInputsRow_TurbineGeometry = new SpecificationSpecificTurbineGeometry();
                    vermeulenNearWakeLengthInputsRow_TurbineGeometry.NumberOfBlades_of(3);
                    vermeulenNearWakeLengthInputsRow_TurbineGeometry.Diameter_of(76);
                    vermeulenNearWakeLengthInputsRow.TurbineGeometry_of(vermeulenNearWakeLengthInputsRow_TurbineGeometry);
                    VermeulenNearWakeLengthInputsRow.Add(vermeulenNearWakeLengthInputsRow);
                }
                {
                    var vermeulenNearWakeLengthInputsRow = new SpecificationSpecificVermeulenNearWakeLengthInput();
                    vermeulenNearWakeLengthInputsRow.Velocity_of(10);
                    vermeulenNearWakeLengthInputsRow.Ambient_Turbulence_of(0.15);
                    vermeulenNearWakeLengthInputsRow.RevolutionsPerMinute_of(15);
                    vermeulenNearWakeLengthInputsRow.Thrust_Coefficient_of(0.7);
                    var vermeulenNearWakeLengthInputsRow_TurbineGeometry = new SpecificationSpecificTurbineGeometry();
                    vermeulenNearWakeLengthInputsRow_TurbineGeometry.NumberOfBlades_of(3);
                    vermeulenNearWakeLengthInputsRow_TurbineGeometry.Diameter_of(60);
                    vermeulenNearWakeLengthInputsRow.TurbineGeometry_of(vermeulenNearWakeLengthInputsRow_TurbineGeometry);
                    VermeulenNearWakeLengthInputsRow.Add(vermeulenNearWakeLengthInputsRow);
                }
                {
                    var vermeulenNearWakeLengthInputsRow = new SpecificationSpecificVermeulenNearWakeLengthInput();
                    vermeulenNearWakeLengthInputsRow.Velocity_of(10);
                    vermeulenNearWakeLengthInputsRow.Ambient_Turbulence_of(0.15);
                    vermeulenNearWakeLengthInputsRow.RevolutionsPerMinute_of(20);
                    vermeulenNearWakeLengthInputsRow.Thrust_Coefficient_of(0.7);
                    var vermeulenNearWakeLengthInputsRow_TurbineGeometry = new SpecificationSpecificTurbineGeometry();
                    vermeulenNearWakeLengthInputsRow_TurbineGeometry.NumberOfBlades_of(3);
                    vermeulenNearWakeLengthInputsRow_TurbineGeometry.Diameter_of(76);
                    vermeulenNearWakeLengthInputsRow.TurbineGeometry_of(vermeulenNearWakeLengthInputsRow_TurbineGeometry);
                    VermeulenNearWakeLengthInputsRow.Add(vermeulenNearWakeLengthInputsRow);
                }
                {
                    var vermeulenNearWakeLengthInputsRow = new SpecificationSpecificVermeulenNearWakeLengthInput();
                    vermeulenNearWakeLengthInputsRow.Velocity_of(10);
                    vermeulenNearWakeLengthInputsRow.Ambient_Turbulence_of(0.15);
                    vermeulenNearWakeLengthInputsRow.RevolutionsPerMinute_of(15);
                    vermeulenNearWakeLengthInputsRow.Thrust_Coefficient_of(0.6);
                    var vermeulenNearWakeLengthInputsRow_TurbineGeometry = new SpecificationSpecificTurbineGeometry();
                    vermeulenNearWakeLengthInputsRow_TurbineGeometry.NumberOfBlades_of(3);
                    vermeulenNearWakeLengthInputsRow_TurbineGeometry.Diameter_of(76);
                    vermeulenNearWakeLengthInputsRow.TurbineGeometry_of(vermeulenNearWakeLengthInputsRow_TurbineGeometry);
                    VermeulenNearWakeLengthInputsRow.Add(vermeulenNearWakeLengthInputsRow);
                }
                vermeulenNearWakeLengthCalculator.VermeulenNearWakeLengthInputs_table_of(VermeulenNearWakeLengthInputsRow);
            }
            
            return vermeulenNearWakeLengthCalculator;
        }
        
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
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AngularVelocity_rps, 1.5707963267949, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TipSpeedRatio, 5.96902604182061, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.FlowFieldRatio, 1.82574185835055, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.ShearTurbulenceWakeErosionRate, -0.0545195614367584, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AmbientTurbulenceWakeErosionRate, 0.425, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.MechanicalWakeErosionRate, 0.214884937505542, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TotalErosionRate, 0.47934634550189, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.RadiusOfInviscidExpandedRotorDisk_m, 45.168413982883, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.N, 1.48590926602023, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 148.554057802943, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AngularVelocity_rps, 1.5707963267949, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TipSpeedRatio, 5.96902604182061, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.FlowFieldRatio, 1.82574185835055, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.ShearTurbulenceWakeErosionRate, -0.0545195614367584, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AmbientTurbulenceWakeErosionRate, 0.425, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.MechanicalWakeErosionRate, 0.143256625003695, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TotalErosionRate, 0.451796240784168, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.RadiusOfInviscidExpandedRotorDisk_m, 45.168413982883, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.N, 1.48590926602023, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 120.183645217439, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AngularVelocity_rps, 1.5707963267949, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TipSpeedRatio, 9.94837673636768, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.FlowFieldRatio, 1.82574185835055, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.ShearTurbulenceWakeErosionRate, -0.0545195614367584, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AmbientTurbulenceWakeErosionRate, 0.425, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.MechanicalWakeErosionRate, 0.358141562509236, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TotalErosionRate, 0.558446739963458, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.RadiusOfInviscidExpandedRotorDisk_m, 45.168413982883, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.N, 1.48590926602023, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 179.923866116264, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AngularVelocity_rps, 1.5707963267949, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TipSpeedRatio, 5.96902604182061, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.FlowFieldRatio, 1.82574185835055, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.ShearTurbulenceWakeErosionRate, -0.0545195614367584, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AmbientTurbulenceWakeErosionRate, 0.3, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.MechanicalWakeErosionRate, 0.214884937505542, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TotalErosionRate, 0.373025359655369, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.RadiusOfInviscidExpandedRotorDisk_m, 45.168413982883, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.N, 1.48590926602023, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 114.976979961318, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AngularVelocity_rps, 1.5707963267949, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TipSpeedRatio, 4.71238898038469, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.FlowFieldRatio, 1.82574185835055, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.ShearTurbulenceWakeErosionRate, -0.0545195614367584, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AmbientTurbulenceWakeErosionRate, 0.425, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.MechanicalWakeErosionRate, 0.169646003293849, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TotalErosionRate, 0.460843952995841, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.RadiusOfInviscidExpandedRotorDisk_m, 35.6592741970129, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.N, 1.48590926602023, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 130.209237531055, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AngularVelocity_rps, 2.0943951023932, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TipSpeedRatio, 7.95870138909414, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.FlowFieldRatio, 1.82574185835055, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.ShearTurbulenceWakeErosionRate, -0.0545195614367584, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AmbientTurbulenceWakeErosionRate, 0.425, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.MechanicalWakeErosionRate, 0.286513250007389, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TotalErosionRate, 0.515448566793092, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.RadiusOfInviscidExpandedRotorDisk_m, 45.168413982883, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.N, 1.48590926602023, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 132.546880858023, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AngularVelocity_rps, 1.5707963267949, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TipSpeedRatio, 5.96902604182061, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.FlowFieldRatio, 1.58113883008419, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.ShearTurbulenceWakeErosionRate, -0.0404267135598099, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AmbientTurbulenceWakeErosionRate, 0.425, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.MechanicalWakeErosionRate, 0.214884937505542, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TotalErosionRate, 0.477948590892376, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.RadiusOfInviscidExpandedRotorDisk_m, 43.1692278749665, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.N, 1.46749427895161, 0.001)
                        }
                    }
                )
            };
        }
        
        protected override string AssertionClassPrefixAddedByGenerator => "I";
        
        protected override bool RoundTrippable() => false;
        
        protected override IEnumerable<string> IssuesPreventingRoundTrip() => new List<string> {
            "There is a complex property ('TurbineGeometry of', cell D7) within a table in the Excel test, worksheet 'VermeulenNearWakeLength'"
        };
    }
}
