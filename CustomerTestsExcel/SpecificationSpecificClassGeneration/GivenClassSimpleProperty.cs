using CustomerTestsExcel.ExcelToCode;
using System;
using System.Collections.Generic;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class GivenClassSimpleProperty : IGivenClassProperty
    {
        public string Name { get; }
        public ExcelPropertyType Type { get; }
        public string ExampleValue { get; }
        public bool Nullable { get; }

        public GivenClassSimpleProperty(
            string name,
            ExcelPropertyType type,
            string exampleValue = "",
            bool nullable = true)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new System.ArgumentException("name is required", nameof(name));

            // this is a code smell, but not going to worry too much for now. A Sum type is really 
            // what I want here but they don't easily exist in C#. There almost certainly is a better
            // solution though
            if (type == ExcelPropertyType.Object)
                throw new System.ArgumentException("ExcelPropertyType cannot be ExcelPropertyType.Object for simple properties", nameof(name));

            Name = name;
            Type = type;
            ExampleValue = exampleValue;
            Nullable = nullable;
        }

        public bool TypesMatch(Type cSharpPropertyType)
        {
            if (Type == ExcelPropertyType.Null && IsNullableType(cSharpPropertyType))
                return true;

            if (Type == ExcelPropertyType.Enum && cSharpPropertyType.IsEnum)
                return true;

            if (Type == ExcelPropertyType.Number && IsNumberType(cSharpPropertyType))
                return true;

            if (Type == ExcelPropertyType.Decimal && cSharpPropertyType == typeof(decimal))
                return true;

            if (Type == ExcelPropertyType.DateTime && cSharpPropertyType == typeof(DateTime))
                return true;

            if (Type == ExcelPropertyType.TimeSpan && cSharpPropertyType == typeof(TimeSpan))
                return true;

            if (Type == ExcelPropertyType.String && cSharpPropertyType == typeof(string))
                return true;

            if (Type == ExcelPropertyType.Boolean && cSharpPropertyType == typeof(bool))
                return true;

            return false;
        }

        bool IsNumberType(Type csharpPropertytype)
        {
            _ = System.Nullable.GetUnderlyingType(csharpPropertytype);

            return
                csharpPropertytype == typeof(float)
                || csharpPropertytype == typeof(double)
                || csharpPropertytype == typeof(int)
                || csharpPropertytype == typeof(sbyte)
                || csharpPropertytype == typeof(byte)
                || csharpPropertytype == typeof(short)
                || csharpPropertytype == typeof(uint)
                || csharpPropertytype == typeof(long)
                || csharpPropertytype == typeof(ulong)
                || csharpPropertytype == typeof(char)
                || csharpPropertytype == typeof(float?)
                || csharpPropertytype == typeof(double?)
                || csharpPropertytype == typeof(int?)
                || csharpPropertytype == typeof(sbyte?)
                || csharpPropertytype == typeof(byte?)
                || csharpPropertytype == typeof(short?)
                || csharpPropertytype == typeof(uint?)
                || csharpPropertytype == typeof(long?)
                || csharpPropertytype == typeof(ulong?)
                || csharpPropertytype == typeof(char?);
        }

        bool IsNullableType(Type csharpPropertytype) =>
            csharpPropertytype == typeof(float?)
            || csharpPropertytype == typeof(double?)
            || csharpPropertytype == typeof(int?)
            || csharpPropertytype == typeof(sbyte?)
            || csharpPropertytype == typeof(byte?)
            || csharpPropertytype == typeof(short?)
            || csharpPropertytype == typeof(uint?)
            || csharpPropertytype == typeof(long?)
            || csharpPropertytype == typeof(ulong?)
            || csharpPropertytype == typeof(char?)
            || csharpPropertytype == typeof(decimal?)
            || csharpPropertytype == typeof(DateTime?)
            || csharpPropertytype == typeof(TimeSpan?)
            || csharpPropertytype == typeof(bool?)
            || csharpPropertytype == typeof(string)
            || csharpPropertytype.IsInterface
            || csharpPropertytype.IsClass;

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
