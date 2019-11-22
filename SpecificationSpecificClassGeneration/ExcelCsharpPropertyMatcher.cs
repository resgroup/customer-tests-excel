using CustomerTestsExcel.ExcelToCode;
using System;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class ExcelCsharpPropertyMatcher
    {
        public ExcelCsharpPropertyMatcher()
        {
        }

        public bool PropertiesMatch(
            Type csharpPropertytype,
            ExcelPropertyType excelPropertyType)
        {
            if (excelPropertyType == ExcelPropertyType.Null && IsNullableType(csharpPropertytype))
                return true;

            if (excelPropertyType == ExcelPropertyType.Enum && csharpPropertytype.IsEnum)
                return true;

            if (excelPropertyType == ExcelPropertyType.Number && IsNumberType(csharpPropertytype))
                return true;

            if (excelPropertyType == ExcelPropertyType.Decimal && csharpPropertytype == typeof(decimal))
                return true;

            if (excelPropertyType == ExcelPropertyType.DateTime && csharpPropertytype == typeof(DateTime))
                return true;

            if (excelPropertyType == ExcelPropertyType.TimeSpan && csharpPropertytype == typeof(TimeSpan))
                return true;

            if (excelPropertyType == ExcelPropertyType.String && csharpPropertytype == typeof(string))
                return true;

            if (excelPropertyType == ExcelPropertyType.StringNull && csharpPropertytype == typeof(string))
                return true;

            if (excelPropertyType == ExcelPropertyType.Boolean && csharpPropertytype == typeof(bool))
                return true;

            return false;
        }

        bool IsNumberType(Type csharpPropertytype) =>
            csharpPropertytype == typeof(float)
            || csharpPropertytype == typeof(double)
            || csharpPropertytype == typeof(int)
            || csharpPropertytype == typeof(sbyte)
            || csharpPropertytype == typeof(byte)
            || csharpPropertytype == typeof(short)
            || csharpPropertytype == typeof(uint)
            || csharpPropertytype == typeof(long)
            || csharpPropertytype == typeof(ulong)
            || csharpPropertytype == typeof(char);

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
    }
}