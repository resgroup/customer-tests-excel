using System;
using System.IO;

namespace RES.Specification
{
    public interface ITabularLibrary
    {
        string DefaultExtension { get; }

        ITabularBook NewBook(string fileName);
        ITabularBook NewBook();
        ITabularBook OpenBook(Stream workbookFile);
    }
}