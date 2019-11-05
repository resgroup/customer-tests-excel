using CustomerTestsExcel.ExcelToCode;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public interface IGivenClassProperty
    {
        string Name { get; }
        ExcelPropertyType Type { get; }
    }
}