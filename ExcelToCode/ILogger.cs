namespace CustomerTestsExcel.ExcelToCode
{
    public interface ILogger
    {
        void LogIssuePreventingRoundTrip(string workbookName, string worksheetName, string issue);
    }
}