using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CustomerTestsExcel.CodeOutputWriters
{
    public class HumanFriendlyFormatter : IHumanFriendlyFormatter
    {
        public string FormatValue(object value)
        {
            if (value is NoValue)
            {
                return "";
            }
            else if (value == null)
            {
                return "null";
            }
            else if (value is string)
            {
                return "\"" + value + "\"";
            }
            else
            {
                return value.ToString();
            }
        }

        public string FormatSpecificationSpecificClassName(string className)
        {
            return className.Replace("SpecificationSpecific", "").Replace("specificationSpecific", "");
        }

        public string FormatMethodName(string methodName)
        {
            string underscoresAndSpecificationSpecificRemoved =
                methodName.Replace('_', ' ')
                          .Replace("SpecificationSpecific", "");

            string replaced = Regex.Replace(underscoresAndSpecificationSpecificRemoved, "([A-Z])", " $1", RegexOptions.Compiled);
            string trimmed = replaced.Trim();
            return trimmed;
        }
    }
}
