using CustomerTestsExcel.Indentation;
using System;

namespace CustomerTestsExcel.ExcelToCode
{
    public class CodeState
    {
        public AutoIndentingStringBuilder code;

        public CodeState()
        {
        }

        internal void Initialise()
        {
            code = new AutoIndentingStringBuilder("    ");
        }

        public string GeneratedCode =>
            code.ToString();

        public void Add(string lineOfCSharpCode) =>
            code.AppendLine(lineOfCSharpCode);

        public TidyUp OutputAndOpenAutoClosingBracket(string lineOfCSharpCodeWithoutBracket)
        {
            code.AppendLine($"{lineOfCSharpCodeWithoutBracket}(");
            Indent();
            return new TidyUp(CloseBracketAndOutdent);
        }

        public void BlankLine() =>
            Add("");

        public void OpenCurlyBracket() =>
            Add("{");

        public void CloseCurlyBracket() =>
            Add("}");

        public TidyUp Scope() =>
            AutoCloseCurlyBracket();

        public TidyUp AutoCloseCurlyBracket() =>
            new TidyUp(OpenCurlyBracket, CloseCurlyBracket);

        public void Indent() =>
            code.Indent();

        public void Outdent() =>
            code.Outdent();

        public TidyUp AutoCloseIndent() =>
            new TidyUp(Indent, Outdent);

        public void OpenBracketAndIndent()
        {
            Add("(");
            code.Indent();
        }

        public void CloseBracketAndOutdent()
        {
            code.Outdent();
            Add(")");
        }

        public TidyUp AutoCloseBracketAndIndent() =>
            new TidyUp(OpenBracketAndIndent, CloseBracketAndOutdent);

        public void AddError(string message) =>
            Add($"// {message}");
    }
}
