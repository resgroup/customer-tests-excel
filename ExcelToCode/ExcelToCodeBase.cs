using CustomerTestsExcel.Indentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCodeBase
    {
        protected ITabularPage worksheet;
        protected readonly ICodeNameToExcelNameConverter converter;
        protected uint row;
        protected uint column;
        protected string _sutName;
        protected AutoIndentingStringBuilder code;

        public ExcelToCodeBase(ICodeNameToExcelNameConverter converter)
        {
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        protected void Output(string lineOfCSharpCode) =>
            code.AppendLine(lineOfCSharpCode);

        protected void Output() =>
            Output("");

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

        protected string CSharpSUTSpecificationSpecificClassName() =>
            converter.ExcelClassNameToCodeName(SUTClassName());

        protected string CSharpSUTVariableName() =>
            VariableCase(SUTClassName());

        protected string VariableCase(string camelCase) =>
            // it is assumed to already be in camel case, this means making the first letter lower case
            // this isn't a two way process (eg the conversion process doesn't care what the string is) so this isn't in the _namer
            string.IsNullOrWhiteSpace(camelCase) ? "" : char.ToLower(camelCase[0]) + camelCase.Substring(1);

        protected static string LeadingComma(int index) =>
            (index == 0) ? " " : ",";

        protected bool RowToColumnIsEmpty(uint column)
        {
            for (uint columnToCheck = 1; columnToCheck <= column; columnToCheck++)
            {
                if (!string.IsNullOrWhiteSpace(Cell(row, columnToCheck))) return false;
            }

            return true;
        }

        protected bool RowToCurrentColumnIsEmpty() =>
            RowToColumnIsEmpty(column);

        protected bool AllColumnsAreEmpty() =>
            RowToColumnIsEmpty(GetLastColumn());

        protected bool AnyPrecedingColumnHasAValue() =>
            !RowToColumnIsEmpty(column - 1);

        protected bool AnyFollowingColumnHasAValue(int rowOffset = 0)
        {
            for (uint column = this.column + 1; column <
                GetLastColumn(); column++)
            {
                if (!string.IsNullOrWhiteSpace(Cell((uint)(row + rowOffset), column))) return true;
            }

            return false;
        }

        protected void ExcelMoveUp(uint by = 1) =>
            row -= by;

        protected void ExcelMoveDown(uint by = 1) =>
            row += by;

        protected void ExcelMoveDownToToken(string token)
        {
            while (CurrentCell() != token)
            {
                if (row > GetLastRow()) throw new ExcelToCodeException(string.Format("Cannot find token {0} in column {1}, reached last row ({2})", token, column, row));
                ExcelMoveDown();
            }
        }

        protected void ExcelMoveRight(uint by = 1) =>
            column += by;

        protected void ExcelMoveLeft(uint by = 1) =>
            column -= by;

        protected uint? FindTokenInCurrentRowFromCurrentColumn(string token)
        {
            uint column = this.column;
            while (Cell(row, column) != token)
            {
                if (column > GetLastColumn()) return null;
                column++;
            }

            return column;
        }

        protected void OpenCurlyBracket() =>
            Output("{");

        protected void CloseCurlyBracket() =>
            Output("}");

        protected TidyUp Scope() =>
            AutoCloseCurlyBracket();

        protected TidyUp AutoCloseCurlyBracket() =>
            new TidyUp(OpenCurlyBracket, CloseCurlyBracket);

        protected void OpenBracketAndIndent()
        {
            Output("(");
            code.Indent();
        }

        protected void CloseBracketAndOutdent()
        {
            code.Outdent();
            Output(")");
        }

        protected TidyUp AutoCloseBracketAndIndent() =>
            new TidyUp(OpenBracketAndIndent, CloseBracketAndOutdent);

        protected TidyUp SavePosition()
        {
            uint savedRow = this.row;
            uint savedColumn = column;

            return new TidyUp(() => { row = savedRow; column = savedColumn; });
        }

        protected TidyUp AutoRestoreExcelMoveDown(uint by = 1)
        {
            ExcelMoveDown(by);
            return new TidyUp(() => ExcelMoveUp(by));
        }

        protected TidyUp AutoRestoreExcelMoveRight(uint by = 1)
        {
            ExcelIndent(by);
            return new TidyUp(() => ExcelUnIndent(by));
        }

        protected TidyUp AutoRestoreExcelMoveDownRight(uint downBy = 1, uint rightBy = 1)
        {
            ExcelIndent(rightBy);
            ExcelMoveDown(downBy);

            return new TidyUp(() => 
            {
                ExcelUnIndent(rightBy);
                ExcelMoveUp(downBy);
            });
        }

        protected void ExcelIndent(uint by = 1) =>
            column += by;

        protected void ExcelUnIndent(uint by = 1)
        {
            column -= by;
            if (column <= 0)
                throw new Exception("UnIndent without matching Indent");
        }

        protected uint GetLastRow() =>
            worksheet.MaxRow;

        protected uint GetLastColumn() =>
            worksheet.MaxColumn + 20; // maxcolumn seems to underreport the amount of columns that there are ...

        protected string PeekAbove(uint by = 1) =>
            Cell(row - by, column);

        protected string PeekBelow(uint by = 1) =>
            Cell(row + by, column);

        protected string PeekRight(uint by = 1) =>
            Cell(row, column + by);

        protected string PeekBelowRight(uint belowBy = 1, uint rightBy = 1) =>
            Cell(row + belowBy, column + rightBy);

        protected string CurrentCell() =>
            Cell(row, column);

        protected object CurrentCellRaw() =>
            CellRaw(row, column);

        protected string Cell(uint row, uint column)
        {
            var value = worksheet.GetCell(row, column).Value;

            return (value == null) ? "" : value.ToString();
        }

        protected object CellRaw(uint row, uint column) =>
            worksheet.GetCell(row, column).Value;

        protected string CellReferenceA1Style() =>
            CellReferenceA1Style(row, column);

        protected string CellReferenceA1Style(uint row, uint column) =>
            $"{ColumnReferenceA1Style(column)}{row}";

        protected string ColumnReferenceA1Style()
            => ColumnReferenceA1Style(column);

        protected string ColumnReferenceA1Style(uint column)
        {
            const uint A = 65;
            const uint NUMBER_OF_LETTERS_IN_ALPHABET = 26;
            uint dividend = column;
            string columnName = String.Empty;
            uint modulo;

            // this works because the int representation of all capital letters starts at 65, is continuous and in alphabetical order
            while (dividend > 0)
            {
                modulo = (dividend - 1) % NUMBER_OF_LETTERS_IN_ALPHABET;
                columnName = Convert.ToChar(A + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / NUMBER_OF_LETTERS_IN_ALPHABET;
            }

            return columnName;
        }
    }
}
