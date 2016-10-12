using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AddNavigationToSpecificationHTMLOutputFiles
{
    public class NavigationHTMLFormatter : INavigationHTMLFormatter
    {
        protected StringBuilder _indexHTML;

        public NavigationHTMLFormatter()
        {
            _indexHTML = new StringBuilder();
        }

        public string HTML()
        {
            return _indexHTML.ToString();
        }

        public void StartIndex(string description, int nestDepth)
        {
            _indexHTML.Clear();
            _indexHTML.AppendLine(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN\""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">");
            _indexHTML.AppendLine(@"<html>");
            _indexHTML.AppendLine(@"<head>");
            _indexHTML.AppendLine(@"<title>");
            _indexHTML.AppendLine(description);
            _indexHTML.AppendLine(@"</title>");
            _indexHTML.AppendLine(@"</head>");
            _indexHTML.Append(@"<link rel='stylesheet' href='"); // stylesheet should be in the directory above the root directory for tests
            for (int i = 0; i <= nestDepth; i++) _indexHTML.Append(@"..\");
            _indexHTML.AppendLine(@"SpecificationIndex.css'>");
            _indexHTML.AppendLine(@"<body>");
            _indexHTML.AppendLine(@"<h1>");
            _indexHTML.AppendLine(description);
            _indexHTML.AppendLine(@"</h1>");
        }

        public void StartParent()
        {
            _indexHTML.AppendLine(@"<div class='parent'>");
            _indexHTML.AppendLine("<h2>Parent Tests</h2>");
            _indexHTML.AppendLine(@"<ul class='parentLinks'>");
        }

        public void EndParent()
        {
            _indexHTML.AppendLine(@"</ul>");
            _indexHTML.AppendLine("</div>");
        }

        public void StartTests()
        {
            _indexHTML.AppendLine(@"<div class='tests'>");
            _indexHTML.AppendLine("<h2>Tests</h2>");
            _indexHTML.AppendLine(@"<ul class='testLinks'>");
        }

        public void EndTests()
        {
            _indexHTML.AppendLine(@"</ul>");
            _indexHTML.AppendLine("</div>");
        }

        public void StartChildren()
        {
            _indexHTML.AppendLine(@"<div class='children'>");
            _indexHTML.AppendLine("<h2>Child Tests</h2>");
            _indexHTML.AppendLine(@"<ul class='childLinks'>");
        }

        public void EndChildren()
        {
            _indexHTML.AppendLine(@"</ul>");
            _indexHTML.AppendLine("</div>");
        }

        public void EndIndex()
        {
            _indexHTML.AppendLine(@"</body>");
            _indexHTML.AppendLine(@"</HTML>");
        }

        public void AddLink(string url, string description, string cssClass)
        {
            _indexHTML.Append(@"<li><a href='");
            _indexHTML.Append(url);
            _indexHTML.Append("' class='");
            _indexHTML.Append(cssClass);
            _indexHTML.Append(@"'>");
            _indexHTML.Append(description);
            _indexHTML.Append(@"</a></li>");
        }
    }
}
