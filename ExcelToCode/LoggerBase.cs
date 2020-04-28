﻿using System;
using System.Collections.Generic;

namespace CustomerTestsExcel.ExcelToCode
{
    public abstract class LoggerBase : ILogger
    {
        public bool HasErrors { get; private set; }

        protected abstract void Log(string message);

        public void LogIssuePreventingRoundTrip(string workbookName, string worksheetName, string issue) =>
            Log($"Warning: Workbook '{workbookName}', Worksheet '{worksheetName}' will not be able to round trip to Excel. {issue}");

        public void LogAssemblyError(string assemblyFilename, Exception exception) =>
            Log(
$@"Error: Assembly '{assemblyFilename}' could not be loaded (the framework modifies existing projects, and doesn't create new ones).
This filename comes from the `/folder` and `/project` command line parameters.
The error returned is:
{exception}");

        public void LogCsprojLoadError(string csprojFilename, Exception exception) =>
            LogError(
$@"Error: Project '{csprojFilename}' could not be loaded.
This filename comes from the `/assembliesUnderTest` command line parameter.
The error returned is:
{exception}");

        public void LogCsprojSaveError(string csprojFilename, Exception exception) =>
            LogError(
$@"Error: Project '{csprojFilename}' could not be save.
This filename comes from the `/assembliesUnderTest` command line parameter.
The error returned is:
{exception}");

        public void LogWorkbookError(string workbookName, string worksheetName, string error) =>
            LogError($"Error: Workbook '{workbookName}', Worksheet '{worksheetName}' could not be converted. {error}");

        public void LogWarning(string workbookName, string worksheetName, string issue) =>
            Log($"Warning: Workbook '{workbookName}', Worksheet '{worksheetName}'. {issue}");

        void LogError(string message)
        {
            HasErrors = true;

            Log(message);
        }

    }
}