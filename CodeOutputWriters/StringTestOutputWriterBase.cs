using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CustomerTestsExcel.CodeOutputWriters
{
    public class StringTestOutputWriterBase
    {
        protected readonly IHumanFriendlyFormatter _formatter;
        protected readonly ITextLineWriter _writer;
        private int _indentLevel;
        private string _indentString;

        public StringTestOutputWriterBase(IHumanFriendlyFormatter formatter, ITextLineWriter writer)
        {
            if (formatter == null) throw new ArgumentNullException("formatter");
            if (writer == null) throw new ArgumentNullException("writer");

            _formatter = formatter;
            _writer = writer;

            SetIndentLevel(0);
        }

        public void WriteLine(string text)
        {
            _writer.WriteLine(_indentString + text);
        }

        public void StartLine(string text)
        {
            _writer.StartLine(_indentString + text);
        }

        public void ContinueLine(string text)
        {
            _writer.ContinueLine(text);
        }

        public void EndLine(string text)
        {
            _writer.EndLine(text);
        }

        protected void Indent()
        {
            SetIndentLevel(_indentLevel + 1);
        }

        protected void Outdent()
        {
            SetIndentLevel(_indentLevel - 1);
        }

        private void SetIndentLevel(int indentLevel)
        {
            if (indentLevel < 0)
            {
                throw new Exception("Indent level cannot go below zero.");
            }

            _indentLevel = indentLevel;
            _indentString = new string('\t', _indentLevel);
        }
    }
}
