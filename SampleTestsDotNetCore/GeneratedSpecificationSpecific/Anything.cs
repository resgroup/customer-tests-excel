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

namespace SampleTests.GeneratedSpecificationSpecific
{
    public partial class SpecificationSpecificAnything : ReportsSpecificationSetup
    {
        readonly Mock<IAnything> anything;

        public IAnything Anything =>
            anything.Object;

        public SpecificationSpecificAnything()
        {
            anything = new Mock<IAnything>();

        }

        internal SpecificationSpecificAnything AnInteger_of(Int32 anInteger)
        {
            AddValueProperty(GetCurrentMethod(), anInteger);

            anything.Setup(m => m.AnInteger).Returns(anInteger);

            return this;
        }

    }
}
