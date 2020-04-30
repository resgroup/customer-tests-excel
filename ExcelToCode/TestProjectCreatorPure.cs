using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;

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
        XElement compileItemGroupNode;
        XElement excelItemGroupNode;

        public TestProjectCreatorPure(
            ILogger logger)
        {
            givenClassRecorder = new GivenClassRecorder();
            excelCsharpPropertyMatcher = new ExcelCsharpPropertyMatcher();
            excelCsharpClassMatcher = new ExcelCsharpClassMatcher(excelCsharpPropertyMatcher);

            this.logger = logger;
        }

        public GeneratedProject Create(
            XDocument existingCsproj,
            IEnumerable<ITabularBook> excelSpreadsheets,
            string projectRootNamespace,
            string excelTestsFolderName,
            IEnumerable<string> usings,
            IEnumerable<Type> typesUnderTest,
            string assertionClassPrefix,
            ITabularLibrary excel)
        {
            this.excelTestsFolderName = excelTestsFolderName;

            generatedProject = new GeneratedProject { CsprojFile = new XDocument(existingCsproj) };

            compileItemGroupNode = GetItemGroupForCompileNodes(generatedProject.CsprojFile);
            excelItemGroupNode = GetItemGroupForExcelNodes(generatedProject.CsprojFile);

            GenerateTestClasses(
                excelSpreadsheets,
                projectRootNamespace,
                usings,
                assertionClassPrefix,
                logger);

            GenerateSpecificationSpecificSetupClasses(
                projectRootNamespace,
                usings,
                typesUnderTest);

            GenerateSpecificationSpecificPlaceholder(projectRootNamespace);

            return generatedProject;
        }

        private void GenerateTestClasses(IEnumerable<ITabularBook> workbooks, string projectRootNamespace, IEnumerable<string> usings, string assertionClassPrefix, ILogger logger)
        {
            foreach (var workbook in workbooks)
            {
                excelItemGroupNode.Add(MakeFileElement(generatedProject.CsprojFile.Root.Name.Namespace.NamespaceName, "None", Path.Combine(excelTestsFolderName, Path.GetFileName(workbook.Filename))));
                OutputWorkbook(projectRootNamespace, usings, assertionClassPrefix, logger, workbook);
            }
        }

        void GenerateSpecificationSpecificPlaceholder(string projectRootNamespace)
        {
            AddCsharpFile(
                SpecificationSpecificPlaceholderGenerator.GenerateSpecificationSpecificPlaceholder(projectRootNamespace),
                Path.Combine("GeneratedSpecificationSpecific", "Placeholder.cs"));
        }

        void AddCsharpFile(string cSharpCode, string projectRelativePath)
        {
            AddFile(cSharpCode, projectRelativePath);

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

        void AddTextFile(string textFileContent, string projectRelativePath)
        {
            AddFile(textFileContent, projectRelativePath);

            compileItemGroupNode.Add(
                new XElement(
                    XName.Get(
                        "None",
                        compileItemGroupNode.Name.Namespace.NamespaceName
                        ),
                new XAttribute("Include", projectRelativePath)
                )
            );
        }

        void AddFile(string cSharpCode, string projectRelativePath) =>
            generatedProject.Files.Add(new FileToSave { Content = cSharpCode, PathRelativeToProjectRoot = projectRelativePath });

        void GenerateSpecificationSpecificSetupClasses(
            string projectRootNamespace,
            IEnumerable<string> usings,
            IEnumerable<Type> typesUnderTest)
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
                        var projectRelativePath = Path.Combine("GeneratedSpecificationSpecific", excelGivenClass.Name);

                        var customClassAlreadyExists = generatedProject.CsprojFile.Descendants().Any(e => e.Name.LocalName == "Compile" && e.Attributes().Any(a => a.Name.LocalName == "Include" && a.Value.Contains($"SpecificationSpecific{excelGivenClass.Name}.cs")));

                        if (customClassAlreadyExists)
                            AddTextFile(code, $"{projectRelativePath}.cs.txt");
                        else
                            AddCsharpFile(code, $"{projectRelativePath}.cs");
                    }
                }
            );
        }

        void OutputWorkbook(
            string projectRootNamespace,
            IEnumerable<string> usings,
            string assertionClassPrefix,
            ILogger logger,
            ITabularBook workbook)
        {
            var workBookName = Path.GetFileNameWithoutExtension(workbook.Filename);
            for (int i = 0; i < workbook.NumberOfPages; i++)
            {
                var sheet = workbook.GetPage(i);
                if (IsTestSheet(sheet))
                {
                    var cSharpCode = OutputWorkSheet(usings, assertionClassPrefix, workBookName, sheet, logger, projectRootNamespace);

                    var projectRelativePath = Path.Combine(workBookName, sheet.Name + ".cs");

                    AddCsharpFile(cSharpCode, projectRelativePath);
                }
            }

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


        XElement GetItemGroupForCompileNodes(XDocument projectFile) =>
            GetItemGroupForNodeTypes(projectFile, "Compile");

        XElement GetItemGroupForExcelNodes(XDocument projectFile) =>
            GetItemGroupForNodeTypes(projectFile, "None");

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

    }
}