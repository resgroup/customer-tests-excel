using System;

namespace CustomerTestsExcel.ExcelToCode
{
    public abstract class LoggerBase : ILogger
    {
        protected abstract void Log(string message);

        public void LogIssuePreventingRoundTrip(string workbookName, string worksheetName, string issue) =>
            Log($"Warning: Workbook '{workbookName}', Worksheet '{worksheetName}' will not be able to round trip to Excel. {issue}");

        public void LogError(string workbookName, string worksheetName, string error) =>
            Log($"Error: Workbook '{workbookName}', Worksheet '{worksheetName}' could not be converted. {error}");

        public void LogWarning(string workbookName, string worksheetName, string issue) =>
            Log($"Warning: Workbook '{workbookName}', Worksheet '{worksheetName}'. {issue}");
    }
}