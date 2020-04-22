using System.Reflection;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public struct MatchedProperty
    {
        public PropertyInfo CsharpProperty;
        public IGivenClassProperty ExcelProperty;
    }
}