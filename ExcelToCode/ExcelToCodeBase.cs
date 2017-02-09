using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Specification.ExcelToCode
{
    public class ExcelToCodeBase
    {
        protected ITabularPage _worksheet;
        protected readonly ICodeNameToExcelNameConverter _converter;
        protected uint _row;
        protected uint _column;
        protected string _sutName;
        protected AutoIndentingStringBuilder _code;

        public ExcelToCodeBase(ICodeNameToExcelNameConverter converter)
        {
            if (converter == null) throw new ArgumentNullException(nameof(converter));

            _converter = converter;
        }

        protected void Output(string lineOfCSharpCode)
        {
            _code.AppendLine(lineOfCSharpCode);
        }

        protected void Output()
        {
            _code.AppendLine();
        }

        protected string UnIndex(string propertyName)
        {
            propertyName = propertyName.Replace("s(", "(");

            if (propertyName.IndexOf("(") == -1)
            {
                return propertyName;
            }
            else
            {
                return propertyName.Remove(propertyName.IndexOf("("));
            }
        }

        protected string GetIndex(string variableName)
        {
            var splut = variableName.Split(new char[] { '(', ')' });
            return (splut.Count() > 1) ? splut[1] : "";
        }

        protected string SUTClassName()
        {
            if (_sutName == null) throw new Exception("Trying to read _sutName before it has been set");

            return _sutName;
        }

        protected static string CreationalCSharpVariableName(string cSharpVariableName)
        {
            return cSharpVariableName + "CreationalProperties";
        }

        protected string CSharpSUTSpecificationSpecificClassName()
        {
            return _converter.ExcelClassNameToCodeName(SUTClassName());
        }

        protected string CSharpSUTVariableName()
        {
            return VariableCase(SUTClassName());
        }

        protected string VariableCase(string camelCase)
        {
            // it is assumed to already be in camel case, this means making the first letter lower case
            // this isn't a two way process (eg the conversion process doesn't care what the string is) so this isn't in the _namer
            return string.IsNullOrWhiteSpace(camelCase) ? "" : char.ToLower(camelCase[0]) + camelCase.Substring(1);
        }

        protected static string LeadingComma(int index)
        {
            return (index == 0) ? " " : ",";
        }

        protected bool RowToCurrentColumnIsEmpty()
        {
            for (uint column = 1; column <= _column; column++)
            {
                if (!string.IsNullOrWhiteSpace(Cell(_row, column))) return false;
            }

            return true;
        }

        protected bool AnyPrecedingColumnHasAValue()
        {
            for (uint column = 1; column < _column; column++)
            {
                if (!string.IsNullOrWhiteSpace(Cell(_row, column))) return true;
            }

            return false;
        }

        protected bool AnyFollowingColumnHasAValue(int rowOffset = 0)
        {
            for (uint column = _column + 1; column <
                GetLastColumn(); column++)
            {
                if (!string.IsNullOrWhiteSpace(Cell((uint)(_row + rowOffset), column))) return true;
            }

            return false;
        }

        protected bool AllColumnsAreEmpty()
        {
            for (uint column = 1; column <= GetLastColumn(); column++)
            {
                if (!string.IsNullOrWhiteSpace(Cell(_row, column))) return false;
            }

            return true;
        }

        protected void MoveDown(uint by = 1)
        {
            _row += by;
        }

        protected void MoveDownToToken(string token)
        {
            while (CurrentCell() != token)
            {
                if (_row > GetLastRow()) throw new ExcelToCodeException(string.Format("Cannot find token {0} in column {1}, reached last row ({2})", token, _column, _row));
                MoveDown();
            }
        }

        protected void MoveUp()
        {
            _row--;
        }

        protected void MoveRight(uint by = 1)
        {
            _column += by;
        }

        protected void MoveLeft(uint by = 1)
        {
            _column -= by;
        }

        protected uint? FindTokenInCurrentRowFromCurrentColumn(string token)
        {
            uint column = _column;
            while (Cell(_row, column) != token)
            {
                if (column > GetLastColumn()) return null;
                column++;
            }

            return column;
        }

        protected TidyUp SavePosition()
        {
            uint row = _row;
            uint column = _column;

            return new TidyUp(() => { _row = row; _column = column; });
        }

        protected TidyUp AutoCloseIndent(uint by = 1)
        {
            Indent();
            return new TidyUp(() => UnIndent(by));
        }

        protected void Indent(uint by = 1)
        {
            _column += by;
        }

        protected void UnIndent(uint by = 1)
        {
            _column -= by;
            if (_column <= 0)
                throw new Exception("UnIndent without matching Indent");
        }

        protected uint GetLastRow()
        {
            return _worksheet.MaxRow;
        }

        protected uint GetLastColumn()
        {
            return _worksheet.MaxColumn + 20; // maxcolumn seems to underreport the amount of columns that there are ...
        }

        protected string PeekAbove(uint by = 1)
        {
            return Cell(_row - by, _column);
        }

        protected string PeekBelow(uint by = 1)
        {
            return Cell(_row + by, _column);
        }

        protected string CurrentCell()
        {
            return Cell(_row, _column);
        }

        protected object CurrentCellRaw()
        {
            return CellRaw(_row, _column);
        }

        protected string Cell(uint row, uint column)
        {
            var value = _worksheet.GetCell(row, column).Value;

            return (value == null) ? "" : value.ToString();
        }

        protected object CellRaw(uint row, uint column)
        {
            return _worksheet.GetCell(row, column).Value;
        }
    }
}
