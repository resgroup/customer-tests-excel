namespace CustomerTestsExcel
{
    public interface ITabularCell
    {
        object Value { get; }
        bool IsFormula { get; } 
    }
}