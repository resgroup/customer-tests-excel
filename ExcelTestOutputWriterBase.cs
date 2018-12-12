using CustomerTestsExcel.Indentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public class ExcelTestOutputWriterBase
    {
        protected readonly ITabularLibrary _excel;
        protected readonly ICodeNameToExcelNameConverter _namer;
        protected ITabularBook _workbook;
        protected ITabularPage _worksheet;
        protected uint _row;
        protected uint _column;

        public ExcelTestOutputWriterBase(ITabularLibrary excel, ICodeNameToExcelNameConverter namer)
        {
            if (excel == null) throw new ArgumentNullException(nameof(excel));
            if (namer == null) throw new ArgumentNullException(nameof(namer));

            _excel = excel;
            _namer = namer;
        }

        protected void SetCell(uint row, uint column, string cSharpValue, object excelValue)
        {
            var cell = _worksheet.GetCell(row, column);
            if (cell.Value == null || (cell.Value.Equals(excelValue) == false && cell.Value.ToString() != cSharpValue))
            {
                if (!cell.IsFormula)
                {
                    _worksheet.SetCell(row, column, excelValue);
                }
                else
                {
                    AddSkippedCellWarning(row, column, excelValue);
                }
            }
        }

        protected void SetCell(uint row, uint column, object value)
        {
            SetCell(row, column, "", value);
        }

        protected void ClearSkippedCellWarnings()
        {
            _worksheet.SetCell(1, 3, "");
        }

        void AddSkippedCellWarning(uint row, uint column, object value)
        {
            // put all warnings in cell 1, 3, which is reserved for this purpose
            _worksheet.SetCell(1, 3, $"{_worksheet.GetCell(1, 3).Value} Skipped updating Cell R{row}C{column} to '{value}' as it has a formula in it. Please fix this value by hand, or remove the formula and re run the test.\r\n");
        }

        protected void SetCell(object value)
        {
            SetCell(_row, _column, "", value);
        }

        protected void SetCell(string cSharpValue, object value)
        {
            SetCell(_row, _column, cSharpValue, value);
        }

        protected void SetPosition(uint row, uint column)
        {
            _row = row;
            _column = column;
        }

        protected void SetColumn(uint column)
        {
            SetPosition(_row, column);
        }

        protected void MoveToNextRow()
        {
            _row++;
        }

        protected void Indent(uint by = 1)
        {
            _column += by;
        }

        protected void UnIndent(uint by = 1)
        {
            _column -= by;
        }

        protected TidyUp SavePosition()
        {
            uint savedColumn = _column;
            uint savedRow = _row;

            return new TidyUp(() => SetPosition(savedRow, savedColumn));
        }

    }
}
