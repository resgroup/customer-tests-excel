using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;

namespace CustomerTestsExcel.ExcelToCode
{
    public class InMemoryGenerateCSharpFromExcel
    {
        readonly string excelTestsFolderName;
        readonly IEnumerable<string> usings;
        readonly IEnumerable<Type> typesUnderTest;
        readonly string assertionClassPrefix;
        readonly GivenClassRecorder givenClassRecorder;
        readonly ExcelCsharpClassMatcher excelCsharpClassMatcher;
        readonly ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher;
        readonly ILogger logger;
        readonly IEnumerable<ITabularBook> workbooks;
        readonly string projectRootNamespace;
        readonly GeneratedCsharpProject generatedProject;
        readonly XElement compileItemGroupNode;
        readonly XElement excelItemGroupNode;

        public InMemoryGenerateCSharpFromExcel(
            ILogger logger,
            XDocument existingCsproj,
            IEnumerable<ITabularBook> workbooks,
            string projectRootNamespace,
            string excelTestsFolderName,
            IEnumerable<string> usings,
            IEnumerable<Type> typesUnderTest,
            string assertionClassPrefix)
        {
            givenClassRecorder = new GivenClassRecorder();
            excelCsharpPropertyMatcher = new ExcelCsharpPropertyMatcher();
            excelCsharpClassMatcher = new ExcelCsharpClassMatcher(excelCsharpPropertyMatcher);

            this.logger = logger;
            this.workbooks = workbooks;
            this.projectRootNamespace = projectRootNamespace;
            this.excelTestsFolderName = excelTestsFolderName;
            this.usings = usings;
            this.typesUnderTest = typesUnderTest;
            this.assertionClassPrefix = assertionClassPrefix;

            generatedProject = new GeneratedCsharpProject {
                CsprojFile = new XDocument(existingCsproj)
            };
            compileItemGroupNode = GetOrCreateItemGroupForCompileNodes(generatedProject.CsprojFile);
            excelItemGroupNode = GetOrCreateItemGroupForExcelNodes(generatedProject.CsprojFile);
        }

        public GeneratedCsharpProject Generate()
        {
            RemoveGeneratedFilesFromCsproj();

            GenerateTestClasses();

            GenerateSpecificationSpecificSetupClasses();

            GenerateSpecificationSpecificPlaceholder();

            return generatedProject;
        }

        void RemoveGeneratedFilesFromCsproj()
        {
            var itemGroupNodes =
                generatedProject
                .CsprojFile
                .Descendants()
                .Where(n => n.Name.LocalName == "ItemGroup");

            if (itemGroupNodes.Any())
            {
                // remove all code except things in the protected "IgnoreOnGeneration" folder, which is kept for non generated things
                itemGroupNodes
                    .Elements()
                    .Where(e => e.Name.LocalName == "Compile" || e.Name.LocalName == "None")
                    .Where(e => e.Attribute("Include")
                    ?.Value
                    ?.StartsWith("IgnoreOnGeneration\\") != true)
                    .Remove();
            }
        }

        void GenerateTestClasses()
        {
            foreach (var workbook in workbooks)
            {
                AddFileToCsproj(excelItemGroupNode, "None", Path.Combine(excelTestsFolderName, Path.GetFileName(workbook.Filename)));

                OutputWorkbook(workbook);
            }
        }

        void OutputWorkbook(ITabularBook workbook)
        {
            var workBookName = Path.GetFileNameWithoutExtension(workbook.Filename);
            for (int i = 0; i < workbook.NumberOfPages; i++)
            {
                var sheet = workbook.GetPage(i);
                if (IsTestSheet(sheet))
                {
                    var cSharpCode = OutputWorkSheet(workBookName, sheet);

                    var projectRelativePath = Path.Combine(workBookName, sheet.Name + ".cs");

                    AddCsharpFile(cSharpCode, projectRelativePath);
                }
            }

        }

        bool IsTestSheet(ITabularPage excelSheet) =>
            excelSheet.GetCell(1, 1).Value != null 
            && (excelSheet.GetCell(1, 1).Value.ToString() == "Specification");

        string OutputWorkSheet(string workBookName, ITabularPage sheet)
        {
            var sheetConverter = new ExcelToCode(new CodeNameToExcelNameConverter(assertionClassPrefix));
            sheetConverter.logState.AddVisitor(givenClassRecorder);

            var cSharpTestCode = sheetConverter.GenerateCSharpTestCode(
                usings,
                sheet,
                projectRootNamespace,
                workBookName);

            // Could potentially pass the logger in to sheet converter instead of doing this
            // This would avoid the slightly messy situation where the class returns some
            // data from the function and some as instance properties.
            sheetConverter.logState.Errors.ToList().ForEach(error => logger.LogWorkbookError(workBookName, sheet.Name, error));

            sheetConverter.logState.Warnings.ToList().ForEach(warning => logger.LogWarning(workBookName, sheet.Name, warning));

            sheetConverter.logState.IssuesPreventingRoundTrip.ToList().ForEach(issue => logger.LogIssuePreventingRoundTrip(workBookName, sheet.Name, issue));

            return cSharpTestCode;
        }

        void GenerateSpecificationSpecificPlaceholder()
        {
            AddCsharpFile(
                SpecificationSpecificPlaceholderGenerator.GenerateSpecificationSpecificPlaceholder(projectRootNamespace),
                Path.Combine("GeneratedSpecificationSpecific", "Placeholder.cs"));
        }

        void GenerateSpecificationSpecificSetupClasses()
        {
            givenClassRecorder.Classes.ToList().ForEach(
                GenerateSpecificationSpecificSetupClass
            );
        }

        void GenerateSpecificationSpecificSetupClass(GivenClass excelGivenClass)
        {
            if (excelGivenClass.IsRootClass)
                GenerateSpecificationSpecificRootClass(excelGivenClass);
            else
            {
                var matchingType = typesUnderTest.FirstOrDefault(t => excelCsharpClassMatcher.Matches(t, excelGivenClass));

                if (matchingType != null)
                {
                    GeneratedSpecificationSpecificMatchedClass(excelGivenClass, matchingType);
                }
                else if (!excelGivenClass.IsFramworkSuppliedClass())
                {
                    GeneratedSpecificationSpecificClassWithoutMatchingTypeInSut(excelGivenClass);
                }
            }
        }

        void GenerateSpecificationSpecificRootClass(GivenClass excelGivenClass)
        {
            var code =
                new SpecificationSpecificRootClassGenerator(
                    excelCsharpPropertyMatcher,
                    excelGivenClass)
                .CsharpCode(
                    projectRootNamespace,
                    usings
                );

            AddGeneratedFile(excelGivenClass, code);
        }

        void GeneratedSpecificationSpecificMatchedClass(
            GivenClass excelGivenClass, 
            Type matchingType)
        {
            var code =
                new SpecificationSpecificClassGenerator(
                    excelCsharpPropertyMatcher,
                    excelGivenClass)
                .CsharpCode(
                    projectRootNamespace,
                    usings,
                    matchingType
                );

            AddGeneratedFile(excelGivenClass, code);
        }

        void GeneratedSpecificationSpecificClassWithoutMatchingTypeInSut(GivenClass excelGivenClass)
        {
            var code =
                new SpecificationSpecificUnmatchedClassGenerator(
                    excelCsharpPropertyMatcher,
                    excelGivenClass)
                .CsharpCode(
                    projectRootNamespace,
                    usings
                );

            AddGeneratedFile(excelGivenClass, code);
        }

        void AddGeneratedFile(GivenClass excelGivenClass, string code)
        {
            var projectRelativePath = Path.Combine("GeneratedSpecificationSpecific", excelGivenClass.Name);

            var customClassAlreadyExists = 
                generatedProject
                .CsprojFile
                .Descendants()
                .Any(
                    e => 
                        e.Name.LocalName == "Compile" 
                        && e.Attributes().Any(
                            a => a.Name.LocalName == "Include" 
                            && a.Value.Contains(
                                $"SpecificationSpecific{excelGivenClass.Name}.cs"
                            )
                        )
                 );

            if (customClassAlreadyExists)
                AddTextFile(code, $"{projectRelativePath}.cs.txt");
            else
                AddCsharpFile(code, $"{projectRelativePath}.cs");
        }

        void AddCsharpFile(string cSharpCode, string filePathRelativeToProject)
        {
            AddFile(cSharpCode, filePathRelativeToProject);

            AddFileToCsproj(compileItemGroupNode, "Compile", filePathRelativeToProject);
        }

        void AddTextFile(string textFileContent, string filePathRelativeToProject)
        {
            AddFile(textFileContent, filePathRelativeToProject);

            AddFileToCsproj(compileItemGroupNode, "None", filePathRelativeToProject);
        }

        void AddFile(string cSharpCode, string projectRelativePath)
        {
            generatedProject.Files.Add(
                new CsharpProjectFileToSave {
                    Content = cSharpCode,
                    PathRelativeToProjectRoot = projectRelativePath
                }
            );
        }

        void AddFileToCsproj(
            XElement groupNode, 
            string buildAction, 
            string filePathRelativeToProject)
        {
            groupNode.Add(
                new XElement(
                    XName.Get(
                        buildAction,
                        groupNode.Name.Namespace.NamespaceName
                        ),
                    new XAttribute("Include", filePathRelativeToProject)
                )
            );
        }

        XElement GetOrCreateItemGroupForCompileNodes(XDocument projectFile) =>
            GetOrCreateItemGroupForNodeType(projectFile, "Compile");

        XElement GetOrCreateItemGroupForExcelNodes(XDocument projectFile) =>
            GetOrCreateItemGroupForNodeType(projectFile, "None");

        XElement GetOrCreateItemGroupForNodeType(XDocument projectFile, string nodeName)
        {
            var itemGroupNodes =
                projectFile
                .Descendants()
                .Where(n => n.Name.LocalName == nodeName);

            if (itemGroupNodes.Any())
            {
                return itemGroupNodes.First().Parent;
            }
            else
            {
                var itemGroupNode = new XElement("ItemGroup");
                projectFile.Root.Add(itemGroupNode);
                return itemGroupNode;
            }
        }
    }
}