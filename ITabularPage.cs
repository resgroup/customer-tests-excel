namespace RES.Specification
{
    public interface ITabularPage
    {
        uint MaxColumn { get; }
        uint MaxRow { get; }
        string Name { get; set; }

        ITabularCell GetCell(uint row, uint column);
        void SetCell(uint row, uint column, object Value);
    }
}