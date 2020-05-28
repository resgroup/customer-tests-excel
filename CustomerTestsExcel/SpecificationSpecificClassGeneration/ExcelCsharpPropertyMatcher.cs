using CustomerTestsExcel.ExcelToCode;
using System;
using System.Reflection;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public class ExcelCsharpPropertyMatcher
    {
        public ExcelCsharpPropertyMatcher()
        {
        }

        public bool PropertiesMatch(
            PropertyInfo cSharpProperty,
            IGivenClassProperty excelProperty) =>
                NamesMatch(cSharpProperty.Name, excelProperty.Name)
                && excelProperty.TypesMatch(cSharpProperty.PropertyType);

        public bool MethodsMatch(
            MethodInfo cSharpMethod,
            IGivenClassProperty excelProperty)
        {
            return false;

            //if (!NamesMatch(cSharpMethod.Name, excelProperty.Name))
            //    return false;

            //if (cSharpMethod.ReturnType != typeof(void))
            //    return false;

            //if (cSharpMethod.GetParameters().Length == 0)
            //    return excelProperty.Type == ExcelPropertyType.Null;
            //else if (cSharpMethod.GetParameters().Length == 1)
            //    return excelProperty.TypesMatch(cSharpMethod.GetParameters()[0].ParameterType);

            //return false;
        }

        public bool NamesMatch(
            string cSharpName,
            string excelName) =>
                cSharpName == excelName;

    }
}