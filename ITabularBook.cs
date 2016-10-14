using System;

namespace RES.Specification
{
    public interface ITabularBook : IDisposable
    {
        int NumberOfPages { get; }

        string[] GetPageNames();
        ITabularPage GetPage(string page);
        ITabularPage AddPageBefore(int page);
        void SaveAs(string filename);
        ITabularPage GetPage(int page);
    }
}