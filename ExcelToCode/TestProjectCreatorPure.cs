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
    // This class generates the tests themselves, as well as just the project, so it should be named to better communicate this
    public class TestProjectCreatorPure
    {
        string excelTestsFolderName;
        readonly GivenClassRecorder givenClassRecorder;
        readonly ExcelCsharpClassMatcher excelCsharpClassMatcher;
        readonly ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher;
        readonly ILogger logger;

        public TestProjectCreatorPure(
            ILogger logger)
        {
            givenClassRecorder = new GivenClassRecorder();
            excelCsharpPropertyMatcher = new ExcelCsharpPropertyMatcher();
            excelCsharpClassMatcher = new ExcelCsharpClassMatcher(excelCsharpPropertyMatcher);

            this.logger = logger;
        }

        public XDocument Create(
            string specificationFolder,
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

            var newCsproj = new XDocument(existingCsproj);
            var compileItemGroupNode = GetItemGroupForCompileNodes(newCsproj);
            var excelItemGroupNode = GetItemGroupForExcelNodes(newCsproj);

            GenerateTestClasses(
                specificationFolder,
                excelTestFilenames, 
                projectRootNamespace, 
                usings, 
                assertionClassPrefix, 
                excel, 
                logger,
                newCsproj, 
                compileItemGroupNode, 
                excelItemGroupNode);

            GenerateSpecificationSpecificSetupClasses(
                specificationFolder,
                projectRootNamespace,
                usings,
                typesUnderTest,
                newCsproj,
                compileItemGroupNode);

            GenerateSpecificationSpecificPlaceholder(
                specificationFolder,
                projectRootNamespace,
                compileItemGroupNode);

            return newCsproj;
        }
        
        private void GenerateTestClasses(string specificationFolder, IEnumerable<string> excelTestFilenames, string projectRootNamespace, IEnumerable<string> usings, string assertionClassPrefix, ITabularLibrary excel, ILogger logger, XDocument project, XElement compileItemGroupNode, XElement excelItemGroupNode)
        {
            foreach (var excelFileName in excelTestFilenames)
            {
                excelItemGroupNode.Add(MakeFileElement(project.Root.Name.Namespace.NamespaceName, "None", Path.Combine(excelTestsFolderName, Path.GetFileName(excelFileName))));
                OutputWorkbook(specificationFolder, projectRootNamespace, usings, assertionClassPrefix, excel, logger, compileItemGroupNode, excelFileName);
            }
        }

        private static void GenerateSpecificationSpecificPlaceholder(string specificationFolder, string projectRootNamespace, XElement compileItemGroupNode)
        {
            var projectRelativePath = Path.Combine("GeneratedSpecificationSpecific", "Placeholder.cs");
            var outputPath = Path.Combine(specificationFolder, projectRelativePath);
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

        IEnumerable<Type> GetTypesFromAssembly(string assemblyFilename)
        {
            try
            {
                return Assembly.LoadFile(assemblyFilename).GetTypes();
            }
            catch (Exception exception)
            {
                logger.LogAssemblyError(assemblyFilename, exception);
                return new List<Type>();
            }
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

        XElement MakeFileElement(string xmlNamespace, string nodeName, string relativeFilePath)
        {
            return new XElement(XName.Get(nodeName, xmlNamespace), new XAttribute("Include", relativeFilePath));
        }

        void OutputWorkbook(
            string specificationFolder,
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
                        if (IsTestSheet(workbook.GetPage(i)))
                        {
                            compileItemGroupNode.Add(MakeFileElement(compileItemGroupNode.Name.Namespace.NamespaceName, "Compile", OutputWorkSheet(specificationFolder, usings, assertionClassPrefix, workBookName, workbook.GetPage(i), logger, projectRootNamespace)));
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

        string OutputWorkSheet(string outputFolder, IEnumerable<string> usings, string assertionClassPrefix, string workBookName, ITabularPage sheet, ILogger logger, string projectRootNamespace)
        {
            var sheetConverter = new ExcelToCode(new CodeNameToExcelNameConverter(assertionClassPrefix));
            sheetConverter.AddVisitor(givenClassRecorder);

            // generate test code
            var cSharpTestCode = sheetConverter.GenerateCSharpTestCode(usings, sheet, projectRootNamespace, workBookName);

            // log any errors
            if (sheetConverter.Errors.Any())
            {
                sheetConverter.Errors.ToList().ForEach(error => logger.LogWorkbookError(workBookName, sheet.Name, error));
            }

            // log any warnings
            if (sheetConverter.Warnings.Any())
                sheetConverter.Warnings.ToList().ForEach(warning => logger.LogWarning(workBookName, sheet.Name, warning));

            // log any issues preventing round trip
            if (sheetConverter.IssuesPreventingRoundTrip.Any())
                sheetConverter.IssuesPreventingRoundTrip.ToList().ForEach(issue => logger.LogIssuePreventingRoundTrip(workBookName, sheet.Name, issue));

            // save test code to file
            var projectRelativePath = Path.Combine(workBookName, sheet.Name + ".cs");
            var outputPath = Path.Combine(outputFolder, projectRelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            using (var outputFile = new StreamWriter(outputPath))
            {
                try
                {
                    outputFile.Write(cSharpTestCode);
                }
                catch (Exception ex)
                {
                    outputFile.Write(string.Format("Error creating c# from Excel: {0}", ex.Message));
                }
            }

            // stupidly return the relative path
            return projectRelativePath;
        }
    }
}
