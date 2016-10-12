using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RES.Specification
{
    public class CodeNameToExcelNameConverter : ICodeNameToExcelNameConverter
    {
        public const string SPECIFICATION = "Specification";
        public const string GIVEN = "Given a";
        public const string CREATIONAL = "With Creational";
        public const string PROPERTIES = "With Properties";
        public const string WHEN = "When";
        public const string WHEN_VALIDATING = "Validating";
        public const string WHEN_CALCULATING = "Calculating";
        public const string ASSERT = "Assert";

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
            if(lastIndex > 0)
            {
                className = excelSpecificationName.Substring(lastIndex + 1);
                nameSpace = excelSpecificationName.Substring(0, lastIndex + 1) + "Specification.Setup.";
            }

            return nameSpace + "SpecificationSpecific" + className;
        }

        // the property names (not values) of the "Given" part of the test
        public string GivenPropertyNameCodeNameToExcelName(string cSharpPropertyName, bool isChild, int? indexInParent)
        {
            if (isChild == true)
            {
                return cSharpPropertyName.Replace("_", "(" + indexInParent.ToString() + ") ");
            }
            else
            {
                return cSharpPropertyName.Replace("_", " ");
            }
        }
        public string GivenPropertyNameExcelNameToCodeName(string excelPropertyName)
        {
            int start = excelPropertyName.IndexOf("(", StringComparison.InvariantCulture);
            int end = excelPropertyName.IndexOf(")", StringComparison.InvariantCulture);

            if (start != -1)
            {
                excelPropertyName = excelPropertyName.Substring(0, start) + excelPropertyName.Substring(end + 1);
            }

            return excelPropertyName.Replace(" ", "_");
        }

        public string ActionCodeNameToExcelName(string actionName)
        {
            return actionName.Replace("_", " ");
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

        public string AssertionSubPropertyCodeNameToExcelName(string cSharpAssertPropertyName)
        {
            return cSharpAssertPropertyName.Replace("SpecificationSpecific", "");
        }
        public string AssertionSubPropertyExcelNameToCodeName(string excelAssertPropertyName)
        {
            return excelAssertPropertyName;
        }

        public string AssertionSubClassExcelNameToCodeName(string excelClassName)
        {
            return excelClassName;
        }


        // the property name side of an assertion (ig the "IsValid" bit of "IsValid == true"
        public string AssertPropertyCodeNameToExcelName(string cSharpAssertName)
        {
            return cSharpAssertName.Replace("_", " ");
        }
        public string AssertPropertyExcelNameToCodeName(string excelAssertName)
        {
            return excelAssertName.Replace(" ", "_");
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
            //// the excel parser does not appear to return a decimal for cells formatted as "money" so this currently does not work. i am leaving it here because it could become useful if the feature is implemented somewhere else, like in RES.Excel.
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
            double parsedDouble;
            return Double.TryParse(Convert.ToString(value), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out parsedDouble);
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

        public string Creational
        {
            get { return CREATIONAL; }
        }

        public string Properties
        {
            get { return PROPERTIES; }
        }

        public string When
        {
            get { return WHEN; }
        }

        public string Assert
        {
            get { return ASSERT; }
        }

        public string WhenValidating 
        {
            get { return WHEN_VALIDATING; } 
        }

        public string WhenCalculating 
        {
            get { return WHEN_CALCULATING; } 
        }
        


    }
}
