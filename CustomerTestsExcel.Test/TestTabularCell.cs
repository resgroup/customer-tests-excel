namespace CustomerTestsExcel.Test
{
    internal class TestTabularCell : ITabularCell
    {
        public object Value { get; }

        public TestTabularCell(object value)
        {
            Value = value;
        }

        public bool IsFormula =>
            false;
    }
}