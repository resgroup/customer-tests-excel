using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class GivenClass
    {
        public string Name { get; }
        public IReadOnlyList<IGivenClassProperty> Properties { get; }

        public GivenClass(string name, IReadOnlyList<IGivenClassProperty> properties)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            Properties = properties ?? throw new System.ArgumentNullException(nameof(properties));
        }

        public override string ToString()
        {
            var properties = string.Join("\n", Properties.Select(p => p.ToString()));
            return $"Name {Name}\nProperties\n{properties}";
        }

        // see if can use normal Properties equalto, now that test is passing
        public override bool Equals(object obj) =>
            obj is GivenClass givenClass
            && Name == givenClass.Name
            && Properties.All(property => givenClass.Properties.Contains(property));

        public override int GetHashCode()
        {
            var hashCode = -1578535950;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<IReadOnlyList<IGivenClassProperty>>.Default.GetHashCode(Properties);
            return hashCode;
        }
    }
}