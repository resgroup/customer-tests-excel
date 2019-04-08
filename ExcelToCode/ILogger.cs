namespace CustomerTestsExcel.ExcelToCode
{
    public interface ILogger
    {
        void LogIssuePreventingRoundTrip(string workbookName, string worksheetName, string issue);
        void LogError(string workbookName, string worksheetName, string issue);
        void LogWarning(string workbookName, string worksheetName, string issue);
    }
}