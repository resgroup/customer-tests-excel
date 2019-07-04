using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public class StringBuilderTextLineWriter : ITextLineWriter
    {
        protected readonly StringBuilder stringBuilder;
        public StringBuilder StringBuilder { get { return stringBuilder; } }

        public StringBuilderTextLineWriter()
        {
            stringBuilder = new StringBuilder();
        }

        public void WriteLine(string text)
        {
            stringBuilder.AppendLine(text);
        }

        public void StartLine(string text)
        {
            stringBuilder.AppendLine(text);
        }

        public void ContinueLine(string text)
        {
            stringBuilder.Append(text);
        }

        public void EndLine(string text)
        {
            stringBuilder.AppendLine(text);
        }
    }
}
