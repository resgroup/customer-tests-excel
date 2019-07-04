using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.Test
{
    public class TestTabularBook : ITabularBook
    {
        public readonly List<TestTabularPage> Pages;

        public TestTabularBook()
        {
            Pages = new List<TestTabularPage>();
        }

        public int NumberOfPages => 
            Pages.Count;

        public ITabularPage AddPageBefore(int page)
        {
            var tabularPage = new TestTabularPage();

            Pages.Insert(page, tabularPage);

            return tabularPage;
        }

        public ITabularPage GetPage(string page) =>
            Pages.FirstOrDefault(p => p.Name == page);

        public ITabularPage GetPage(int page) =>
            Pages[page];

        public string[] GetPageNames() =>
            Pages.Select(page => page.Name).ToArray();

        public void SaveAs(string filename)
        {
            // don't actually save in test
        }

        public void Dispose()
        {
            // nothing to dispose of
        }
    }
}