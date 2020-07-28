using static System.Reflection.MethodBase;

namespace SampleTests.Setup
{
    public partial class SpecificationSpecificClassWithCustomProperty
    {
        // This shows that you can add custom properties to partially matched classes if it is useful
        internal SpecificationSpecificClassWithCustomProperty CustomInt_of(int value)
        {
            AddValueProperty(GetCurrentMethod(), value);

            // Do some custom thing with value here
            // classWithCustomProperty.Setup(m => m.Name).Returns(value);

            return this;
        }
    }
}