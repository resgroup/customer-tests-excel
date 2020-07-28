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
    public partial class SpecificationSpecificHasObjectProperty : ReportsSpecificationSetup
    {
        readonly Mock<IHasObjectProperty> hasObjectProperty;

        public IHasObjectProperty HasObjectProperty =>
            hasObjectProperty.Object;

        public SpecificationSpecificHasObjectProperty()
        {
            hasObjectProperty = new Mock<IHasObjectProperty>();

        }

        internal SpecificationSpecificHasObjectProperty Anything_of(IAnything anything)
        {
            AddValueProperty(GetCurrentMethod(), anything);

            hasObjectProperty.Setup(m => m.Anything).Returns(anything);

            return this;
        }

    }
}
