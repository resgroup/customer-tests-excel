using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class GivenClassMutable
    {
        public bool IsRootClass { get; }
        public string Name { get; set; }

        readonly List<IGivenClassProperty> properties;
        public IReadOnlyList<IGivenClassProperty> Properties => properties;

        public GivenClassMutable(string name, bool isRootClass = false)
        {
            Name = name;
            IsRootClass = isRootClass;
            properties = new List<IGivenClassProperty>();
        }

        public void AddProperty(IGivenClassProperty property)
        {
            // could think about raising an exception if the property name already
            // exists but with a different type
            if (properties.Any(p => p.Name == property.Name) == false)
                properties.Add(property);
        }
    }
}
