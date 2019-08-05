using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using CustomerTestsExcel.Assertions;

namespace CustomerTestsExcel
{
    public class CodeNameToExcelNameConverter : ICodeNameToExcelNameConverter
    {
        public const string SPECIFICATION = "Specification";
        public const string GIVEN = "Given a";
        public const string WITH_PROPERTIES = "With Properties";
        public const string WHEN = "When";
        public const string ASSERT = "Assert";

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

        // Change "Calibrations(0) of" to "Calibrations of"
        string RemoveArrayIndex(string excelPropertyName)
        {
            int start = excelPropertyName.IndexOf("(", StringComparison.InvariantCulture);
            int end = excelPropertyName.IndexOf(")", StringComparison.InvariantCulture);

            if (start != -1)
            {
                excelPropertyName = excelPropertyName.Substring(0, start) + excelPropertyName.Substring(end + 1);
            }
            return excelPropertyName;
        }

        // the property names (not values) of the "Given" part of the test
        public string GivenTablePropertyNameCodeNameToExcelName(string cSharpPropertyName)
        {
            // hmmm, I should prbably call this from somewhere
            throw new NotImplementedException();
        }
        // Change "Calibrations    table of" to "Calibrations_table_of"
        public string GivenTablePropertyNameExcelNameToCodeName(string excelPropertyName)
        {
            string withoutIndex = RemoveArrayIndex(excelPropertyName);

            string withoutTableOf = RemoveTableOfPostfix(withoutIndex);

            return withoutTableOf.Trim().Replace(" ", "_") + "_table_of";
        }
        // Change "Calibrations    table of" to "Calibrations" (potentially this should also lower case the first character, but it doesn't at the moment, and ExcelToCode does it instead. I guess if other languages are supported in the future, this class shouldn't do the casing, so maybe its best not being done here.)
        public string GivenTablePropertyNameExcelNameToCodeVariableName(string excelPropertyName)
        {
            var methodName = GivenTablePropertyNameExcelNameToCodeName(excelPropertyName);

            return methodName.Substring(0, methodName.Length - TableOf.Length - 1);
        }

        // Change "Calibrations table of" to "Calibrations"
        string RemoveTableOfPostfix(string excelPropertyName) =>
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
                return ((TimeSpan)cSharpPropertyValue).ToString();
            }
            else if (cSharpPropertyValue is DateTime)
            {
                return ((DateTime)cSharpPropertyValue).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return cSharpPropertyValue;
            }
        }
        public string PropertyValueExcelToCode(string excelPropertyName, object excelPropertyValue)
        {
            if (excelPropertyValue == null)
                return NullForPropertyNothingForMethod(excelPropertyName);

            if (excelPropertyValue is DateTime)
                return string.Format(CultureInfo.InvariantCulture, "DateTime.Parse(\"{0:s}\")", excelPropertyValue);

            if (excelPropertyValue is TimeSpan)
                return string.Format(CultureInfo.InvariantCulture, "TimeSpan.Parse(\"{0:c}\")", excelPropertyValue);

            if (excelPropertyValue is DateTimeOffset)
                return string.Format(CultureInfo.InvariantCulture, "DateTimeOffset.Parse(\"{0:u}\")", excelPropertyValue);

            var stringValue = string.Format(CultureInfo.InvariantCulture, "{0}", excelPropertyValue).Trim();

            // enum
            if (IsEnum(stringValue))
                return stringValue;

            // number
            if (IsNumeric(excelPropertyValue))
                return stringValue;

            //decimal
            if (stringValue.EndsWith("m", StringComparison.InvariantCulture) && IsNumeric(stringValue.Substring(0, stringValue.Length - 1)))
                return stringValue;
            //// the excel parser does not appear to return a decimal for cells formatted as "money" so this currently does not work. i am leaving it here because it could become useful if the feature is implemented somewhere else.
            //if (excelPropertyValue is decimal) 
            //    return string.Format(CultureInfo.InvariantCulture, "{0}m", excelPropertyValue);

            if (stringValue.ToLower() == "false")
            {
                return "false";
            }
            else if (stringValue.ToLower() == "true")
            {
                return "true";
            }
            else if (stringValue.ToLower() == "null")
            {
                return "null";
            }
            else if (stringValue == "")
            {
                return NullForPropertyNothingForMethod(excelPropertyName);
            }
            else
            {
                // by default treat values as strings and give them quotes
                // This means we have to be able to detect all the other possible types of value, or maybe write code to convert them from a string
                // Other possible types are currently Dates, numbers (including decimals) and enums.
                if (stringValue.StartsWith("\"", StringComparison.InvariantCulture) && stringValue.EndsWith("\"", StringComparison.InvariantCulture))
                    return stringValue;
                return $"\"{stringValue}\"";
            }
        }

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

        static bool IsEnum(string value)
        {
            return (
                value.StartsWith("Base.", StringComparison.CurrentCultureIgnoreCase)
                || (!string.IsNullOrWhiteSpace(value) && Regex.Match(value, @"[A-Za-z0-9_]*\.[A-Za-z0-9_]*").Value == value)
            );
        }

        public string Specification
        {
            get { return SPECIFICATION; }
        }

        public string Given
        {
            get { return GIVEN; }
        }

        public string WithProperties
        {
            get { return WITH_PROPERTIES; }
        }

        public string When
        {
            get { return WHEN; }
        }

        public string Assert
        {
            get { return ASSERT; }
        }

        public string TableOf =>
            "table of";

        public string ListOf =>
            "list of";

        public string WithItem =>
            "With Item";
    }
}
