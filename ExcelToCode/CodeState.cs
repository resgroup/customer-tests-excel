using CustomerTestsExcel.Indentation;
using System;
using System.Collections.Generic;

namespace CustomerTestsExcel.ExcelToCode
{
    public class CodeState
    {
        //protected ITabularPage worksheet;
        protected readonly ICodeNameToExcelNameConverter converter;
        //protected uint row;
        //protected uint column;
        protected string _sutName;
        protected AutoIndentingStringBuilder code;

        //protected readonly List<string> errors;
        //public IReadOnlyList<string> Errors => errors;

        //protected readonly List<string> issuesPreventingRoundTrip;
        //public IReadOnlyList<string> IssuesPreventingRoundTrip => issuesPreventingRoundTrip;

        //protected readonly List<string> warnings;
        //public IReadOnlyList<string> Warnings => warnings;

        //protected readonly List<IExcelToCodeVisitor> visitors;

        public CodeState(ICodeNameToExcelNameConverter converter)
        {
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
            //errors = new List<string>();
            //issuesPreventingRoundTrip = new List<string>();
            //warnings = new List<string>();
            //visitors = new List<IExcelToCodeVisitor>();
        }

        //public void AddVisitor(IExcelToCodeVisitor visitor) =>
        //    visitors.Add(visitor);

        protected void Output(string lineOfCSharpCode) =>
            code.AppendLine(lineOfCSharpCode);

        protected TidyUp OutputAndOpenAutoClosingBracket(string lineOfCSharpCodeWithoutBracket)
        {
            code.AppendLine($"{lineOfCSharpCodeWithoutBracket}(");
            Indent();
            return new TidyUp(CloseBracketAndOutdent);
        }

        protected void OutputBlankLine() =>
            Output("");

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

        //protected bool RowToColumnIsEmpty(uint column)
        //{
        //    for (uint columnToCheck = 1; columnToCheck <= column; columnToCheck++)
        //    {
        //        if (!string.IsNullOrWhiteSpace(Cell(row, columnToCheck))) return false;
        //    }

        //    return true;
        //}

        //protected bool RowToCurrentColumnIsEmpty() =>
        //    RowToColumnIsEmpty(column);

        //protected bool AllColumnsAreEmpty() =>
        //    RowToColumnIsEmpty(GetLastColumn());

        //protected bool AnyPrecedingColumnHasAValue() =>
        //    !RowToColumnIsEmpty(column - 1);

        //protected bool AnyFollowingColumnHasAValue(int rowOffset = 0)
        //{
        //    for (uint columnToCheck = column + 1; columnToCheck <
        //        GetLastColumn(); columnToCheck++)
        //    {
        //        if (!string.IsNullOrWhiteSpace(Cell((uint)(row + rowOffset), columnToCheck))) return true;
        //    }

        //    return false;
        //}

        //protected void ExcelMoveUp(uint by = 1) =>
        //    row -= by;

        //protected void ExcelMoveDown(uint by = 1) =>
        //    row += by;

        //protected void ExcelMoveDownToToken(string token)
        //{
        //    while (CurrentCell() != token)
        //    {
        //        if (row > GetLastRow()) throw new ExcelToCodeException(string.Format("Cannot find token {0} in column {1}, reached last row ({2})", token, column, row));
        //        ExcelMoveDown();
        //    }
        //}

        //protected void ExcelMoveRight(uint by = 1) =>
        //    column += by;

        //protected void ExcelMoveLeft(uint by = 1) =>
        //    column -= by;

        //protected uint? FindTokenInCurrentRowFromCurrentColumn(string token)
        //{
        //    uint columnToCheck = column;
        //    while (Cell(row, columnToCheck) != token)
        //    {
        //        if (columnToCheck > GetLastColumn()) return null;
        //        columnToCheck++;
        //    }

        //    return columnToCheck;
        //}

        protected void OpenCurlyBracket() =>
            Output("{");

        protected void CloseCurlyBracket() =>
            Output("}");

        protected TidyUp Scope() =>
            AutoCloseCurlyBracket();

        protected TidyUp AutoCloseCurlyBracket() =>
            new TidyUp(OpenCurlyBracket, CloseCurlyBracket);

        protected void Indent() =>
            code.Indent();

        protected void Outdent() =>
            code.Outdent();

        protected TidyUp AutoCloseIndent() =>
            new TidyUp(Indent, Outdent);

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

        //protected TidyUp SavePosition()
        //{
        //    uint savedRow = this.row;
        //    uint savedColumn = column;

        //    return new TidyUp(() => { row = savedRow; column = savedColumn; });
        //}

        //protected TidyUp AutoRestoreExcelMoveDown(uint by = 1)
        //{
        //    ExcelMoveDown(by);
        //    return new TidyUp(() => ExcelMoveUp(by));
        //}

        //protected TidyUp AutoRestoreExcelMoveRight(uint by = 1)
        //{
        //    ExcelMoveRight(by);
        //    return new TidyUp(() => ExcelMoveLeft(by));
        //}

        //protected TidyUp AutoRestoreExcelMoveDownRight(uint downBy = 1, uint rightBy = 1)
        //{
        //    ExcelMoveRight(rightBy);
        //    ExcelMoveDown(downBy);

        //    return new TidyUp(() =>
        //    {
        //        ExcelMoveLeft(rightBy);
        //        ExcelMoveUp(downBy);
        //    });
        //}

        //protected uint GetLastRow() =>
        //    worksheet.MaxRow;

        //protected uint GetLastColumn() =>
        //    worksheet.MaxColumn + 20; // maxcolumn seems to underreport the amount of columns that there are ...

        //protected string PeekAbove(uint by = 1) =>
        //    Cell(row - by, column);

        //protected string PeekBelow(uint by = 1) =>
        //    Cell(row + by, column);

        //protected string PeekRight(uint by = 1) =>
        //    Cell(row, column + by);

        //protected string PeekBelowRight(uint belowBy = 1, uint rightBy = 1) =>
        //    Cell(row + belowBy, column + rightBy);

        //protected string CurrentCell() =>
        //    Cell(row, column);

        //protected object CurrentCellRaw() =>
        //    CellRaw(row, column);

        //protected string Cell(uint row, uint column)
        //{
        //    var value = worksheet.GetCell(row, column).Value;

        //    return (value == null) ? "" : value.ToString();
        //}

        //protected object CellRaw(uint row, uint column) =>
        //    worksheet.GetCell(row, column).Value;

        //protected string CellReferenceA1Style() =>
        //    CellReferenceA1Style(row, column);

        //protected string CellReferenceA1Style(uint row, uint column) =>
        //    $"{ColumnReferenceA1Style(column)}{row}";

        //protected string ColumnReferenceA1Style()
        //    => ColumnReferenceA1Style(column);

        //protected string ColumnReferenceA1Style(uint column)
        //{
        //    const uint A = 65;
        //    const uint NUMBER_OF_LETTERS_IN_ALPHABET = 26;
        //    uint dividend = column;
        //    string columnName = String.Empty;
        //    uint modulo;

        //    // this works because the int representation of all capital letters starts at 65, is continuous and in alphabetical order
        //    while (dividend > 0)
        //    {
        //        modulo = (dividend - 1) % NUMBER_OF_LETTERS_IN_ALPHABET;
        //        columnName = Convert.ToChar(A + modulo).ToString() + columnName;
        //        dividend = (dividend - modulo) / NUMBER_OF_LETTERS_IN_ALPHABET;
        //    }

        //    return columnName;
        //}

        protected void AddError(string message)
        {
            // this will appear at the relevant point in the generated code
            Output($"// {message}");

            // this can be used elsewhere, such as in the console output of the test generation
            //errors.Add(message);
        }
    }
}
