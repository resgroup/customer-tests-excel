namespace CustomerTestsExcel
{
    // split these out in to their own classes
    public interface ITableHeader
    {
        string PropertyName { get; }
        bool Equals(ITableHeader header);
    }
}
