using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;

namespace AddNavigationToSpecificationHTMLOutputFiles
{
    public class AddNavigationToHTMLOutput
    {
        protected string _rootDirectory;
        protected IFileSystem _fileSystem;
        protected Queue<DirectoryToIndex> _directories;
        protected INavigationHTMLFormatter _formatter;

        protected struct DirectoryToIndex
        {
            public string FullDirectory;
            public string ParentDirectory;
            public string Description;
            public int NestDepth;
        }


        public AddNavigationToHTMLOutput(IFileSystem fileSystem, INavigationHTMLFormatter formatter)
        {
            if (fileSystem == null) throw new ArgumentNullException("fileSystem");
            if (formatter == null) throw new ArgumentNullException("formatter");

            _fileSystem = fileSystem;
            _formatter = formatter;

            _directories = new Queue<DirectoryToIndex>();
        }

        public void CreateIndexHtmlFiles(string rootDirectory, string rootName)
        {
            if (string.IsNullOrEmpty(rootDirectory)) throw new ArgumentNullException("rootDirectory");

            _directories.Enqueue(new DirectoryToIndex() { FullDirectory = rootDirectory, ParentDirectory =  null, Description = rootName, NestDepth = 0});

            while (_directories.Any())
            {
                AddIndex(_directories.Dequeue());
            }
        }

        protected void AddIndex(DirectoryToIndex directoryToIndex)
        {
            _formatter.StartIndex(directoryToIndex.Description, directoryToIndex.NestDepth);

            if (string.IsNullOrEmpty(directoryToIndex.ParentDirectory) == false)
            {
                _formatter.StartParent();
                _formatter.AddLink(Index(".."), GetLastDirectoryName(directoryToIndex.ParentDirectory), "parentLink");
                _formatter.EndParent();
            }

            _formatter.StartTests();
            foreach (var filename in _fileSystem.Directory.GetFiles(directoryToIndex.FullDirectory, "*.html"))
            {
                if (_fileSystem.Path.GetFileName(filename).ToLower() != "index.html")
                {
                    _formatter.AddLink(_fileSystem.Path.GetFileName(filename), _fileSystem.Path.GetFileNameWithoutExtension(filename), "testLink");
                }
            }
            _formatter.EndTests();

            var subSirectories = _fileSystem.Directory.GetDirectories(directoryToIndex.FullDirectory);
            if (subSirectories.Any())
            {
                _formatter.StartChildren();
                foreach (var fullDirectory in _fileSystem.Directory.GetDirectories(directoryToIndex.FullDirectory))
                {
                    if (fullDirectory.TrimEnd(_fileSystem.Path.DirectorySeparatorChar).ToLower() != directoryToIndex.FullDirectory.TrimEnd(_fileSystem.Path.DirectorySeparatorChar).ToLower())
                    {
                        var subDirectory = GetLastDirectoryName(fullDirectory);
                        _formatter.AddLink(Index(subDirectory), subDirectory, "childLink");
                        _directories.Enqueue(
                            new DirectoryToIndex()
                            {
                                FullDirectory = fullDirectory,
                                ParentDirectory = directoryToIndex.FullDirectory,
                                Description = subDirectory,
                                NestDepth = directoryToIndex.NestDepth + 1
                            });
                    }
                }
                _formatter.EndChildren();
            }
            _formatter.EndIndex();

            SaveIndex(Index(directoryToIndex.FullDirectory));
        }

        private string GetLastDirectoryName(string parentDirectory)
        {
            return _fileSystem.Path.GetFileName(parentDirectory.TrimEnd(_fileSystem.Path.DirectorySeparatorChar));
        }

        private void SaveIndex(string filename)
        {
            _fileSystem.File.WriteAllText(filename, _formatter.HTML());
        }

        private string Index(string directory)
        {
            return directory + @"\index.html";
        }
    }
}
