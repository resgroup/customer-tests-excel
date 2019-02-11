using CustomerTestsExcel.ExcelToCode;
using System;
using System.Text;

namespace CustomerTestsExcel.Test
{
    public class TestLogger : ILogger
    {
        readonly StringBuilder log = new StringBuilder(); 

        public void LogIssuePreventingRoundTrip(string workbookName, string worksheetName, string issue)
        {
            log.AppendLine(workbookName);
            log.AppendLine(worksheetName);
            log.AppendLine(issue);
        }

        public string Log => log.ToString();
    }
}