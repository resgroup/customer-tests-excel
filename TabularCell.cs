namespace RES.Specification
{
    internal class TabularCell : ITabularCell
    {
        public object Value { get; }
        public bool IsFormula { get; }

        public TabularCell(object value, bool isFormula)
        {
            this.Value = value;
            this.IsFormula = isFormula;
        }
    }
}