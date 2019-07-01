using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace CustomerTestsExcel.Assertions
{
    public class ExcelFormulaDoesNotMatchCodeAssertion<T> : IAssertion<T>
    {
        private readonly string _propertyName;
        private readonly string _excelValue;
        private readonly string _csharpValue;

        public ExcelFormulaDoesNotMatchCodeAssertion(string propertyName, string excelValue, string csharpValue)
        {
            _propertyName = propertyName;
            _excelValue = excelValue;
            _csharpValue = csharpValue;
        }

        public bool Passed(T sut) => false;

        public void Write(T sut, bool passed, ITestOutputWriter writer)
        {
            writer.CodeValueDoesNotMatchExcelFormula(_propertyName, _excelValue, _csharpValue);
        }
    }
}
