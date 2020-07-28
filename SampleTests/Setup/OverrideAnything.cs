using System;
using static System.Reflection.MethodBase;
using Moq;
using CustomerTestsExcel;
using SampleSystemUnderTest.Misc;

// This is a functionally exact copy of the code in Anything.cs.txt
// It is here to demonstrate how to override generated files with
// custom ones (Create a "Override<old filename>" file).
namespace SampleTests.Setup
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
