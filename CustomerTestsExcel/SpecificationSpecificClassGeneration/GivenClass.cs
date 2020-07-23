using CustomerTestsExcel.ExcelToCode;
using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class GivenClass
    {
        public bool IsRootClass { get; }
        public string Name { get; }
        public IReadOnlyList<IGivenClassProperty> Properties { get; }

        readonly IReadOnlyList<GivenClassSimpleProperty> givenSimpleProperties;
        readonly IReadOnlyList<GivenClassComplexProperty> givenComplexProperties;
        readonly IReadOnlyList<GivenClassFunction> givenFunctions;
        readonly IReadOnlyList<GivenClassComplexListProperty> givenComplexListProperties;

        public GivenClass(
            string name,
            IReadOnlyList<IGivenClassProperty> properties,
            IReadOnlyList<GivenClassSimpleProperty> givenSimpleProperties,
            IReadOnlyList<GivenClassComplexProperty> givenComplexProperties,
            IReadOnlyList<GivenClassFunction> givenFunctions,
            IReadOnlyList<GivenClassComplexListProperty> givenComplexListProperties,
            bool isRootClass = false)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            Properties = properties;
            this.givenSimpleProperties = givenSimpleProperties;
            this.givenComplexProperties = givenComplexProperties;
            this.givenFunctions = givenFunctions;
            this.givenComplexListProperties = givenComplexListProperties;
            IsRootClass = isRootClass;
        }

        public IEnumerable<GivenClassSimpleProperty> SimpleProperties =>
            givenSimpleProperties;

        public IEnumerable<GivenClassComplexProperty> ComplexProperties =>
            givenComplexProperties;

        public IEnumerable<GivenClassComplexListProperty> ComplexListProperties =>
            givenComplexListProperties;

        public IEnumerable<IGivenClassProperty> Functions =>
            givenFunctions;

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