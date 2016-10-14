namespace RES.Specification
{
    public interface ITabularCell
    {
        object Value { get; }
        bool IsFormula { get; } 
    }
}