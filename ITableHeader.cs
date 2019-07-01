namespace CustomerTestsExcel
{
    public interface ITableHeader
    {
        string PropertyName { get; }
        bool Equals(ITableHeader header);
    }
}
