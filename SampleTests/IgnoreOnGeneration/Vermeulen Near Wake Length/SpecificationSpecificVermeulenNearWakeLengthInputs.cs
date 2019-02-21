using System;
using CustomerTestsExcel;
using SampleTests.Vermeulen_Near_Wake_Length;

namespace SampleTests.IgnoreOnGeneration.Vermeulen_Near_Wake_Length
{
    internal class SpecificationSpecificVermeulenNearWakeLengthInputs : ReportsSpecificationSetup
    {
        internal double velocity_mps { get; private set; }
        internal double ambientTurbuluence { get; private set; }
        internal double revolutionsPerMinute { get; private set; }
        internal double thrustCoefficient { get; private set; }
        internal SpecificationSpecificTurbineGeometry turbineGeometry { get; private set; }

        internal SpecificationSpecificVermeulenNearWakeLengthInputs Velocity_of(int velocity_mps)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, velocity_mps);

            this.velocity_mps = velocity_mps;

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInputs Ambient_Turbulence_of(double ambientTurbuluence)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ambientTurbuluence);

            this.ambientTurbuluence = ambientTurbuluence;

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInputs RevolutionsPerMinute_of(double revolutionsPerMinute)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, revolutionsPerMinute);

            this.revolutionsPerMinute = revolutionsPerMinute;

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInputs Thrust_Coefficient_of(double thrustCoefficient)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, thrustCoefficient);

            this.thrustCoefficient = thrustCoefficient;

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInputs TurbineGeometry_of(SpecificationSpecificTurbineGeometry turbineGeometry)
        {
            _valueProperties.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, turbineGeometry);

            this.turbineGeometry = turbineGeometry;

            return this;
        }
    }
}