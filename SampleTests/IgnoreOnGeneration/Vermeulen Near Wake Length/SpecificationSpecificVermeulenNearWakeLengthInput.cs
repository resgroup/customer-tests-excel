using static System.Reflection.MethodBase;
using System;
using CustomerTestsExcel;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleTests.GeneratedSpecificationSpecific;

namespace SampleTests.IgnoreOnGeneration.Vermeulen_Near_Wake_Length
{
    internal class SpecificationSpecificVermeulenNearWakeLengthInput : ReportsSpecificationSetup, IVermeulenNearWakeLengthInput
    {
        public double Velocity_mps { get; private set; }
        public double AmbientTurbuluence { get; private set; }
        public double RevolutionsPerMinute { get; private set; }
        public double ThrustCoefficient { get; private set; }
        public ITurbineGeometry TurbineGeometry => turbineGeometry.TurbineGeometry;
        SpecificationSpecificTurbineGeometry turbineGeometry;

        internal SpecificationSpecificVermeulenNearWakeLengthInput Velocity_of(int velocity_mps)
        {
            valueProperties.Add(GetCurrentMethod(), velocity_mps);

            this.Velocity_mps = velocity_mps;

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInput Ambient_Turbulence_of(double ambientTurbuluence)
        {
            valueProperties.Add(GetCurrentMethod(), ambientTurbuluence);

            this.AmbientTurbuluence = ambientTurbuluence;

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInput RevolutionsPerMinute_of(double revolutionsPerMinute)
        {
            valueProperties.Add(GetCurrentMethod(), revolutionsPerMinute);

            this.RevolutionsPerMinute = revolutionsPerMinute;

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInput Thrust_Coefficient_of(double thrustCoefficient)
        {
            valueProperties.Add(GetCurrentMethod(), thrustCoefficient);

            this.ThrustCoefficient = thrustCoefficient;

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInput TurbineGeometry_of(SpecificationSpecificTurbineGeometry turbineGeometry)
        {
            valueProperties.Add(GetCurrentMethod(), turbineGeometry);

            this.turbineGeometry = turbineGeometry;

            return this;
        }
    }
}