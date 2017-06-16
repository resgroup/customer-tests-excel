using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomerTestsExcel.NavigationHTML
{
    public class NavigationHTMLFormatter : INavigationHTMLFormatter
    {
        private StringBuilder Builder { get; } = new StringBuilder();

        public string HTML() => Builder.ToString();

        public void StartIndex(string description, int nestDepth)
        {
            Builder.Clear();
            Builder.AppendLine(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN\""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">");
            Builder.AppendLine(@"<html>");
            Builder.AppendLine(@"<head>");
            Builder.AppendLine(@"<title>");
            Builder.AppendLine(description);
            Builder.AppendLine(@"</title>");
            Builder.AppendLine(@"</head>");
            Builder.Append(@"<link rel='stylesheet' href='"); // stylesheet should be in the directory above the root directory for tests
            for (int i = 0; i <= nestDepth; i++)
            {
                Builder.Append(@"..\");
            }
            Builder.AppendLine(@"SpecificationIndex.css'>");
            Builder.AppendLine(@"<body>");
            Builder.AppendLine(@"<h1>");
            Builder.AppendLine(description);
            Builder.AppendLine(@"</h1>");
        }

        public void StartParent()
        {
            Builder.AppendLine(@"<div class='parent'>");
            Builder.AppendLine("<h2>Parent Tests</h2>");
            Builder.AppendLine(@"<ul class='parentLinks'>");
        }

        public void EndParent()
        {
            Builder.AppendLine(@"</ul>");
            Builder.AppendLine("</div>");
        }

        public void StartTests()
        {
            Builder.AppendLine(@"<div class='tests'>");
            Builder.AppendLine("<h2>Tests</h2>");
            Builder.AppendLine(@"<ul class='testLinks'>");
        }

        public void EndTests()
        {
            Builder.AppendLine(@"</ul>");
            Builder.AppendLine("</div>");
        }

        public void StartChildren()
        {
            Builder.AppendLine(@"<div class='children'>");
            Builder.AppendLine("<h2>Child Tests</h2>");
            Builder.AppendLine(@"<ul class='childLinks'>");
        }

        public void EndChildren()
        {
            Builder.AppendLine(@"</ul>");
            Builder.AppendLine("</div>");
        }

        public void EndIndex()
        {
            Builder.AppendLine(@"</body>");
            Builder.AppendLine(@"</HTML>");
        }

        public void AddLink(string url, string description, string cssClass)
        {
            Builder.Append(@"<li><a href='");
            Builder.Append(url);
            Builder.Append("' class='");
            Builder.Append(cssClass);
            Builder.Append(@"'>");
            Builder.Append(description);
            Builder.Append(@"</a></li>");
        }
    }
}
