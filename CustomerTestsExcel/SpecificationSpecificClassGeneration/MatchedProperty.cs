using System.Reflection;

namespace CustomerTestsExcel.SpecificationSpecificClassGeneration
{
    public struct MatchedProperty<T> where T: IGivenClassProperty
    {
        public PropertyInfo CsharpProperty;
        public T ExcelProperty;
    }
}