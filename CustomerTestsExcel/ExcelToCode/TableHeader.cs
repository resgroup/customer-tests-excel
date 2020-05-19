namespace CustomerTestsExcel.ExcelToCode
{
    public abstract class TableHeader
    {
        public string ExcelPropertyName { get; }
        public uint EndRow { get; }
        public uint EndColumn { get; }
        public bool IsRoundTrippable { get; }

        protected TableHeader(
            string excelPropertyName,
            uint endRow,
            uint endColumn,
            bool isRoundTrippable)
        {
            ExcelPropertyName = excelPropertyName;
            EndRow = endRow;
            EndColumn = endColumn;
            IsRoundTrippable = isRoundTrippable;
        }

        public override string ToString() =>
            $"{{ PropertyName: {ExcelPropertyName}, EndRow: {EndRow}, EndColumn: {EndColumn}, IsRoundTrippable: {IsRoundTrippable} }}";
    }
}
