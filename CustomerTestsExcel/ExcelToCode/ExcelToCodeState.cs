using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCodeState
    {
        public ExcelToCodeGiven Given { get; }
        public ExcelToCodeTable Table { get; }
        public ExcelToCodeSimpleProperty SimpleProperty { get; }
        public ExcelToCodeComplexProperty ComplexProperty { get; }
        public ExcelToCodeList List { get; }
        public ExcelToCodeWhen When { get; }
        public ExcelToCodeThen Then { get; }
        public LogState Log { get; }
        public ExcelState Excel { get; }
        public CodeState Code { get; }
        public ICodeNameToExcelNameConverter Converter { get; }

        public ExcelToCodeState(ICodeNameToExcelNameConverter converter)
        {
            Converter = converter ?? throw new ArgumentNullException(nameof(converter));
            Log = new LogState();
            Code = new CodeState();
            Excel = new ExcelState();

            Given = new ExcelToCodeGiven(this);
            Table = new ExcelToCodeTable(this);
            SimpleProperty = new ExcelToCodeSimpleProperty(this);
            ComplexProperty = new ExcelToCodeComplexProperty(this);
            List = new ExcelToCodeList(this);
            When = new ExcelToCodeWhen(this);
            Then = new ExcelToCodeThen(this);
        }

        public void Initialise(ITabularPage worksheet)
        {
            Excel.Initialise(worksheet);
            Code.Initialise();
            Log.Initialise();
        }

    }
}
