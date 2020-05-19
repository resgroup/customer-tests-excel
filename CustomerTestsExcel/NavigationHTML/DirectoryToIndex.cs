namespace CustomerTestsExcel.NavigationHTML
{
    public struct DirectoryToIndex
    {
        public string FullDirectory { get; }
        public string ParentDirectory { get; }
        public string Description { get; }
        public int NestDepth { get; }
        public DirectoryToIndex(string fullDirectory, string parentDirectory, string description, int nestDepth)
        {
            this.FullDirectory = fullDirectory;
            this.ParentDirectory = parentDirectory;
            this.Description = description;
            this.NestDepth = nestDepth;
        }
    }
}
