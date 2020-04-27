using CustomerTestsExcel.ExcelToCode;
using System;
using System.Text;

namespace CustomerTestsExcel.Test
{
    public class TestProjectCreatorResults : LoggerBase
    {
        readonly StringBuilder logMessages = new StringBuilder();
        public string LogMessages => logMessages.ToString();

        protected override void Log(string message) =>
            logMessages.AppendLine(message);

    }
}