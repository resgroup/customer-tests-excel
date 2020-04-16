using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using CustomerTestsExcel.Assertions;
using CustomerTestsExcel.ExcelToCode;

namespace CustomerTestsExcel
{
    public class CodeNameToExcelNameConverter : ICodeNameToExcelNameConverter
    {
        public string Specification =>
            "Specification";

        public string Given =>
            "Given a";

        public string WithProperties =>
            "With Properties";

        public string When =>
            "When";

        public string Assert =>
            "Assert";

        public string TableOf =>
            "table of";

        public string ListOf =>
            "list of";

        public string WithItem =>
            "With Item";

        public string AssertionClassPrefixAddedByGenerator { get; }

        public CodeNameToExcelNameConverter(string assertionClassPrefixAddedByGenerator)
        {
            AssertionClassPrefixAddedByGenerator = assertionClassPrefixAddedByGenerator;
        }

        public string CodeSpecificationClassNameToExcelName(string cSharpClassName)
        {
            return cSharpClassName.Replace("_", " ");
        }
        public string ExcelSpecificationNameToCodeSpecificationClassName(string excelSpecificationName)
        {
            return excelSpecificationName.Replace(" ", "_");
        }

        public string CodeClassNameToExcelName(string cSharpClassName)
        {
            return cSharpClassName.Replace("SpecificationSpecific", "").Replace("Specification.Setup.", "");
        }
        public string ExcelClassNameToCodeName(string excelSpecificationName)
        {
            var lastIndex = excelSpecificationName.LastIndexOf('.');
            var className = excelSpecificationName;
            var nameSpace = "";
            if (lastIndex > 0)
            {
                className = excelSpecificationName.Substring(lastIndex + 1);
                nameSpace = excelSpecificationName.Substring(0, lastIndex + 1) + "Specification.Setup.";
            }

            return nameSpace + "SpecificationSpecific" + className.Trim().Replace(" ", "_");
        }

        // the property names (not values) of the "Given" part of the test
        // Change "Calibrations_of" to "Calibrations of"
        public string GivenPropertyNameCodeNameToExcelName(string cSharpPropertyName)
        {
            var withoutOfPostfix = RemoveCsharpOfPostfix(cSharpPropertyName);

            return withoutOfPostfix + (CsharpPropertyNameHasOfPostfix(cSharpPropertyName) ? " of" : "");
        }

        // Change "Ambient Turbulence   of  "Ambient_Turbulence"
        // Designed for use with generating the specification specific classes
        public string GivenPropertyNameExcelNameToSutName(string excelPropertyName)
        {
            var withoutOf = RemoveExcelOfPostfix(excelPropertyName);

            var trimmedAndUnderscores = withoutOf.Trim().Replace(" ", "_");

            return trimmedAndUnderscores;
        }
        // Change "Calibrations   of  " to "Calibrations_of"
        public string GivenPropertyNameExcelNameToCodeName(string excelPropertyName)
        {
            var withoutOf = RemoveExcelOfPostfix(excelPropertyName);

            var trimmedAndUnderscores = withoutOf.Trim().Replace(" ", "_");

            var withOf = trimmedAndUnderscores + (ExcelPropertyNameHasOfPostfix(excelPropertyName) ? "_of" : "");

            return withOf;
        }

        bool ExcelPropertyNameHasOfPostfix(string excelPropertyName) =>
            excelPropertyName.EndsWith(" of");

        bool CsharpPropertyNameHasOfPostfix(string cSharpPropertyName) =>
            cSharpPropertyName.EndsWith("_of");

        // Change "Calibrations of" or "Calibrations_of" to "Calibrations"
        string RemoveExcelOfPostfix(string excelPropertyName)
        {
            const int ofPostfixLength = 3;

            return
                ExcelPropertyNameHasOfPostfix(excelPropertyName)
                ? excelPropertyName.Substring(0, excelPropertyName.Length - ofPostfixLength)
                : excelPropertyName;
        }
        // Change "Calibrations of" or "Calibrations_of" to "Calibrations"
        string RemoveCsharpOfPostfix(string cSharpPropertyName)
        {
            const int ofPostfixLength = 3;

            return
                CsharpPropertyNameHasOfPostfix(cSharpPropertyName)
                ? cSharpPropertyName.Substring(0, cSharpPropertyName.Length - ofPostfixLength)
                : cSharpPropertyName;
        }

        // the property names (not values) of the "Given" part of the test
        public string GivenTablePropertyNameCodeNameToExcelName(string cSharpPropertyName)
        {
            // hmmm, I should prbably call this from somewhere
            throw new NotImplementedException();
        }
        // Change "Calibrations    table of" to "Calibrations_table_of"
        public string GivenTablePropertyNameExcelNameToCodeName(string excelPropertyName) =>
            RemoveTableOfPostfix(excelPropertyName).Trim().Replace(" ", "_") + "_table_of";
        // Change "Calibrations    table of" to "Calibrations" (potentially this should also lower case the first character, but it doesn't at the moment, and ExcelToCode does it instead. I guess if other languages are supported in the future, this class shouldn't do the casing, so maybe its best not being done here.)
        public string GivenTablePropertyNameExcelNameToCodeVariableName(string excelPropertyName)
        {
            var methodName = GivenTablePropertyNameExcelNameToCodeName(excelPropertyName);

            return methodName.Substring(0, methodName.Length - TableOf.Length - 1);
        }
        // Change "Calibrations table of" to "Calibrations"
        // Designed for use with specification specific class generation
        public string GivenTablePropertyNameExcelNameToSutName(string excelPropertyName) =>
            RemoveTableOfPostfix(excelPropertyName).Trim().Replace(" ", "_");
        static string RemoveTableOfPostfix(string excelPropertyName) => 
            excelPropertyName.Substring(0, excelPropertyName.Length - 9);



        // the property names (not values) of the "Given" part of the test
        public string GivenListPropertyNameCodeNameToExcelName(string cSharpPropertyName)
        {
            string withoutListOf = RemoveListOfPostfix(cSharpPropertyName);

            return withoutListOf + " list of";
        }
        // Change "Calibrations    table of" to "Calibrations_table_of"
        public string GivenListPropertyNameExcelNameToCodeName(string excelPropertyName)
        {
            string withoutListOf = RemoveListOfPostfix(excelPropertyName);

            return withoutListOf.Trim().Replace(" ", "_") + "_list_of";
        }

        public string GivenListPropertyNameExcelNameToCodeVariableName(string excelPropertyName)
        {
            var methodName = GivenListPropertyNameExcelNameToCodeName(excelPropertyName);

            return methodName.Substring(0, methodName.Length - ListOf.Length - 1);
        }

        // Change "Calibrations list of" or "Calibrations_list_of" to "Calibrations"
        string RemoveListOfPostfix(string propertyName) =>
            propertyName.Substring(0, propertyName.Length - 8);

        public string ActionExcelNameToCodeName(string excelActionName)
        {
            return excelActionName.Trim().Replace(" ", "_");
        }
        public string ActionCodeNameToExcelName(string actionName)
        {
            return actionName.Trim().Replace("_", " ");
        }


        // the value side of an assertion (ig the "true" bit of "IsValid == true"
        public object AssertValueCodeNameToExcelName(object cSharpAssertValue)
        {
            if (cSharpAssertValue is decimal)
            {
                // these will appear in excel as text and not numbers, which is a shame, but otherwise strange things can happen, such as: (decimal) 50 doesn't equal (int) 50, so I think its better this way
                // we only want this for assertions, where this strange stuff happens
                return cSharpAssertValue.ToString() + "m";
            }
            else
            {
                // we will need to test the cSharpAssertValue at some point to see if it needs namespacing and if so pass in the namespace. See ReportSpecificationProperty
                return PropertyValueCodeToExcel("", cSharpAssertValue);
            }
        }
        public string AssertValueExcelNameToCodeName(string excelPropertyName, object excelAssertValue)
        {
            return PropertyValueExcelToCode(excelPropertyName, excelAssertValue);
        }

        public string AssertionSubPropertyCodePropertyNameToExcelName(string cSharpAssertPropertyName)
        {
            return cSharpAssertPropertyName.Replace("SpecificationSpecific", "");
        }
        public string AssertionSubPropertyExcelNameToCodeMethodName(string excelAssertPropertyName)
        {
            return excelAssertPropertyName.Trim();
        }

        public string AssertionSubPropertyCodeClassNameToExcelName(string cSharpAssertClassName)
        {
            return RemoveAssertionClassPrefixAddedByGenerator(cSharpAssertClassName);
        }
        string RemoveAssertionClassPrefixAddedByGenerator(string cSharpAssertName) =>
            cSharpAssertName.StartsWith(AssertionClassPrefixAddedByGenerator) ? cSharpAssertName.Substring(AssertionClassPrefixAddedByGenerator.Length) : cSharpAssertName;
        // It seems like this should include the AssertionClassPrefixAddedByGenerator, being as the matching method above removes it
        // It must be added somewhere else, but we should move it here instead.
        public string AssertionSubPropertyExcelNameToCodeClassName(string excelAssertClassName)
        {
            return excelAssertClassName;
        }

        public string AssertionSubClassExcelNameToCodeName(string excelClassName) =>
            AssertionClassPrefixAddedByGenerator + excelClassName;


        // the property name side of an assertion (ig the "IsValid" bit of "IsValid == true"
        public string AssertPropertyCodeNameToExcelName(string cSharpAssertName)
        {
            return cSharpAssertName.Replace("_", " ");
        }

        public string AssertPropertyExcelNameToCodeName(string excelAssertName)
        {
            return excelAssertName.Trim().Replace(" ", "_");
        }

        // the operator part of an assertion (ig the "==" bit of "IsValid == true")
        public string AssertionOperatorCodeNameToExcelName(AssertionOperator assertionOperator)
        {
            return assertionOperator.ToDescription();
        }

        public string ExcelFileNameToCodeNamespacePart(string workBookName)
        {
            return workBookName.Replace(" ", "_");
        }

        public string CodeNamespaceToExcelFileName(string cSharpAssemblyName)
        {
            return cSharpAssemblyName.Split('.').Last().Replace("_", " ");
        }

        public object PropertyValueCodeToExcel(string cSharpNameNamespace, object cSharpPropertyValue)
        {
            if (cSharpPropertyValue is NoValue)
            {
                return "";
            }
            else if (cSharpPropertyValue == null)
            {
                return "null";
            }
            else if (cSharpPropertyValue is string)
            {
                return "\"" + cSharpPropertyValue + "\"";
            }
            else if (cSharpPropertyValue.GetType().IsEnum)
            {
                return cSharpNameNamespace + cSharpPropertyValue.GetType().Name + "." + cSharpPropertyValue;
            }
            else if (cSharpPropertyValue is TimeSpan)
            {
                return cSharpPropertyValue;
            }
            else if (cSharpPropertyValue is DateTime)
            {
                return cSharpPropertyValue;
            }
            else
            {
                return cSharpPropertyValue;
            }
        }
        public string PropertyValueExcelToCode(string excelPropertyName, object excelPropertyValue)
        {
            var excelPropertyType = ExcelPropertyTypeFromCellValue(excelPropertyValue);

            if (excelPropertyType == ExcelPropertyType.Null)
                return NullForPropertyNothingForMethod(excelPropertyName);

            if (excelPropertyType == ExcelPropertyType.DateTime)
                return DateTimeCsharpValueFromExcelValue(excelPropertyValue);

            if (excelPropertyType == ExcelPropertyType.TimeSpan)
                return TimespanValueFromExcelValue(excelPropertyValue);

            var stringValue = StringValueFromExcelValue(excelPropertyValue);

            if (excelPropertyType == ExcelPropertyType.StringNull)
                return stringValue;

            if (excelPropertyType == ExcelPropertyType.Enum)
                return stringValue;

            if (excelPropertyType == ExcelPropertyType.Number)
                return stringValue;

            if (excelPropertyType == ExcelPropertyType.Decimal)
                return stringValue;

            if (excelPropertyType == ExcelPropertyType.Boolean)
                return stringValue.ToLower();

            if (excelPropertyType == ExcelPropertyType.Function)
                return "";

            // make sure strings are quoted (which is optional in excel)
            if (excelPropertyType == ExcelPropertyType.String)
                return EnsureSurroundedByDoubleQuotes(stringValue);

            throw new ExcelToCodeException($"Unrecognised ExcelPropertyType: {excelPropertyType}");
        }

        public ExcelPropertyType ExcelPropertyTypeFromCellValue(object excelPropertyValue)
        {
            if (excelPropertyValue == null)
                return ExcelPropertyType.Null;

            var stringValue = StringValueFromExcelValue(excelPropertyValue);

            if (stringValue == "")
                return ExcelPropertyType.Null;

            if (stringValue.ToLower() == "null")
                return ExcelPropertyType.StringNull;

            // Dates are a total disaster in Excel, so this might not always work as expected
            // See ExcelTabularPage
            if (excelPropertyValue is DateTime)
                return ExcelPropertyType.DateTime;

            if (excelPropertyValue is TimeSpan)
                return ExcelPropertyType.TimeSpan;

            if (IsNumeric(excelPropertyValue))
                return ExcelPropertyType.Number;

            if (IsEnumBeCarefulAlsoMatchesFloats(stringValue))
                return ExcelPropertyType.Enum;

            // Its a shame that formatting numbers as "Money" in Excel does not help us here
            if (stringValue.EndsWith("m", StringComparison.InvariantCulture) && IsNumeric(stringValue.Substring(0, stringValue.Length - 1)))
                return ExcelPropertyType.Decimal;

            if (stringValue.ToLower() == "false")
                return ExcelPropertyType.Boolean;

            if (stringValue.ToLower() == "true")
                return ExcelPropertyType.Boolean;

            // by default treat values as strings
            // This means we have to be able to detect all the other possible types of value, or maybe write code to convert them from a string
            return ExcelPropertyType.String;
        }

        static string StringValueFromExcelValue(object excelPropertyValue) =>
            string.Format(CultureInfo.InvariantCulture, "{0}", excelPropertyValue).Trim();

        static string DateTimeCsharpValueFromExcelValue(object excelPropertyValue) =>
            string.Format(CultureInfo.InvariantCulture, "DateTime.Parse(\"{0:s}\")", excelPropertyValue);

        static string TimespanValueFromExcelValue(object excelPropertyValue) =>
            string.Format(CultureInfo.InvariantCulture, "TimeSpan.Parse(\"{0:c}\")", excelPropertyValue);

        static string EnsureSurroundedByDoubleQuotes(string stringValue) =>
                 (IsSurroundedByDoubleQuotes(stringValue))
                    ? stringValue
                    : $"\"{stringValue}\"";

        static bool IsSurroundedByDoubleQuotes(string stringValue) =>
            stringValue.StartsWith("\"", StringComparison.InvariantCulture)
            && stringValue.EndsWith("\"", StringComparison.InvariantCulture);

        string NullForPropertyNothingForMethod(string excelPropertyName)
        {
            return (excelPropertyName.EndsWith(" of", StringComparison.InvariantCulture)) ? "null" : "";
        }

        static bool IsNumeric(object value)
        {
            return double.TryParse(
                Convert.ToString(value),
                NumberStyles.Any,
                NumberFormatInfo.InvariantInfo, out double _);
        }

        static bool IsEnumBeCarefulAlsoMatchesFloats(string value)
        {
            return (
                value.StartsWith("Base.", StringComparison.CurrentCultureIgnoreCase)
                || (!string.IsNullOrWhiteSpace(value) && Regex.Match(value, @"[A-Za-z0-9_]*\.[A-Za-z0-9_]*").Value == value)
            );
        }
    }
}
