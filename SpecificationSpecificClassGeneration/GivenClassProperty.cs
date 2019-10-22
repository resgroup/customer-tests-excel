using CustomerTestsExcel.ExcelToCode;
using System.Collections.Generic;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class GivenClassProperty
    {
        public string Name { get; }
        public ExcelPropertyType Type { get; }

        public GivenClassProperty(string name, ExcelPropertyType type)
        {
            Name = name;
            Type = type;
        }

        public override bool Equals(object obj) =>
            obj is GivenClassProperty givenClassProperty
            && Name == givenClassProperty.Name
            && Type == givenClassProperty.Type;

        public override int GetHashCode()
        {
            var hashCode = -243844509;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            return hashCode;
        }
    }
}