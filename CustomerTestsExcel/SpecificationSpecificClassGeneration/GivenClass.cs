using CustomerTestsExcel.ExcelToCode;
using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class GivenClass
    {
        private readonly IReadOnlyList<IVisitedGivenSimpleProperty> givenSimpleProperties;
        private readonly IReadOnlyList<IVisitedGivenComplexProperty> givenComplexProperties;
        private readonly IReadOnlyList<IVisitedGivenFunction> givenFunctions;
        private readonly IReadOnlyList<IVisitedGivenListProperty> givenListProperties;
        private readonly IReadOnlyList<IVisitedGivenTableProperty> givenTableProperties;

        public bool IsRootClass { get; }
        public string Name { get; }
        public IReadOnlyList<IGivenClassProperty> Properties { get; }

        public GivenClass(
            string name,
            IReadOnlyList<IGivenClassProperty> properties,
            IReadOnlyList<IVisitedGivenSimpleProperty> givenSimpleProperties,
            IReadOnlyList<IVisitedGivenComplexProperty> givenComplexProperties,
            IReadOnlyList<IVisitedGivenFunction> givenFunctions,
            IReadOnlyList<IVisitedGivenListProperty> givenListProperties,
            IReadOnlyList<IVisitedGivenTableProperty> givenTableProperties,
            bool isRootClass = false)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            Properties = properties;
            this.givenSimpleProperties = givenSimpleProperties;
            this.givenComplexProperties = givenComplexProperties;
            this.givenFunctions = givenFunctions;
            this.givenListProperties = givenListProperties;
            this.givenTableProperties = givenTableProperties;
            IsRootClass = isRootClass;
        }

        public IEnumerable<IGivenClassProperty> SimpleProperties =>
            Properties
            .Where(excelProperty => excelProperty.Type.IsSimpleProperty());

        public IEnumerable<IGivenClassProperty> ComplexProperties =>
            Properties
            .Where(excelProperty => excelProperty.Type == ExcelPropertyType.Object);

        public IEnumerable<IGivenClassProperty> ListProperties =>
            Properties
            .Where(excelProperty => excelProperty.Type == ExcelPropertyType.List);

        public IEnumerable<IGivenClassProperty> Functions =>
            Properties
            .Where(excelProperty => excelProperty.Type == ExcelPropertyType.Function);

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