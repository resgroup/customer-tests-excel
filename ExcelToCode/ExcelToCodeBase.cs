using CustomerTestsExcel.Indentation;
using System;
using System.Collections.Generic;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCodeBase
    {
        public LogState logState;
        protected ExcelState excelState;
        protected CodeState codeState;
        //protected ITabularPage worksheet;
        protected readonly ICodeNameToExcelNameConverter converter;
        //protected uint row;
        //protected uint column;
        protected string _sutName;
        //protected AutoIndentingStringBuilder code;

        //protected readonly List<string> errors;
        //public IReadOnlyList<string> Errors => errors;

        //protected readonly List<string> issuesPreventingRoundTrip;
        //public IReadOnlyList<string> IssuesPreventingRoundTrip => issuesPreventingRoundTrip;

        //protected readonly List<string> warnings;
        //public IReadOnlyList<string> Warnings => warnings;

        //protected readonly List<IExcelToCodeVisitor> visitors;

        public ExcelToCodeBase(ICodeNameToExcelNameConverter converter)
        {
            this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
            logState = new LogState();
            codeState = new CodeState(converter);
            excelState = new ExcelState();
            //errors = new List<string>();
            //issuesPreventingRoundTrip = new List<string>();
            //warnings = new List<string>();
            //visitors = new List<IExcelToCodeVisitor>();
        }

        //public void AddVisitor(IExcelToCodeVisitor visitor) =>
        //    visitors.Add(visitor);

        protected void Output(string lineOfCSharpCode) =>
            codeState.code.AppendLine(lineOfCSharpCode);

        //protected TidyUp OutputAndOpenAutoClosingBracket(string lineOfCSharpCodeWithoutBracket)
        //{
        //    code.AppendLine($"{lineOfCSharpCodeWithoutBracket}(");
        //    Indent();
        //    return new TidyUp(CloseBracketAndOutdent);
        //}

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

        protected bool RowToColumnIsEmpty(uint column) =>
            excelState.RowToColumnIsEmpty(column);
        //{
        //    for (uint columnToCheck = 1; columnToCheck <= column; columnToCheck++)
        //    {
        //        if (!string.IsNullOrWhiteSpace(Cell(row, columnToCheck))) return false;
        //    }

        //    return true;
        //}

        protected bool RowToCurrentColumnIsEmpty() =>
            excelState.RowToCurrentColumnIsEmpty();

        protected bool AllColumnsAreEmpty() =>
            excelState.AllColumnsAreEmpty();

        protected bool AnyPrecedingColumnHasAValue() =>
            excelState.AnyPrecedingColumnHasAValue();

        protected bool AnyFollowingColumnHasAValue(int rowOffset = 0) =>
            excelState.AnyFollowingColumnHasAValue(rowOffset);

        protected void ExcelMoveUp(uint by = 1) =>
            excelState.row -= by;

        protected void ExcelMoveDown(uint by = 1) =>
            excelState.row += by;

        protected void ExcelMoveDownToToken(string token) =>
            excelState.ExcelMoveDownToToken(token);
       
        protected void ExcelMoveRight(uint by = 1) =>
            excelState.column += by;

        protected void ExcelMoveLeft(uint by = 1) =>
            excelState.column -= by;

        protected uint? FindTokenInCurrentRowFromCurrentColumn(string token) =>
            excelState.FindTokenInCurrentRowFromCurrentColumn(token);

        protected void OpenCurlyBracket() =>
            Output("{");

        protected void CloseCurlyBracket() =>
            Output("}");

        protected TidyUp Scope() =>
            AutoCloseCurlyBracket();

        protected TidyUp AutoCloseCurlyBracket() =>
            new TidyUp(OpenCurlyBracket, CloseCurlyBracket);

        protected void Indent() =>
            codeState.Indent();

        protected void Outdent() =>
            codeState.Outdent();

        protected TidyUp AutoCloseIndent() =>
            new TidyUp(Indent, Outdent);

        protected void OpenBracketAndIndent()
        {
            Output("(");
            codeState.Indent();
        }

        protected void CloseBracketAndOutdent()
        {
            codeState.Outdent();
            Output(")");
        }

        protected TidyUp AutoCloseBracketAndIndent() =>
            new TidyUp(OpenBracketAndIndent, CloseBracketAndOutdent);

        protected TidyUp SavePosition() =>
            excelState.SavePosition();

        protected TidyUp AutoRestoreExcelMoveDown(uint by = 1)
        {
            ExcelMoveDown(by);
            return new TidyUp(() => ExcelMoveUp(by));
        }

        protected TidyUp AutoRestoreExcelMoveRight(uint by = 1)
        {
            ExcelMoveRight(by);
            return new TidyUp(() => ExcelMoveLeft(by));
        }

        protected TidyUp AutoRestoreExcelMoveDownRight(uint downBy = 1, uint rightBy = 1)
        {
            ExcelMoveRight(rightBy);
            ExcelMoveDown(downBy);

            return new TidyUp(() =>
            {
                ExcelMoveLeft(rightBy);
                ExcelMoveUp(downBy);
            });
        }

        protected uint GetLastRow() =>
            excelState.GetLastRow();

        protected uint GetLastColumn() =>
            excelState.GetLastColumn();

        protected string PeekAbove(uint by = 1) =>
            excelState.PeekAbove(by);

        protected string PeekBelow(uint by = 1) =>
            excelState.PeekBelow(by);

        protected string PeekRight(uint by = 1) =>
            excelState.PeekRight(by);

        protected string PeekBelowRight(uint belowBy = 1, uint rightBy = 1) =>
            excelState.PeekBelowRight(belowBy, rightBy);

        protected string CurrentCell() =>
            excelState.CurrentCell();

        protected object CurrentCellRaw() =>
            excelState.CurrentCellRaw();

        protected string Cell(uint row, uint column) =>
            excelState.Cell(row, column);

        protected object CellRaw(uint row, uint column) =>
            excelState.CellRaw(row, column);

        protected string CellReferenceA1Style() =>
            excelState.CellReferenceA1Style();

        protected string CellReferenceA1Style(uint row, uint column) =>
            excelState.CellReferenceA1Style(row, column);

        protected string ColumnReferenceA1Style() => 
            excelState.ColumnReferenceA1Style();

        protected string ColumnReferenceA1Style(uint column) =>
            excelState.ColumnReferenceA1Style(column);

        protected void AddError(string message)
        {
            // this will appear at the relevant point in the generated code
            Output($"// {message}");

            // this can be used elsewhere, such as in the console output of the test generation
            logState.errors.Add(message);
        }
    }
}
