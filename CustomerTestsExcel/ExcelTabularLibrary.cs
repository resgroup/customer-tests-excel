using System;
using System.IO;

namespace CustomerTestsExcel
{
    public class ExcelTabularLibrary : ITabularLibrary
    {
        public string DefaultExtension => "xlsx";

        public ITabularBook NewBook() => new ExcelTabularBook();

        public ITabularBook NewBook(string filename) => new ExcelTabularBook(filename);

        public ITabularBook OpenBook(Stream stream) => new ExcelTabularBook(stream);
    }
}