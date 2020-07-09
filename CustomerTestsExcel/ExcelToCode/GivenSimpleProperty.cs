using System.Collections.Generic;

namespace CustomerTestsExcel.ExcelToCode
{
    public class GivenSimpleProperty : IGivenSimpleProperty
    {
        public string PropertyOrFunctionName { get; }
        public string CsharpCodeRepresentation { get; }
        public ExcelPropertyType ExcelPropertyType { get; }
        public bool Nullable { get; }

        public GivenSimpleProperty(
            string propertyOrFunctionName,
            string cSharpCodeRepresentation,
            ExcelPropertyType excelPropertyType,
            bool nullable = false)
        {
            PropertyOrFunctionName = propertyOrFunctionName;
            CsharpCodeRepresentation = cSharpCodeRepresentation;
            ExcelPropertyType = excelPropertyType;
            Nullable = nullable;
        }

        public override bool Equals(object obj) =>
            obj is GivenSimpleProperty property
            && PropertyOrFunctionName == property.PropertyOrFunctionName
            && CsharpCodeRepresentation == property.CsharpCodeRepresentation
            && ExcelPropertyType == property.ExcelPropertyType;

        public override int GetHashCode()
        {
            var hashCode = -274620603;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyOrFunctionName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CsharpCodeRepresentation);
            hashCode = hashCode * -1521134295 + ExcelPropertyType.GetHashCode();
            return hashCode;
        }

        public override string ToString() =>
            $"{PropertyOrFunctionName}, {CsharpCodeRepresentation}, {ExcelPropertyType}";
    }
}
