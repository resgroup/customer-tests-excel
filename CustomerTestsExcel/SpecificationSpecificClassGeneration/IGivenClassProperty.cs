using CustomerTestsExcel.ExcelToCode;
using System;
using System.Collections.Generic;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public interface IGivenClassProperty
    {
        string Name { get; }
        ExcelPropertyType Type { get; }
        bool TypesMatch(Type cSharpPropertytype);
    }
}