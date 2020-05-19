namespace CustomerTestsExcel.ExcelToCode
{
    internal class AssertionTableHeader
    {
        internal string PropertyName { get; }
        internal string AssertionOperator { get; }
        internal string AssertionSpecificKey { get; }
        internal string AssertionSpecificValue { get; }

        internal AssertionTableHeader(
            string propertyName, 
            string assertionOperator, 
            string assertionSpecificKey,
            string assertionSpecificValue)
        {
            PropertyName = propertyName;
            AssertionOperator = assertionOperator;
            AssertionSpecificKey = assertionSpecificKey;
            AssertionSpecificValue = assertionSpecificValue;
        }

        internal uint Rows() =>
            string.IsNullOrWhiteSpace(AssertionSpecificValue)
                ? (uint) 2
                : 4;
    }
}
