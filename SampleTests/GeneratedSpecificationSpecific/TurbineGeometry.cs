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
using SampleTests.IgnoreOnGeneration.Calculator;
using SampleSystemUnderTest.Calculator;
using SampleTests.IgnoreOnGeneration.NameConversions;

namespace SampleTests.GeneratedSpecificationSpecific
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
            valueProperties.Add(GetCurrentMethod(), numberOfBlades);

            turbineGeometry.Setup(m => m.NumberOfBlades).Returns(numberOfBlades);

            return this;
        }

        internal SpecificationSpecificTurbineGeometry Diameter_of(Double diameter)
        {
            valueProperties.Add(GetCurrentMethod(), diameter);

            turbineGeometry.Setup(m => m.Diameter).Returns(diameter);

            return this;
        }





    }
}
