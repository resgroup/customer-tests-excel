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
            if (excelPropertyType == ExcelPropertyType.Decimal && csharpPropertytype == typeof(Decimal))
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
    }
}