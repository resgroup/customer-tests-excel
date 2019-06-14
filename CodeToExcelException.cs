using System;

namespace CustomerTestsExcel
{
    public class CodeToExcelException : Exception
    {
        public CodeToExcelException() : base() { }
        public CodeToExcelException(string message) : base(message) { }
        public CodeToExcelException(string message, Exception inner) : base(message, inner) { }
    }
}
