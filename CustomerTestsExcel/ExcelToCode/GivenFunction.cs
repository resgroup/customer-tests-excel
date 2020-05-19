using System.Collections.Generic;

namespace CustomerTestsExcel.ExcelToCode
{
    public class GivenFunction : IGivenFunction
    {
        public string PropertyOrFunctionName { get; }
        public ExcelPropertyType ExcelPropertyType =>
            ExcelPropertyType.Function;
        public string CsharpCodeRepresentation =>
            "";

        public GivenFunction(string propertyOrFunctionName)
        {
            PropertyOrFunctionName = propertyOrFunctionName;
        }

        public override bool Equals(object obj) =>
            obj is GivenFunction property
            && PropertyOrFunctionName == property.PropertyOrFunctionName;

        public override int GetHashCode()
        {
            var hashCode = -274620603;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyOrFunctionName);
            return hashCode;
        }

        public override string ToString() =>
            $"{PropertyOrFunctionName}";
    }
}
