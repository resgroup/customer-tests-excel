using CustomerTestsExcel.ExcelToCode;
using System;
using System.Text;

namespace CustomerTestsExcel.Test
{
    public class TestLogger : LoggerBase
    {
        readonly StringBuilder logMessages = new StringBuilder();

        protected override void Log(string message) =>
            logMessages.AppendLine(message);

        public string LogMessages => logMessages.ToString();
    }
}