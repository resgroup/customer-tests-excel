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
    public partial class SpecificationSpecificClassWithCustomProperty : ReportsSpecificationSetup
    {
        readonly Mock<IClassWithCustomProperty> classWithCustomProperty;

        public IClassWithCustomProperty ClassWithCustomProperty =>
            classWithCustomProperty.Object;

        public SpecificationSpecificClassWithCustomProperty()
        {
            classWithCustomProperty = new Mock<IClassWithCustomProperty>();

        }

    }
}
