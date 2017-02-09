using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;

namespace RES.Specification.NavigationHTML
{
    public class AddNavigationToHTMLOutput
    {
        private Queue<DirectoryToIndex> _directories = new Queue<DirectoryToIndex>();
        public IFileSystem FileSystem { get; }
        public INavigationHTMLFormatter Formatter { get; }

        public AddNavigationToHTMLOutput(IFileSystem fileSystem, INavigationHTMLFormatter formatter)
        {
            if (fileSystem == null) throw new ArgumentNullException("fileSystem");
            if (formatter == null) throw new ArgumentNullException("formatter");

            FileSystem = fileSystem;
            Formatter = formatter;
        }

        public void CreateIndexHtmlFiles(string rootDirectory, string rootName)
        {
            if (string.IsNullOrEmpty(rootDirectory)) throw new ArgumentNullException("rootDirectory");

            var directoryToIndex = new DirectoryToIndex(
                fullDirectory: rootDirectory,
                parentDirectory: null,
                description: rootName,
                nestDepth: 0);

            _directories.Enqueue(directoryToIndex);

            while (_directories.Any())
            {
                AddIndex(_directories.Dequeue());
            }
        }

        private void AddIndex(DirectoryToIndex directoryToIndex)
        {
            Formatter.StartIndex(directoryToIndex.Description, directoryToIndex.NestDepth);

            if (string.IsNullOrEmpty(directoryToIndex.ParentDirectory) == false)
            {
                Formatter.StartParent();
                Formatter.AddLink(Index(".."), GetLastDirectoryName(directoryToIndex.ParentDirectory), "parentLink");
                Formatter.EndParent();
            }

            Formatter.StartTests();
            foreach (var filename in FileSystem.Directory.GetFiles(directoryToIndex.FullDirectory, "*.html"))
            {
                if (FileSystem.Path.GetFileName(filename).ToLower() != "index.html")
                {
                    Formatter.AddLink(FileSystem.Path.GetFileName(filename), FileSystem.Path.GetFileNameWithoutExtension(filename), "testLink");
                }
            }
            Formatter.EndTests();

            var subSirectories = FileSystem.Directory.GetDirectories(directoryToIndex.FullDirectory);
            if (subSirectories.Any())
            {
                Formatter.StartChildren();
                foreach (var fullDirectory in FileSystem.Directory.GetDirectories(directoryToIndex.FullDirectory))
                {
                    if (fullDirectory.TrimEnd(FileSystem.Path.DirectorySeparatorChar).ToLower() != directoryToIndex.FullDirectory.TrimEnd(FileSystem.Path.DirectorySeparatorChar).ToLower())
                    {
                        var subDirectory = GetLastDirectoryName(fullDirectory);
                        Formatter.AddLink(Index(subDirectory), subDirectory, "childLink");

                        _directories.Enqueue(new DirectoryToIndex(
                            fullDirectory,
                            parentDirectory: directoryToIndex.FullDirectory,
                            description: subDirectory,
                            nestDepth: directoryToIndex.NestDepth + 1));
                    }
                }
                Formatter.EndChildren();
            }
            Formatter.EndIndex();

            SaveIndex(Index(directoryToIndex.FullDirectory));
        }

        private string GetLastDirectoryName(string parentDirectory)
        {
            return FileSystem.Path.GetFileName(parentDirectory.TrimEnd(FileSystem.Path.DirectorySeparatorChar));
        }

        private void SaveIndex(string filename)
        {
            FileSystem.File.WriteAllText(filename, Formatter.HTML());
        }

        private string Index(string directory)
        {
            return directory + @"\index.html";
        }
    }
}
