using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Specification
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

        public string  FormatSpecificationSpecificClassName(string className)
        {
            return className.Replace("SpecificationSpecific", "").Replace("specificationSpecific", "");
        }

        public string FormatMethodName(string methodName)
        {
            string underscoresAndSpecificationSpecificRemoved = methodName.Replace('_', ' ').Replace("SpecificationSpecific", ""); 
            return System.Text.RegularExpressions.Regex.Replace(underscoresAndSpecificationSpecificRemoved, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }

    }
}
