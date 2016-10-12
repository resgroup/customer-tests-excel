using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AddNavigationToSpecificationHTMLOutputFiles
{
    public interface INavigationHTMLFormatter
    {
        void StartIndex(string description, int nestDepth);
        void EndIndex();

        void StartParent();
        void EndParent();
        
        void StartTests();
        void EndTests();
        
        void StartChildren();
        void EndChildren();
        
        void AddLink(string url, string description, string cssClass);
        
        string HTML();
    }
}
