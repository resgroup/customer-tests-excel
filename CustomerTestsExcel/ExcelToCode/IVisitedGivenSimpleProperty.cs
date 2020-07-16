namespace CustomerTestsExcel.ExcelToCode
{
    public interface IVisitedGivenSimpleProperty
    {
        string PropertyOrFunctionName { get; }
        string CsharpCodeRepresentation { get; }
        ExcelPropertyType ExcelPropertyType { get; }
        bool Nullable { get; }
    }
}
