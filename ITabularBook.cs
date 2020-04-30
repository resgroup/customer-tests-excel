using System;

namespace CustomerTestsExcel
{
    public interface ITabularBook : IDisposable
    {
        int NumberOfPages { get; }

        string Filename { get; set; }
        string[] GetPageNames();
        ITabularPage GetPage(string page);
        ITabularPage AddPageBefore(int page);
        void SaveAs(string filename);
        ITabularPage GetPage(int page);
    }
}