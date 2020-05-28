using System;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.MethodBase;
using Moq;
using CustomerTestsExcel;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using SampleSystemUnderTest;
using SampleSystemUnderTest.AnovaCalculator;
using SampleSystemUnderTest.Routing;
using SampleSystemUnderTest.VermeulenNearWakeLength;
using SampleSystemUnderTest.Calculator;
using SampleSystemUnderTest.CustomProperties;

namespace SampleTests.GeneratedSpecificationSpecific
{
    public partial class SpecificationSpecificVermeulenNearWakeLengthInput : ReportsSpecificationSetup
    {
        readonly Mock<IVermeulenNearWakeLengthInput> vermeulenNearWakeLengthInput;

        public IVermeulenNearWakeLengthInput VermeulenNearWakeLengthInput =>
            vermeulenNearWakeLengthInput.Object;



        public SpecificationSpecificVermeulenNearWakeLengthInput()
        {
            vermeulenNearWakeLengthInput = new Mock<IVermeulenNearWakeLengthInput>();


        }

        internal SpecificationSpecificVermeulenNearWakeLengthInput Velocity_of(Double velocity)
        {
            AddValueProperty(GetCurrentMethod(), velocity);

            vermeulenNearWakeLengthInput.Setup(m => m.Velocity).Returns(velocity);

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInput Ambient_Turbulence_of(Double ambient_Turbulence)
        {
            AddValueProperty(GetCurrentMethod(), ambient_Turbulence);

            vermeulenNearWakeLengthInput.Setup(m => m.Ambient_Turbulence).Returns(ambient_Turbulence);

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInput RevolutionsPerMinute_of(Double revolutionsPerMinute)
        {
            AddValueProperty(GetCurrentMethod(), revolutionsPerMinute);

            vermeulenNearWakeLengthInput.Setup(m => m.RevolutionsPerMinute).Returns(revolutionsPerMinute);

            return this;
        }

        internal SpecificationSpecificVermeulenNearWakeLengthInput Thrust_Coefficient_of(Double thrust_Coefficient)
        {
            AddValueProperty(GetCurrentMethod(), thrust_Coefficient);

            vermeulenNearWakeLengthInput.Setup(m => m.Thrust_Coefficient).Returns(thrust_Coefficient);

            return this;
        }


        internal SpecificationSpecificVermeulenNearWakeLengthInput TurbineGeometry_of(SpecificationSpecificTurbineGeometry turbineGeometry)
        {
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), turbineGeometry));

            vermeulenNearWakeLengthInput.Setup(m => m.TurbineGeometry).Returns(turbineGeometry.TurbineGeometry);

            return this;
        }



    }
}
