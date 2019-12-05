using CustomerTestsExcel.ExcelToCode;
using System;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public interface IGivenClassProperty
    {
        string Name { get; }
        ExcelPropertyType Type { get; }
        bool TypesMatch(Type cSharpPropertytype);
    }
}