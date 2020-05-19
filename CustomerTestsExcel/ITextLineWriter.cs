using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel
{
    public interface ITextLineWriter
    {
        void WriteLine(string text);

        void StartLine(string text);

        void ContinueLine(string text);

        void EndLine(string text);
    }
}
