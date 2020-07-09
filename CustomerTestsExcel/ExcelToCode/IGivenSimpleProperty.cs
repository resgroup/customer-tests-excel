namespace CustomerTestsExcel.ExcelToCode
{
    public interface IGivenSimpleProperty
    {
        string PropertyOrFunctionName { get; }
        string CsharpCodeRepresentation { get; }
        ExcelPropertyType ExcelPropertyType { get; }
        bool Nullable { get; }
    }
}
