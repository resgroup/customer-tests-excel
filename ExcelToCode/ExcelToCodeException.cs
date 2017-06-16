﻿using System;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCodeException : Exception
    {
        public ExcelToCodeException() : base() { }
        public ExcelToCodeException(string message) : base(message) { }
        public ExcelToCodeException(string message, Exception inner) : base(message, inner) { }
    }
}
