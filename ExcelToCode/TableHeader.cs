namespace CustomerTestsExcel.ExcelToCode
{
    abstract class TableHeader
    {
        public string PropertyName { get; }
        public uint EndRow { get; }
        public uint EndColumn { get; }
        public bool IsRoundTrippable { get; }

        protected TableHeader(
            string propertyName, 
            uint endRow, 
            uint endColumn,
            bool isRoundTrippable)
        {
            PropertyName = propertyName;
            EndRow = endRow;
            EndColumn = endColumn;
            IsRoundTrippable = isRoundTrippable;
        }
    }
}
