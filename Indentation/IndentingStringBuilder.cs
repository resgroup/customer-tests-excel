using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Specification.Indentation
{
    public class IndentingStringBuilder
    {
        private readonly StringBuilder _builder = new StringBuilder();
        private string _indentString;
        private int _indentLevel;
        public IndentingStringBuilder(string indentString)
        {
            _indentLevel = 0;
            _indentString = indentString;
        }

        public void Indent() => _indentLevel++;
        public void Outdent() => _indentLevel--;
        public void AppendLine() => _builder.AppendLine();
        public override string ToString() => _builder.ToString();
        public virtual void AppendLine(string line)
        {
            for (int i = 0; i < _indentLevel; i++)
            {
                _builder.Append(_indentString);
            }
            _builder.AppendLine(line);
        }

        public TidyUp AutoCloseIndent()
        {
            Indent();
            return new TidyUp(() => Outdent());
        }
    }
}
