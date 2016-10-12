namespace RES.Specification
{
    public interface ITabularLibrary
    {
        string DefaultExtension { get; set; }

        ITabularBook NewBook(string fileName);
        ITabularBook NewBook();
    }
}