using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Specification
{
    public class IndentingStringBuilder
    {
        StringBuilder _builder = new StringBuilder();
        string _indentString;
        int _indentLevel;
        public IndentingStringBuilder(string indentString)
        {
            _indentLevel = 0;
            _indentString = indentString;
        }

        public void AppendLine()
        {
            _builder.AppendLine();
        }

        public virtual void AppendLine(string line)
        {
            for (int i = 0; i < _indentLevel; i++)
                _builder.Append(_indentString);
            _builder.AppendLine(line);
        }

        public TidyUp AutoCloseIndent()
        {
            Indent();
            return new TidyUp(() => Outdent());
        }

        public void Indent() { _indentLevel++; }
        public void Outdent() { _indentLevel--; }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }

    public class AutoIndentingStringBuilder : IndentingStringBuilder
    {
        public AutoIndentingStringBuilder(string indentString) : base(indentString) { }

        public override void AppendLine(string line)
        {
            if (line.Contains('}'))
                Outdent();
            base.AppendLine(line);
            if (line.Contains('{'))
                Indent();
        }
    }
}
