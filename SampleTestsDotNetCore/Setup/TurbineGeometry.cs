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
using SampleSystemUnderTest.Misc;

namespace SampleTests.Setup
{
    public partial class SpecificationSpecificTurbineGeometry : ReportsSpecificationSetup
    {
        readonly Mock<ITurbineGeometry> turbineGeometry;

        public ITurbineGeometry TurbineGeometry =>
            turbineGeometry.Object;

        public SpecificationSpecificTurbineGeometry()
        {
            turbineGeometry = new Mock<ITurbineGeometry>();

        }

        internal SpecificationSpecificTurbineGeometry NumberOfBlades_of(Int32 numberOfBlades)
        {
            AddValueProperty(GetCurrentMethod(), numberOfBlades);

            turbineGeometry.Setup(m => m.NumberOfBlades).Returns(numberOfBlades);

            return this;
        }

        internal SpecificationSpecificTurbineGeometry Diameter_of(Double diameter)
        {
            AddValueProperty(GetCurrentMethod(), diameter);

            turbineGeometry.Setup(m => m.Diameter).Returns(diameter);

            return this;
        }

    }
}
