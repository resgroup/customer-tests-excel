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
using SampleTests.Setup;

using SampleSystemUnderTest;
using SampleSystemUnderTest.AnovaCalculator;
using SampleSystemUnderTest.Routing;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleSystemUnderTest.Calculator;
using SampleSystemUnderTest.CustomProperties;
using SampleSystemUnderTest.Misc;

namespace SampleTests.Vermeulen_Near_Wake_Length
{
    [TestFixture]
    public class VermeulenNearWakeLength : SpecificationBase<SpecificationSpecificVermeulenNearWakeLengthCalculator>, ISpecification<SpecificationSpecificVermeulenNearWakeLengthCalculator>
    {
        public override string Description()
        {
            return "Vermeulen Near Wake Length";
        }
        
        public override SpecificationSpecificVermeulenNearWakeLengthCalculator Given()
        {
            return
                new SpecificationSpecificVermeulenNearWakeLengthCalculator()
                .VermeulenNearWakeLengthInputs_table_of(
                    new ReportSpecificationSetupClassUsingTable<SpecificationSpecificVermeulenNearWakeLengthInput>()
                    .Add(
                        new SpecificationSpecificVermeulenNearWakeLengthInput()
                        .Velocity_of(10)
                        .Ambient_Turbulence_of(0.15)
                        .RevolutionsPerMinute_of(15)
                        .Thrust_Coefficient_of(0.7)
                        .TurbineGeometry_of(
                            new SpecificationSpecificTurbineGeometry()
                            .NumberOfBlades_of(3)
                            .Diameter_of(76)
                        )
                    )
                    .Add(
                        new SpecificationSpecificVermeulenNearWakeLengthInput()
                        .Velocity_of(10)
                        .Ambient_Turbulence_of(0.15)
                        .RevolutionsPerMinute_of(15)
                        .Thrust_Coefficient_of(0.7)
                        .TurbineGeometry_of(
                            new SpecificationSpecificTurbineGeometry()
                            .NumberOfBlades_of(2)
                            .Diameter_of(76)
                        )
                    )
                    .Add(
                        new SpecificationSpecificVermeulenNearWakeLengthInput()
                        .Velocity_of(6)
                        .Ambient_Turbulence_of(0.15)
                        .RevolutionsPerMinute_of(15)
                        .Thrust_Coefficient_of(0.7)
                        .TurbineGeometry_of(
                            new SpecificationSpecificTurbineGeometry()
                            .NumberOfBlades_of(3)
                            .Diameter_of(76)
                        )
                    )
                    .Add(
                        new SpecificationSpecificVermeulenNearWakeLengthInput()
                        .Velocity_of(10)
                        .Ambient_Turbulence_of(0.1)
                        .RevolutionsPerMinute_of(15)
                        .Thrust_Coefficient_of(0.7)
                        .TurbineGeometry_of(
                            new SpecificationSpecificTurbineGeometry()
                            .NumberOfBlades_of(3)
                            .Diameter_of(76)
                        )
                    )
                    .Add(
                        new SpecificationSpecificVermeulenNearWakeLengthInput()
                        .Velocity_of(10)
                        .Ambient_Turbulence_of(0.15)
                        .RevolutionsPerMinute_of(15)
                        .Thrust_Coefficient_of(0.7)
                        .TurbineGeometry_of(
                            new SpecificationSpecificTurbineGeometry()
                            .NumberOfBlades_of(3)
                            .Diameter_of(60)
                        )
                    )
                    .Add(
                        new SpecificationSpecificVermeulenNearWakeLengthInput()
                        .Velocity_of(10)
                        .Ambient_Turbulence_of(0.15)
                        .RevolutionsPerMinute_of(20)
                        .Thrust_Coefficient_of(0.7)
                        .TurbineGeometry_of(
                            new SpecificationSpecificTurbineGeometry()
                            .NumberOfBlades_of(3)
                            .Diameter_of(76)
                        )
                    )
                    .Add(
                        new SpecificationSpecificVermeulenNearWakeLengthInput()
                        .Velocity_of(10)
                        .Ambient_Turbulence_of(0.15)
                        .RevolutionsPerMinute_of(15)
                        .Thrust_Coefficient_of(0.6)
                        .TurbineGeometry_of(
                            new SpecificationSpecificTurbineGeometry()
                            .NumberOfBlades_of(3)
                            .Diameter_of(76)
                        )
                    )
                )
            ;
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
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 140.01601451311993, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AngularVelocity_rps, 1.5707963267948966, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TipSpeedRatio, 5.969026041820607, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.FlowFieldRatio, 1.8257418583505536, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.ShearTurbulenceWakeErosionRate, -0.05451956143675844, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AmbientTurbulenceWakeErosionRate, 0.425, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.MechanicalWakeErosionRate, 0.21488493750554188, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TotalErosionRate, 0.47934634550188976, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.RadiusOfInviscidExpandedRotorDisk_m, 45.16841398288299, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.N, 1.4859092660202307, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 148.55405780294296, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AngularVelocity_rps, 1.5707963267948966, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TipSpeedRatio, 5.969026041820607, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.FlowFieldRatio, 1.8257418583505536, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.ShearTurbulenceWakeErosionRate, -0.05451956143675844, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AmbientTurbulenceWakeErosionRate, 0.425, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.MechanicalWakeErosionRate, 0.14325662500369457, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TotalErosionRate, 0.4517962407841677, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.RadiusOfInviscidExpandedRotorDisk_m, 45.16841398288299, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.N, 1.4859092660202307, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 120.18364521743895, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AngularVelocity_rps, 1.5707963267948966, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TipSpeedRatio, 9.948376736367678, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.FlowFieldRatio, 1.8257418583505536, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.ShearTurbulenceWakeErosionRate, -0.05451956143675844, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AmbientTurbulenceWakeErosionRate, 0.425, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.MechanicalWakeErosionRate, 0.35814156250923646, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TotalErosionRate, 0.5584467399634577, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.RadiusOfInviscidExpandedRotorDisk_m, 45.16841398288299, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.N, 1.4859092660202307, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 179.92386611626353, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AngularVelocity_rps, 1.5707963267948966, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TipSpeedRatio, 5.969026041820607, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.FlowFieldRatio, 1.8257418583505536, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.ShearTurbulenceWakeErosionRate, -0.05451956143675844, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AmbientTurbulenceWakeErosionRate, 0.3, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.MechanicalWakeErosionRate, 0.21488493750554188, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TotalErosionRate, 0.3730253596553686, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.RadiusOfInviscidExpandedRotorDisk_m, 45.16841398288299, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.N, 1.4859092660202307, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 114.97697996131835, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AngularVelocity_rps, 1.5707963267948966, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TipSpeedRatio, 4.71238898038469, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.FlowFieldRatio, 1.8257418583505536, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.ShearTurbulenceWakeErosionRate, -0.05451956143675844, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AmbientTurbulenceWakeErosionRate, 0.425, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.MechanicalWakeErosionRate, 0.16964600329384885, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TotalErosionRate, 0.46084395299584113, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.RadiusOfInviscidExpandedRotorDisk_m, 35.659274197012884, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.N, 1.4859092660202307, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 130.2092375310549, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AngularVelocity_rps, 2.0943951023931953, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TipSpeedRatio, 7.958701389094142, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.FlowFieldRatio, 1.8257418583505536, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.ShearTurbulenceWakeErosionRate, -0.05451956143675844, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AmbientTurbulenceWakeErosionRate, 0.425, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.MechanicalWakeErosionRate, 0.28651325000738914, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TotalErosionRate, 0.5154485667930925, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.RadiusOfInviscidExpandedRotorDisk_m, 45.16841398288299, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.N, 1.4859092660202307, 0.001)
                        }
                        ,new List<IAssertion<IVermeulenNearWakeLength>>
                        {
                                  new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.VermeulenNearWakeLength_m, 132.54688085802303, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AngularVelocity_rps, 1.5707963267948966, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TipSpeedRatio, 5.969026041820607, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.FlowFieldRatio, 1.5811388300841895, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.ShearTurbulenceWakeErosionRate, -0.04042671355980986, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.AmbientTurbulenceWakeErosionRate, 0.425, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.MechanicalWakeErosionRate, 0.21488493750554188, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.TotalErosionRate, 0.47794859089237574, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.RadiusOfInviscidExpandedRotorDisk_m, 43.1692278749665, 0.001)
                                , new EqualityAssertionWithPercentagePrecision<IVermeulenNearWakeLength>(vermeulenNearWakeLengths => vermeulenNearWakeLengths.N, 1.4674942789516103, 0.001)
                        }
                    }
                )
            };
        }
        
        protected override string AssertionClassPrefixAddedByGenerator => "I";
        
        protected override bool RoundTrippable() => false;
        
        protected override IEnumerable<string> IssuesPreventingRoundTrip() => new List<string>
        {
            "There is a complex property ('TurbineGeometry of', cell D7) within a table in the Excel test, worksheet 'VermeulenNearWakeLength'"
        };
    }
}
