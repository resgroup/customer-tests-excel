using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;
using CustomerTestsExcel.Indentation;

namespace CustomerTestsExcel.NavigationHTML
{
    public class AddNavigationToHTMLOutput
    {
        readonly Queue<DirectoryToIndex> _directories;
        readonly IFileSystem _fileSystem;
        readonly INavigationHTMLFormatter _formatter;

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

            _directories.Enqueue(
                new DirectoryToIndex(
                    fullDirectory: rootDirectory,
                    parentDirectory: null,
                    description: rootName,
                    nestDepth: 0)
                );

            while (_directories.Any())
                AddIndex(_directories.Dequeue());
        }

        private void AddIndex(DirectoryToIndex directoryToIndex)
        {
            using (new TidyUp(() => _formatter.StartIndex(directoryToIndex.Description, directoryToIndex.NestDepth), EndIndex))
            {
                AddLinkToParentDirectory(directoryToIndex);

                AddLinksToTestsAtDirectory(directoryToIndex);

                AddLinksToChildDirectories(directoryToIndex);
            }

            SaveIndex(Index(directoryToIndex.FullDirectory));
        }

        void AddLinksToChildDirectories(DirectoryToIndex directoryToIndex)
        {
            var subDirectories = _fileSystem.Directory.GetDirectories(directoryToIndex.FullDirectory);
            if (subDirectories.Any())
                using (new TidyUp(StartChildren, EndChildren))
                    _fileSystem.Directory.GetDirectories(directoryToIndex.FullDirectory).ToList().ForEach(fullDirectory => AddLinkToChildDirectoryAndEnqueue(directoryToIndex, fullDirectory));
        }

        private void AddLinkToChildDirectoryAndEnqueue(DirectoryToIndex directoryToIndex, string fullDirectory)
        {
            if (fullDirectory.TrimEnd(_fileSystem.Path.DirectorySeparatorChar).ToLowerInvariant() != directoryToIndex.FullDirectory.TrimEnd(_fileSystem.Path.DirectorySeparatorChar).ToLowerInvariant())
            {
                var subDirectory = GetLastDirectoryName(fullDirectory);
                _formatter.AddLink(Index(subDirectory), subDirectory, "childLink");

                EnqueueChildDirectory(directoryToIndex, fullDirectory, subDirectory);
            }
        }

        private void EnqueueChildDirectory(DirectoryToIndex directoryToIndex, string fullDirectory, string subDirectory)
        {
            _directories.Enqueue(
                new DirectoryToIndex(
                    fullDirectory: fullDirectory,
                    parentDirectory: directoryToIndex.FullDirectory,
                    description: subDirectory,
                    nestDepth: directoryToIndex.NestDepth + 1)
                );
        }

        void AddLinksToTestsAtDirectory(DirectoryToIndex directoryToIndex)
        {
            using (new TidyUp(StartTests, EndTests))
                _fileSystem.Directory.GetFiles(directoryToIndex.FullDirectory, "*.html").ToList().ForEach(filename => AddLinkAtCurrentDirectory(filename));
        }

        private void AddLinkAtCurrentDirectory(string filename)
        {
            if (_fileSystem.Path.GetFileName(filename).ToLowerInvariant() != "index.html")
                _formatter.AddLink(_fileSystem.Path.GetFileName(filename), _fileSystem.Path.GetFileNameWithoutExtension(filename), "testLink");
        }

        void AddLinkToParentDirectory(DirectoryToIndex directoryToIndex)
        {
            if (string.IsNullOrEmpty(directoryToIndex.ParentDirectory) == true) return;

            using (new TidyUp(StartParent, EndParent))
                _formatter.AddLink(Index(".."), GetLastDirectoryName(directoryToIndex.ParentDirectory), "parentLink");
        }

        string GetLastDirectoryName(string parentDirectory) => _fileSystem.Path.GetFileName(parentDirectory.TrimEnd(_fileSystem.Path.DirectorySeparatorChar));

        void SaveIndex(string filename) => _fileSystem.File.WriteAllText(filename, _formatter.HTML());

        string Index(string directory) => directory + @"\index.html";

        Action EndIndex => _formatter.EndIndex;
        Action StartTests => _formatter.StartTests;
        Action EndTests => _formatter.EndTests;
        Action StartParent => _formatter.StartParent;
        Action EndParent => _formatter.EndParent;
        Action StartChildren => _formatter.StartChildren;
        Action EndChildren => _formatter.EndChildren;
    }
}
