namespace RES.Specification
{
    public interface ITabularBook
    {
        object GetPageNames();
        ITabularPage GetPage(string specificationFriendlyName);
        ITabularPage AddPageBefore(int v);
        void SaveAs(string v);
    }
}