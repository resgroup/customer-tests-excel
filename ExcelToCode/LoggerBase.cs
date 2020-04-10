using System;

namespace CustomerTestsExcel.ExcelToCode
{
    public abstract class LoggerBase : ILogger
    {
        protected abstract void Log(string message);

        public void LogIssuePreventingRoundTrip(string workbookName, string worksheetName, string issue) =>
            Log($"Warning: Workbook '{workbookName}', Worksheet '{worksheetName}' will not be able to round trip to Excel. {issue}");

        public void LogAssemblyError(string assemblyFilename, Exception exception) =>
            Log(
$@"Error: Assembly '{assemblyFilename}' could not be loaded, this will probably mean that setup class generation will not happen as desired.
This filename comes from the `/assembliesUnderTest` command line parameter.
The error returned is:
{exception}");

        public void LogError(string workbookName, string worksheetName, string error) =>
            Log($"Error: Workbook '{workbookName}', Worksheet '{worksheetName}' could not be converted. {error}");

        public void LogWarning(string workbookName, string worksheetName, string issue) =>
            Log($"Warning: Workbook '{workbookName}', Worksheet '{worksheetName}'. {issue}");
    }
}