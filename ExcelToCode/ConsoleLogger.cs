using System;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ConsoleLogger : ILogger
    {
        public void LogIssuePreventingRoundTrip(string workbookName, string worksheetName, string issue)
        {
            Console.WriteLine($"Warning: Workbook '{workbookName}', Worksheet '{worksheetName}' will not be able to round trip to Excel. {issue}");
        }
    }
}