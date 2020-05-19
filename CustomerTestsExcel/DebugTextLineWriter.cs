using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CustomerTestsExcel
{
    public class DebugTextLineWriter : ITextLineWriter
    {
        public void WriteLine(string text)
        {
            Debug.WriteLine(text);
        }

        public void StartLine(string text)
        {
            Debug.Write(text);
        }

        public void ContinueLine(string text)
        {
            Debug.Write(text);
        }

        public void EndLine(string text)
        {
            Debug.WriteLine(text);
        }
    }
}
