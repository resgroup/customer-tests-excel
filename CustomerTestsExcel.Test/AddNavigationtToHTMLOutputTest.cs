using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO.Abstractions.TestingHelpers;
using Moq;
using System.IO;
using CustomerTestsExcel.NavigationHTML;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class AddNavigationtToHTMLOutputTest
    {
        struct NavigationHTMLFormatterSpy
        {
            public List<string> CalculatedUrls;
            public List<string> CalculatedDescriptions;
            public Mock<INavigationHTMLFormatter> Formatter;
        }

        [Test]
        public void UrlShouldBeFilenameWithoutPath()
        {
            string filename = @"c:\test.html";

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filename, new MockFileData("") }
            });

            var spy = CreateNavigationHTMLFormatterSpy();

            //act
            new AddNavigationToHTMLOutput(fileSystem, spy.Formatter.Object).CreateIndexHtmlFiles(@"c:\", "");

            Assert.AreEqual(Path.GetFileName(filename), spy.CalculatedUrls[0]);
        }

        [Test]
        public void DescriptionShouldBeFilenameWithoutPathOrExtension()
        {
            string filename = @"c:\test.html";

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filename, new MockFileData("") }
            });

            var spy = CreateNavigationHTMLFormatterSpy();

            //act
            new AddNavigationToHTMLOutput(fileSystem, spy.Formatter.Object).CreateIndexHtmlFiles(@"c:\", "");


            Assert.AreEqual(Path.GetFileNameWithoutExtension(filename), spy.CalculatedDescriptions[0]);
        }

        [Test]
        public void OnlyDotHTMLFilesShouldBeIncluded()
        {
            string filename = @"c:\test.nothtml";

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filename, new MockFileData("") }
            });

            var spy = CreateNavigationHTMLFormatterSpy();

            //act
            new AddNavigationToHTMLOutput(fileSystem, spy.Formatter.Object).CreateIndexHtmlFiles(@"c:\", "");

            Assert.AreEqual(0, spy.CalculatedUrls.Count);
        }

        [Test]
        public void IndexDotHtmlShouldBeIgnored()
        {
            string filename = @"c:\index.html";

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filename, new MockFileData("") }
            });

            var spy = CreateNavigationHTMLFormatterSpy();

            //act
            new AddNavigationToHTMLOutput(fileSystem, spy.Formatter.Object).CreateIndexHtmlFiles(@"c:\", "");

            Assert.AreEqual(0, spy.CalculatedUrls.Count);
        }

        [Test]
        public void SubdirectoriesShouldRelativeLinkToIndexDotHtml()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(@"c:\subdir\");

            var spy = CreateNavigationHTMLFormatterSpy();

            //act
            new AddNavigationToHTMLOutput(fileSystem, spy.Formatter.Object).CreateIndexHtmlFiles(@"c:\", "");

            Assert.AreEqual(@"subdir\index.html", spy.CalculatedUrls[0]);
        }

        [Test]
        public void DescriptionShouldBeSubDirectoryName()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(@"c:\subdir\");

            var spy = CreateNavigationHTMLFormatterSpy();

            //act
            new AddNavigationToHTMLOutput(fileSystem, spy.Formatter.Object).CreateIndexHtmlFiles(@"c:\", "");

            Assert.AreEqual("subdir", spy.CalculatedDescriptions[0]);
        }

        [Test]
        public void RunnableFromAnyFolder()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(@"c:\tests\subdir\");

            var spy = CreateNavigationHTMLFormatterSpy();

            //act
            new AddNavigationToHTMLOutput(fileSystem, spy.Formatter.Object).CreateIndexHtmlFiles(@"c:\tests\", "");

            Assert.AreEqual("subdir", spy.CalculatedDescriptions[0]);
        }

        [Test]
        public void CanPassInDirectoryWithNoBackslashAtEnd()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(@"c:\tests\subdir\");

            var spy = CreateNavigationHTMLFormatterSpy();

            //act
            new AddNavigationToHTMLOutput(fileSystem, spy.Formatter.Object).CreateIndexHtmlFiles(@"c:\tests", "");

            Assert.AreEqual("subdir", spy.CalculatedDescriptions[0]);
        }

        [Test]
        // cedd 2014-06-30: this test is a bit big and complicated ....
        public void NestedFiles()
        {
            const int NestingDepth = 5; // must be greater than one for test to make sense

            var fileSystem = new MockFileSystem();
            for (int depth = 0; depth < NestingDepth; depth++)
            {
                var subdir = @"C:\subdir-1\";
                for (int i = 0; i < depth; i++) subdir = subdir + "subdir" + i.ToString() + @"\";

                fileSystem.AddDirectory(subdir);
                fileSystem.AddFile(subdir + @"\test" + depth.ToString() + ".html", new MockFileData(""));
            }

            var spy = CreateNavigationHTMLFormatterSpy();

            //act
            new AddNavigationToHTMLOutput(fileSystem, spy.Formatter.Object).CreateIndexHtmlFiles(@"c:\subdir-1", "");

            // assert
            var expectedUrls = new List<string>();
            var expectedDescriptions = new List<string>();

            // 1st level file
            expectedUrls.Add("test0.html");
            expectedDescriptions.Add("test0");

            // 1st level directory
            expectedUrls.Add(@"subdir0\index.html");
            expectedDescriptions.Add("subdir0");

            // intermediate levels
            for (int depth = 1; depth < NestingDepth - 1; depth++)
            {
                // parent directory
                expectedUrls.Add(@"..\index.html");
                expectedDescriptions.Add("subdir" + (depth - 2).ToString());

                // file
                expectedUrls.Add("test" + depth.ToString() + ".html");
                expectedDescriptions.Add("test" + depth.ToString());

                // sub directory
                expectedUrls.Add("subdir" + depth.ToString() + @"\index.html");
                expectedDescriptions.Add("subdir" + depth.ToString());
            }

            // last level parent directory
            expectedUrls.Add(@"..\index.html");
            expectedDescriptions.Add("subdir" + (NestingDepth - 1 - 2).ToString());

            // last level file
            expectedUrls.Add("test" + (NestingDepth - 1).ToString() + ".html");
            expectedDescriptions.Add("test" + (NestingDepth - 1).ToString());

            CollectionAssert.AreEquivalent(expectedUrls, spy.CalculatedUrls);
            CollectionAssert.AreEquivalent(expectedDescriptions, spy.CalculatedDescriptions);
        }

        private static NavigationHTMLFormatterSpy CreateNavigationHTMLFormatterSpy()
        {
            var calculatedUrls = new List<String>();
            var calculatedDescriptions = new List<String>();

            var formatter = new Mock<INavigationHTMLFormatter>();
            formatter.Setup(m => m.AddLink(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Callback((string url, string description, string cssClass) => { calculatedUrls.Add(url); calculatedDescriptions.Add(description); });
            formatter.Setup(m => m.HTML()).Returns("");

            return new NavigationHTMLFormatterSpy()
            {
                CalculatedDescriptions = calculatedDescriptions,
                CalculatedUrls = calculatedUrls,
                Formatter = formatter
            };
        }
    }
}
