using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RES.Specification
{
    public class ExcelFormulaDoesNotMatchCodeAssertion<T> : IAssertion<T>
    {
        string _propertyName;
        string _excelValue;
        string _csharpValue;

        public ExcelFormulaDoesNotMatchCodeAssertion(string propertyName, string excelValue, string csharpValue)
        {
            _propertyName = propertyName;
            _excelValue = excelValue;
            _csharpValue = csharpValue;
        }

        public bool Passed(T sut)
        {
            return false;
        }

        public void Write(T sut, bool passed, ITestOutputWriter writer)
        {
            writer.CodeValueDoesNotMatchExcelFormula(_propertyName, _excelValue, _csharpValue);
        }
    }
}
