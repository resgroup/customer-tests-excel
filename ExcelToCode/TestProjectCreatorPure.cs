using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using System.Reflection;

namespace CustomerTestsExcel.ExcelToCode
{
    public struct FileToSave
    {
        public string Content { get; set; }
        public string PathRelativeToProjectRoot { get; set; }
    }

    public class GeneratedProject
    {
        public XDocument CsprojFile { get; set; }
        public List<FileToSave> Files { get; set; }

        public GeneratedProject()
        {
            Files = new List<FileToSave>();
        }
    }

    // This class generates the tests themselves, as well as just the project, so it should be named to better communicate this
    public class TestProjectCreatorPure
    {
        string excelTestsFolderName;
        readonly GivenClassRecorder givenClassRecorder;
        readonly ExcelCsharpClassMatcher excelCsharpClassMatcher;
        readonly ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher;
        readonly ILogger logger;
        GeneratedProject generatedProject;

        public TestProjectCreatorPure(
            ILogger logger)
        {
            givenClassRecorder = new GivenClassRecorder();
            excelCsharpPropertyMatcher = new ExcelCsharpPropertyMatcher();
            excelCsharpClassMatcher = new ExcelCsharpClassMatcher(excelCsharpPropertyMatcher);

            this.logger = logger;
        }

        public GeneratedProject Create(
            string projectRootFolder,
            XDocument existingCsproj,
            IEnumerable<string> excelTestFilenames,
            string projectRootNamespace,
            string excelTestsFolderName,
            IEnumerable<string> usings,
            IEnumerable<Type> typesUnderTest,
            string assertionClassPrefix,
            ITabularLibrary excel)
        {
            this.excelTestsFolderName = excelTestsFolderName;

            generatedProject = new GeneratedProject { CsprojFile = new XDocument(existingCsproj) };

            var compileItemGroupNode = GetItemGroupForCompileNodes(generatedProject.CsprojFile);
            var excelItemGroupNode = GetItemGroupForExcelNodes(generatedProject.CsprojFile);

            GenerateTestClasses(
                projectRootFolder,
                excelTestFilenames,
                projectRootNamespace,
                usings,
                assertionClassPrefix,
                excel,
                logger,
                generatedProject.CsprojFile,
                compileItemGroupNode,
                excelItemGroupNode);

            GenerateSpecificationSpecificSetupClasses(
                projectRootFolder,
                projectRootNamespace,
                usings,
                typesUnderTest,
                generatedProject.CsprojFile,
                compileItemGroupNode);

            GenerateSpecificationSpecificPlaceholder(
                projectRootFolder,
                projectRootNamespace,
                compileItemGroupNode);

            return generatedProject;
        }

        private void GenerateTestClasses(string projectRootFolder, IEnumerable<string> excelTestFilenames, string projectRootNamespace, IEnumerable<string> usings, string assertionClassPrefix, ITabularLibrary excel, ILogger logger, XDocument project, XElement compileItemGroupNode, XElement excelItemGroupNode)
        {
            foreach (var excelFileName in excelTestFilenames)
            {
                excelItemGroupNode.Add(MakeFileElement(project.Root.Name.Namespace.NamespaceName, "None", Path.Combine(excelTestsFolderName, Path.GetFileName(excelFileName))));
                OutputWorkbook(projectRootFolder, projectRootNamespace, usings, assertionClassPrefix, excel, logger, compileItemGroupNode, excelFileName);
            }
        }

        private static void GenerateSpecificationSpecificPlaceholder(string projectRootFolder, string projectRootNamespace, XElement compileItemGroupNode)
        {
            var projectRelativePath = Path.Combine("GeneratedSpecificationSpecific", "Placeholder.cs");
            var outputPath = Path.Combine(projectRootFolder, projectRelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

            File.WriteAllText(outputPath, SpecificationSpecificPlaceholderGenerator.GenerateSpecificationSpecificPlaceholder(projectRootNamespace));

            compileItemGroupNode.Add(
                new XElement(
                    XName.Get(
                        "Compile",
                        compileItemGroupNode.Name.Namespace.NamespaceName
                        ),
                new XAttribute("Include", projectRelativePath)
                )
            );
        }

        private void GenerateSpecificationSpecificSetupClasses(
            string specificationFolder,
            string projectRootNamespace,
            IEnumerable<string> usings,
            IEnumerable<Type> typesUnderTest,
            XDocument project,
            XElement compileItemGroupNode)
        {
            givenClassRecorder.Classes.ToList().ForEach(
                excelGivenClass =>
                {
                    string code = null;

                    if (excelGivenClass.IsRootClass)
                        code = new SpecificationSpecificRootClassGenerator(excelCsharpPropertyMatcher, excelGivenClass).CsharpCode(
                            projectRootNamespace,
                            usings
                        );
                    else
                    {
                        var matchingType = typesUnderTest.FirstOrDefault(t => excelCsharpClassMatcher.Matches(t, excelGivenClass));

                        if (matchingType != null)
                        {
                            code = new SpecificationSpecificClassGenerator(excelCsharpPropertyMatcher, excelGivenClass).CsharpCode(
                                projectRootNamespace,
                                usings,
                                matchingType
                                );
                        }
                        else if (!excelGivenClass.IsFramworkSuppliedClass())
                        {
                            code = new SpecificationSpecificUnmatchedClassGenerator(excelCsharpPropertyMatcher, excelGivenClass).CsharpCode(
                                projectRootNamespace,
                                usings
                                );
                        }
                    }

                    if (code != null)
                    {
                        var customClassAlreadyExists = (project.Descendants().Any(e => e.Name.LocalName == "Compile" && e.Attributes().Any(a => a.Name.LocalName == "Include" && a.Value.Contains($"SpecificationSpecific{excelGivenClass.Name}.cs"))));

                        var fileExtension = (customClassAlreadyExists) ? ".cs.txt" : ".cs";

                        var projectRelativePath = Path.Combine("GeneratedSpecificationSpecific", excelGivenClass.Name + fileExtension);
                        var outputPath = Path.Combine(specificationFolder, projectRelativePath);
                        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

                        File.WriteAllText(outputPath, code);

                        var nodeType = (customClassAlreadyExists) ? "None" : "Compile";

                        compileItemGroupNode.Add(
                            new XElement(
                                XName.Get(
                                    nodeType,
                                    compileItemGroupNode.Name.Namespace.NamespaceName
                                    ),
                            new XAttribute("Include", projectRelativePath)
                            )
                        );
                    }
                }
            );
        }

        XElement GetItemGroupForCompileNodes(XDocument projectFile)
        {
            return GetItemGroupForNodeTypes(projectFile, "Compile");
        }

        XElement GetItemGroupForExcelNodes(XDocument projectFile)
        {
            return GetItemGroupForNodeTypes(projectFile, "None");
        }

        XElement GetItemGroupForNodeTypes(XDocument projectFile, string nodeName)
        {
            var compileNodes = projectFile.Descendants().Where(n => n.Name.LocalName == nodeName);
            XElement compileItemGroupNode;
            if (compileNodes.Any())
            {
                compileItemGroupNode = compileNodes.First().Parent;
                // remove all code except things in the protected "IgnoreOnGeneration" folder, which is kept for non generated things
                compileItemGroupNode.Elements().Where(e => e.Attribute("Include")?.Value?.StartsWith("IgnoreOnGeneration\\") != true).Remove();
            }
            else
            {
                compileItemGroupNode = new XElement("ItemGroup");
                projectFile.Root.Add(compileItemGroupNode);
            }
            return compileItemGroupNode;
        }

        XElement MakeFileElement(string xmlNamespace, string nodeName, string relativeFilePath) =>
            new XElement(XName.Get(nodeName, xmlNamespace), new XAttribute("Include", relativeFilePath));

        void OutputWorkbook(
            string projectRootFolder,
            string projectRootNamespace,
            IEnumerable<string> usings,
            string assertionClassPrefix,
            ITabularLibrary excel,
            ILogger logger,
            XElement compileItemGroupNode,
            string excelFileName)
        {
            using (var workbookFile = GetExcelFileStream(excelFileName))
            {
                using (var workbook = excel.OpenBook(workbookFile))
                {
                    var workBookName = Path.GetFileNameWithoutExtension(excelFileName);
                    for (int i = 0; i < workbook.NumberOfPages; i++)
                    {
                        var sheet = workbook.GetPage(i);
                        if (IsTestSheet(sheet))
                        {
                            var cSharpCode = OutputWorkSheet(usings, assertionClassPrefix, workBookName, sheet, logger, projectRootNamespace);

                            var projectRelativePath = Path.Combine(workBookName, sheet.Name + ".cs");

                            // add to the new struct here, initially as well as saving to file
                            // then write code in the wrapper to save the files
                            // then use that, and remove the code here
                            generatedProject.Files.Add(new FileToSave { Content = cSharpCode, PathRelativeToProjectRoot = projectRelativePath });

                            // save test code to file
                            //var outputPath = Path.Combine(projectRootFolder, projectRelativePath);
                            //Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                            //using (var outputFile = new StreamWriter(outputPath))
                            //{
                            //    try
                            //    {
                            //        outputFile.Write(cSharpCode);
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        outputFile.Write(string.Format("Error creating c# from Excel: {0}", ex.Message));
                            //    }
                            //}

                            compileItemGroupNode.Add(MakeFileElement(compileItemGroupNode.Name.Namespace.NamespaceName, "Compile", projectRelativePath));
                        }
                    }
                }
            }
        }

        Stream GetExcelFileStream(string excelFile, string temporaryFolder = null)
        {
            var templateStream = new MemoryStream();

            if (string.IsNullOrEmpty(temporaryFolder))
                temporaryFolder = Path.GetTempPath();

            var tempFileName = Path.Combine(temporaryFolder, Guid.NewGuid().ToString("N") + ".tmp");

            try
            {
                File.Copy(excelFile, tempFileName, true);

                var attr = File.GetAttributes(tempFileName);
                File.SetAttributes(tempFileName, (attr | FileAttributes.Temporary) & ~FileAttributes.ReadOnly);

                using (var fs = new FileStream(tempFileName, FileMode.Open, FileAccess.Read))
                {
                    fs.CopyTo(templateStream);
                }
            }
            finally
            {
                File.Delete(tempFileName);
            }

            templateStream.Seek(0, SeekOrigin.Begin);
            return templateStream;
        }

        bool IsTestSheet(ITabularPage excelSheet) =>
            excelSheet.GetCell(1, 1).Value != null && (excelSheet.GetCell(1, 1).Value.ToString() == "Specification");

        string OutputWorkSheet(IEnumerable<string> usings, string assertionClassPrefix, string workBookName, ITabularPage sheet, ILogger logger, string projectRootNamespace)
        {
            var sheetConverter = new ExcelToCode(new CodeNameToExcelNameConverter(assertionClassPrefix));
            sheetConverter.AddVisitor(givenClassRecorder);

            // this would be better as a property on sheetConverter, instead of the return value
            var cSharpTestCode = sheetConverter.GenerateCSharpTestCode(usings, sheet, projectRootNamespace, workBookName);

            sheetConverter.Errors.ToList().ForEach(error => logger.LogWorkbookError(workBookName, sheet.Name, error));

            sheetConverter.Warnings.ToList().ForEach(warning => logger.LogWarning(workBookName, sheet.Name, warning));

            sheetConverter.IssuesPreventingRoundTrip.ToList().ForEach(issue => logger.LogIssuePreventingRoundTrip(workBookName, sheet.Name, issue));

            return cSharpTestCode;
        }
    }
}
