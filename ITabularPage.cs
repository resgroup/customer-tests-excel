namespace RES.Specification
{
    public interface ITabularPage
    {
        string Name { get; set; }

        object GetCell(uint row, uint column);
    }
}