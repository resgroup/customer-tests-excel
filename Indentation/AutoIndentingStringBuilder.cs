using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RES.Specification.Indentation
{
    public class AutoIndentingStringBuilder : IndentingStringBuilder
    {
        public AutoIndentingStringBuilder(string indentString)
            : base(indentString)
        {
        }

        public override void AppendLine(string line)
        {
            if (line.Contains('}'))
            {
                Outdent();
            }
            base.AppendLine(line);
            if (line.Contains('{'))
            {
                Indent();
            }
        }
    }
}
