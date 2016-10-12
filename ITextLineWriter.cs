using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RES.Specification
{
    public interface ITextLineWriter
    {
        void WriteLine(string text);

        void StartLine(string text);

        void ContinueLine(string text);

        void EndLine(string text);
    }
}
