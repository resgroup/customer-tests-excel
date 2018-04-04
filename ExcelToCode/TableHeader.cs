namespace CustomerTestsExcel.ExcelToCode
{
    abstract class TableHeader
    {
        public string PropertyName { get; }
        public uint EndRow { get; }
        public uint EndColumn { get; }

        protected TableHeader(string propertyName, uint endRow, uint endColumn)
        {
            PropertyName = propertyName;
            EndRow = endRow;
            EndColumn = endColumn;
        }
    }
}
