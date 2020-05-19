using CustomerTestsExcel.CodeOutputWriters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerTestsExcel.Test
{
    public class TestHumanFriendlyFormatter : IHumanFriendlyFormatter
    {
        public string FormatValue(object value) =>
                value.ToString();

        public string FormatSpecificationSpecificClassName(string className) =>
            className;

        public string FormatMethodName(string methodName) =>
            methodName;
    }
}
