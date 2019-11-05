using CustomerTestsExcel.ExcelToCode;
using System.Collections.Generic;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class GivenClassSimpleProperty : IGivenClassProperty
    {
        public string Name { get; }
        public ExcelPropertyType Type { get; }

        public GivenClassSimpleProperty(string name, ExcelPropertyType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new System.ArgumentException("name is required", nameof(name));

            // this is a code smell, but not going to worry too much for now.
            if (type == ExcelPropertyType.Object)
                throw new System.ArgumentException("ExcelPropertyType cannot be ExcelPropertyYpe.Object for simple properties", nameof(name));

            Name = name;
            Type = type;
        }

        public override string ToString() =>
            $"Name {Name}, Type {Type}";

        public override bool Equals(object obj) =>
            obj is GivenClassSimpleProperty property
            && Name == property.Name
            && Type == property.Type;

        public override int GetHashCode()
        {
            var hashCode = -1557669481;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            return hashCode;
        }
    }
}
