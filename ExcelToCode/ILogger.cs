using System;

namespace CustomerTestsExcel.ExcelToCode
{
    public interface ILogger
    {
        void LogIssuePreventingRoundTrip(string workbookName, string worksheetName, string issue);
        void LogAssemblyError(string error, Exception exception);
        void LogCsprojLoadError(string csprojFilename, Exception exception);
        void LogError(string workbookName, string worksheetName, string issue);
        void LogWarning(string workbookName, string worksheetName, string issue);
        bool HasErrors { get; }

    }
}