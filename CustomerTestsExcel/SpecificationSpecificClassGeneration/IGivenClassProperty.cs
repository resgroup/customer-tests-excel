using CustomerTestsExcel.ExcelToCode;
using System;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public interface IGivenClassProperty
    {
        string Name { get; }
        string ClassName { get; }
        ExcelPropertyType Type { get; }
        string ExampleValue { get; }
        bool TypesMatch(Type cSharpPropertytype);
    }
}