using CustomerTestsExcel.Indentation;
using System;
using System.Collections.Generic;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelState
    {
        public ITabularPage worksheet;
        //public readonly ICodeNameToExcelNameConverter converter;
        public uint row;
        public uint column;
        //public string _sutName;
        //public AutoIndentingStringBuilder code;

        //public readonly List<string> errors;
        //public IReadOnlyList<string> Errors => errors;

        //public readonly List<string> issuesPreventingRoundTrip;
        //public IReadOnlyList<string> IssuesPreventingRoundTrip => issuesPreventingRoundTrip;

        //public readonly List<string> warnings;
        //public IReadOnlyList<string> Warnings => warnings;

        //public readonly List<IExcelToCodeVisitor> visitors;

        public ExcelState()//ICodeNameToExcelNameConverter converter)
        {
            //this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
            //code = new AutoIndentingStringBuilder("\t");
            //errors = new List<string>();
            //issuesPreventingRoundTrip = new List<string>();
            //warnings = new List<string>();
            //visitors = new List<IExcelToCodeVisitor>();
        }

        //public void AddVisitor(IExcelToCodeVisitor visitor) =>
        //    visitors.Add(visitor);

        //public void Output(string lineOfCSharpCode) =>
        //    code.AppendLine(lineOfCSharpCode);

        //public TidyUp OutputAndOpenAutoClosingBracket(string lineOfCSharpCodeWithoutBracket)
        //{
        //    code.AppendLine($"{lineOfCSharpCodeWithoutBracket}(");
        //    Indent();
        //    return new TidyUp(CloseBracketAndOutdent);
        //}

        //public void OutputBlankLine() =>
        //    Output("");

        //public string SUTClassName()
        //{
        //    if (_sutName == null) throw new Exception("Trying to read _sutName before it has been set");

        //    return _sutName;
        //}

        //public string CSharpSUTSpecificationSpecificClassName() =>
        //    converter.ExcelClassNameToCodeName(SUTClassName());

        //public string CSharpSUTVariableName() =>
        //    VariableCase(SUTClassName());

        //public string VariableCase(string camelCase) =>
        //    // it is assumed to already be in camel case, this means making the first letter lower case
        //    // this isn't a two way process (eg the conversion process doesn't care what the string is) so this isn't in the _namer
        //    string.IsNullOrWhiteSpace(camelCase) ? "" : char.ToLower(camelCase[0]) + camelCase.Substring(1);

        //public static string LeadingComma(int index) =>
        //    (index == 0) ? " " : ",";

        public bool RowToColumnIsEmpty(uint column)
        {
            for (uint columnToCheck = 1; columnToCheck <= column; columnToCheck++)
            {
                if (!string.IsNullOrWhiteSpace(Cell(row, columnToCheck))) return false;
            }

            return true;
        }

        public bool RowToCurrentColumnIsEmpty() =>
            RowToColumnIsEmpty(column);

        public bool AllColumnsAreEmpty() =>
            RowToColumnIsEmpty(GetLastColumn());

        public bool AnyPrecedingColumnHasAValue() =>
            !RowToColumnIsEmpty(column - 1);

        public bool AnyFollowingColumnHasAValue(int rowOffset = 0)
        {
            for (uint columnToCheck = column + 1; columnToCheck <
                GetLastColumn(); columnToCheck++)
            {
                if (!string.IsNullOrWhiteSpace(Cell((uint)(row + rowOffset), columnToCheck))) return true;
            }

            return false;
        }

        public void ExcelMoveUp(uint by = 1) =>
            row -= by;

        public void ExcelMoveDown(uint by = 1) =>
            row += by;

        public void ExcelMoveDownToToken(string token)
        {
            while (CurrentCell() != token)
            {
                if (row > GetLastRow()) throw new ExcelToCodeException(string.Format("Cannot find token {0} in column {1}, reached last row ({2})", token, column, row));
                ExcelMoveDown();
            }
        }

        public void ExcelMoveRight(uint by = 1) =>
            column += by;

        public void ExcelMoveLeft(uint by = 1) =>
            column -= by;

        public uint? FindTokenInCurrentRowFromCurrentColumn(string token)
        {
            uint columnToCheck = column;
            while (Cell(row, columnToCheck) != token)
            {
                if (columnToCheck > GetLastColumn()) return null;
                columnToCheck++;
            }

            return columnToCheck;
        }

        //public void OpenCurlyBracket() =>
        //    Output("{");

        //public void CloseCurlyBracket() =>
        //    Output("}");

        //public TidyUp Scope() =>
        //    AutoCloseCurlyBracket();

        //public TidyUp AutoCloseCurlyBracket() =>
        //    new TidyUp(OpenCurlyBracket, CloseCurlyBracket);

        //public void Indent() =>
        //    code.Indent();

        //public void Outdent() =>
        //    code.Outdent();

        //public TidyUp AutoCloseIndent() =>
        //    new TidyUp(Indent, Outdent);

        //public void OpenBracketAndIndent()
        //{
        //    Output("(");
        //    code.Indent();
        //}

        //public void CloseBracketAndOutdent()
        //{
        //    code.Outdent();
        //    Output(")");
        //}

        //public TidyUp AutoCloseBracketAndIndent() =>
        //    new TidyUp(OpenBracketAndIndent, CloseBracketAndOutdent);

        public TidyUp SavePosition()
        {
            uint savedRow = this.row;
            uint savedColumn = column;

            return new TidyUp(() => { row = savedRow; column = savedColumn; });
        }

        public TidyUp AutoRestoreExcelMoveDown(uint by = 1)
        {
            ExcelMoveDown(by);
            return new TidyUp(() => ExcelMoveUp(by));
        }

        public TidyUp AutoRestoreExcelMoveRight(uint by = 1)
        {
            ExcelMoveRight(by);
            return new TidyUp(() => ExcelMoveLeft(by));
        }

        public TidyUp AutoRestoreExcelMoveDownRight(uint downBy = 1, uint rightBy = 1)
        {
            ExcelMoveRight(rightBy);
            ExcelMoveDown(downBy);

            return new TidyUp(() =>
            {
                ExcelMoveLeft(rightBy);
                ExcelMoveUp(downBy);
            });
        }

        public uint GetLastRow() =>
            worksheet.MaxRow;

        public uint GetLastColumn() =>
            worksheet.MaxColumn + 20; // maxcolumn seems to underreport the amount of columns that there are ...

        public string PeekAbove(uint by = 1) =>
            Cell(row - by, column);

        public string PeekBelow(uint by = 1) =>
            Cell(row + by, column);

        public string PeekRight(uint by = 1) =>
            Cell(row, column + by);

        public string PeekBelowRight(uint belowBy = 1, uint rightBy = 1) =>
            Cell(row + belowBy, column + rightBy);

        public string CurrentCell() =>
            Cell(row, column);

        public object CurrentCellRaw() =>
            CellRaw(row, column);

        public string Cell(uint row, uint column)
        {
            var value = worksheet.GetCell(row, column).Value;

            return (value == null) ? "" : value.ToString();
        }

        public object CellRaw(uint row, uint column) =>
            worksheet.GetCell(row, column).Value;

        public string CellReferenceA1Style() =>
            CellReferenceA1Style(row, column);

        public string CellReferenceA1Style(uint row, uint column) =>
            $"{ColumnReferenceA1Style(column)}{row}";

        public string ColumnReferenceA1Style()
            => ColumnReferenceA1Style(column);

        public string ColumnReferenceA1Style(uint column)
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

        //public void AddError(string message)
        //{
        //    // this will appear at the relevant point in the generated code
        //    //Output($"// {message}");

        //    // this can be used elsewhere, such as in the console output of the test generation
        //    //errors.Add(message);
        //}
    }
}
