namespace CustomerTestsExcel.ExcelToCode
{
    class PropertyTableHeader : TableHeader
    {
        const bool ROUND_TRIPPABLE = true;

        public PropertyTableHeader(
            string propertyName,
            uint endRow,
            uint endColumn)
            : base(
                  propertyName,
                  endRow,
                  endColumn,
                  ROUND_TRIPPABLE)
        {
        }
    }
}
