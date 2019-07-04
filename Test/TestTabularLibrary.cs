using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerTestsExcel.Test
{
    public class TestTabularLibrary : ITabularLibrary
    {
        public readonly List<TestTabularBook> Books;

        public TestTabularLibrary()
        {
            Books = new List<TestTabularBook>();
        }

        public string DefaultExtension =>
            "";

        public ITabularBook NewBook(string fileName)
        {
            var book = new TestTabularBook();

            Books.Add(book);

            return book;
        }

        public ITabularBook NewBook()
        {
            var book = new TestTabularBook();

            Books.Add(book);

            return book;
        }

        public ITabularBook OpenBook(Stream workbookFile)
        {
            var book = new TestTabularBook();

            Books.Add(book);

            return book;
        }

    }
}
