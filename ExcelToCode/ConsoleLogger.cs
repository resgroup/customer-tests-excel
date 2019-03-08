using System;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ConsoleLogger : LoggerBase
    {
        protected override void Log(string message) =>
            Console.WriteLine(message);
    }
}