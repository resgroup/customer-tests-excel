using System.Collections.Generic;

namespace CustomerTestsExcel.ExcelToCode
{
    public class VisitedGivenListProperty : IVisitedGivenListProperty
    {
        public string PropertyName { get; }
        public string ClassName { get; }

        public VisitedGivenListProperty(
            string propertyName,
            string className)
        {
            PropertyName = propertyName;
            ClassName = className;
        }

        public override string ToString() =>
            $"{PropertyName}, {ClassName}";

        public override bool Equals(object obj) =>
            obj is VisitedGivenListProperty property
            && PropertyName == property.PropertyName
            && ClassName == property.ClassName;

        public override int GetHashCode()
        {
            var hashCode = 1273138399;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ClassName);
            return hashCode;
        }
    }
}
