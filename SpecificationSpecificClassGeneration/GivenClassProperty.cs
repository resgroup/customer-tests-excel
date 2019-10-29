using CustomerTestsExcel.ExcelToCode;
using System.Collections.Generic;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class GivenClassProperty
    {
        public string Name { get; }
        public ExcelPropertyType Type { get; }
        public string ClassName { get; }

        // the className property is a bit of a code smell, not sure what to do about it, and its not a big deal
        // maybe have two separate classes that implement an interface / abstract class?
        // maybe ExcelPropertyType becomes a class hierarchy, and the Object type also has a className property?
        // Not sure its a big enough problem to worry about
        public GivenClassProperty(string name, ExcelPropertyType type, string className = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new System.ArgumentException("name is required", nameof(name));

            if (type == ExcelPropertyType.Object && string.IsNullOrWhiteSpace(className))
                throw new System.ArgumentException("className is required when type is ExcelPropertyYpe.Object", nameof(name));

            Name = name;
            Type = type;
            ClassName = className;
        }

        public override string ToString() =>
            $"Name {Name}, Type {Type}, Class Name if an object {ClassName}";

        public override bool Equals(object obj) =>
            obj is GivenClassProperty property
            && Name == property.Name
            && Type == property.Type
            && ClassName == property.ClassName;

        public override int GetHashCode()
        {
            var hashCode = -1557669481;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ClassName);
            return hashCode;
        }

    }
}