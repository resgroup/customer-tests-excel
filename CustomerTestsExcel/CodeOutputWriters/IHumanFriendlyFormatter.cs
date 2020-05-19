using System;
namespace CustomerTestsExcel.CodeOutputWriters
{
    public interface IHumanFriendlyFormatter
    {
        string FormatValue(object value);

        string FormatSpecificationSpecificClassName(string className);

        string FormatMethodName(string methodName);
    }
}
