using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public class StringBuilderTextLineWriter : ITextLineWriter
    {
        protected readonly StringBuilder _stringBuilder;
        public StringBuilder StringBuilder { get { return _stringBuilder; } }

        public StringBuilderTextLineWriter()
        {
            _stringBuilder = new StringBuilder();
        }

        public void WriteLine(string text)
        {
            _stringBuilder.AppendLine(text);
        }

        public void StartLine(string text)
        {
            _stringBuilder.AppendLine(text);
        }

        public void ContinueLine(string text)
        {
            _stringBuilder.Append(text);
        }

        public void EndLine(string text)
        {
            _stringBuilder.AppendLine(text);
        }
    }
}
