using System.Collections.Generic;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class GivenClass
    {
        public IReadOnlyList<GivenClassProperty> Properties { get; }

        public GivenClass(IReadOnlyList<GivenClassProperty> properties)
        {
            Properties = properties ?? throw new System.ArgumentNullException(nameof(properties));
        }
    }
}