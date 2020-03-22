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
    public class TestProjectCreator
    {
        readonly GivenClassRecorder givenClassRecorder;
        readonly SpecificationSpecificClassGenerator specificationSpecificClassGenerator;
        readonly ExcelCsharpClassMatcher excelCsharpClassMatcher;
        bool success;

        public TestProjectCreator()
        {
            givenClassRecorder = new GivenClassRecorder();
            specificationSpecificClassGenerator = new SpecificationSpecificClassGenerator(
                new ExcelCsharpPropertyMatcher()
            );
            excelCsharpClassMatcher = new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher());
        }

        public int Create(
            string specificationFolder,
            string specificationProject,
            string excelTestsFolder,
            string projectRootNamespace,
            IEnumerable<string> usings,
            IEnumerable<string> assembliesUnderTest,
            string assertionClassPrefix,
            ITabularLibrary excel,
            ILogger logger)
        {
            success = true;

            var projectFilePath = Path.Combine(specificationFolder, specificationProject);

            var project = OpenProjectFile(projectFilePath);
            var compileItemGroupNode = GetItemGroupForCompileNodes(project);
            var excelItemGroupNode = GetItemGroupForExcelNodes(project);

            string excelFolder = Path.Combine(specificationFolder, excelTestsFolder);
            foreach (var excelFileName in ListValidSpecificationSpreadsheets(excelFolder))
            {
                excelItemGroupNode.Add(MakeFileElement(project.Root.Name.Namespace.NamespaceName, "None", Path.Combine(excelTestsFolder, Path.GetFileName(excelFileName))));
                OutputWorkbook(specificationFolder, projectRootNamespace, usings, assertionClassPrefix, excel, logger, compileItemGroupNode, excelFileName);
            }

            var assemblyTypes = new List<Type>();

            foreach (var assemblyFilename in assembliesUnderTest)
            {
                assemblyTypes.AddRange(Assembly.LoadFile(assemblyFilename).GetTypes());
            }

            givenClassRecorder.Classes.ToList().ForEach(
                excelGivenClass =>
                {

                    var assemblyType = assemblyTypes.FirstOrDefault(t => excelCsharpClassMatcher.Matches(t, excelGivenClass));

                    if (assemblyType != null)
                    {
                        var code = specificationSpecificClassGenerator.cSharpCode(
                            projectRootNamespace,
                            usings.ToList(), // change this to an ienumerable
                            assemblyType,
                            excelGivenClass
                        );

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

            SaveProjectFile(projectFilePath, project);

            return success ? 0 : 1;
        }

        IEnumerable<string> ListValidSpecificationSpreadsheets(string excelFolder)
        {
            var combinedList = new List<string>();
            combinedList.AddRange(Directory.GetFiles(excelFolder, "*.xlsx"));
            combinedList.AddRange(Directory.GetFiles(excelFolder, "*.xlsm"));
            combinedList = combinedList.Where(f => !f.Contains("~$")).ToList(); // these are temporary files created by excel when the main file is open.
            return combinedList;
        }

        XDocument OpenProjectFile(string projectPath)
        {
            XDocument projectFile;
            using (var projectStreamReader = new StreamReader(projectPath))
            {
                projectFile = XDocument.Load(projectStreamReader.BaseStream);
            }
            return projectFile;
        }

        void SaveProjectFile(string projectPath, XDocument projectFile)
        {
            using (var projectStreamWriter = new StreamWriter(projectPath))
            {
                projectFile.Save(projectStreamWriter.BaseStream);
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
                success = false;
                sheetConverter.Errors.ToList().ForEach(error => logger.LogError(workBookName, sheet.Name, error));
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
