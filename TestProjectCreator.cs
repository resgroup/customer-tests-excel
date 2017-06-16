using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace CustomerTestsExcel
{
    public static class TestProjectCreator
    {
        private static XNamespace xNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";

        public static void Create(string specificationFolder, string specificationProject, string projectRootNamespace, IEnumerable<string> usings, string assertionClassPrefix, ITabularLibrary excel)
        {
            var projectFilePath = Path.Combine(specificationFolder, specificationProject);

            var project = OpenProjectFile(projectFilePath);
            var compileItemGroupNode = GetItemGroupForCompileNodes(project);
            var excelItemGroupNode = GetItemGroupForExcelNodes(project);

            string excelFolder = Path.Combine(specificationFolder, "ExcelTests");
            foreach (var excelFileName in ListValidSpecificationSpreadsheets(excelFolder))
            {
                excelItemGroupNode.Add(MakeFileElement("None",Path.Combine("ExcelTests", Path.GetFileName(excelFileName))));
                OutputWorkbook(specificationFolder, projectRootNamespace, usings, assertionClassPrefix, excel, compileItemGroupNode, excelFileName);
            }

            SaveProjectFile(projectFilePath, project);
        }

        private static IEnumerable<string> ListValidSpecificationSpreadsheets(string excelFolder)
        {
            var combinedList = new List<string>();
            combinedList.AddRange(Directory.GetFiles(excelFolder, "*.xlsx"));
            combinedList.AddRange(Directory.GetFiles(excelFolder, "*.xlsm"));
            combinedList = combinedList.Where(f => !f.Contains("~$")).ToList(); // these are temporary files created by excel when the main file is open.
            return combinedList;
        }

        private static XDocument OpenProjectFile(string projectPath)
        {
            XDocument projectFile;
            using (var projectStreamReader = new StreamReader(projectPath))
            {
                projectFile = XDocument.Load(projectStreamReader.BaseStream);
            }
            return projectFile;
        }

        private static void SaveProjectFile(string projectPath, XDocument projectFile)
        {
            using (var projectStreamWriter = new StreamWriter(projectPath))
            {
                projectFile.Save(projectStreamWriter.BaseStream);
            }
        }

        private static XElement GetItemGroupForCompileNodes(XDocument projectFile)
        {
            var compileItemGroupNode = GetItemGroupForNodeTypes(projectFile, "Compile"); ;
            compileItemGroupNode.Add(MakeFileElement("Compile", @"Properties\AssemblyInfo.cs"));
            return compileItemGroupNode;
        }

        private static XElement GetItemGroupForExcelNodes(XDocument projectFile)
        {
            return GetItemGroupForNodeTypes(projectFile, "None");
        }

        private static XElement GetItemGroupForNodeTypes(XDocument projectFile, string nodeName)
        {
            var compileNodes = projectFile.Descendants(xNamespace + nodeName);
            XElement compileItemGroupNode;
            if (compileNodes.Any())
            {
                compileItemGroupNode = projectFile.Descendants(xNamespace + nodeName).First().Parent;
                // remove all code except things in the protected "IgnoreOnGeneration" folder, which is kept for non generated things
                compileItemGroupNode.Elements().Where(e => e.Attribute("Include")?.Value?.StartsWith("IgnoreOnGeneration\\") != true).Remove();
            }
            else
            {

                compileItemGroupNode = new XElement(xNamespace + "ItemGroup");
                projectFile.Root.Add(compileItemGroupNode);
            }
            return compileItemGroupNode;
        }

        private static XElement MakeFileElement(string noteName, string relativeFilePath)
        {
            return new XElement(xNamespace + noteName, new XAttribute("Include", relativeFilePath));
        }

        private static void OutputWorkbook(string specificationFolder, string projectRootNamespace, IEnumerable<string> usings, string assertionClassPrefix, ITabularLibrary excel, XElement compileItemGroupNode, string excelFileName)
        {
            using (var workbookFile = GetExcelFileStream(excelFileName))
            {
                using (var workbook = excel.OpenBook(workbookFile))
                {
                    var workBookName = Path.GetFileNameWithoutExtension(excelFileName);
                    for (int i = 0; i < workbook.NumberOfPages; i++)
                        if (IsTestSheet(workbook.GetPage(i)))
                        {
                            compileItemGroupNode.Add(MakeFileElement("Compile", OutputWorkSheet(specificationFolder, usings, assertionClassPrefix, workBookName, workbook.GetPage(i), projectRootNamespace)));
                        }
                }
            }
        }

        static Stream GetExcelFileStream(string excelFile, string temporaryFolder = null)
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

        private static bool IsTestSheet(ITabularPage excelSheet)
        {
            return excelSheet.GetCell(1, 1).Value != null ? (excelSheet.GetCell(1, 1).Value.ToString() == "Specification") : false;
        }

        private static string OutputWorkSheet(string outputFolder, IEnumerable<string> usings, string assertionClassPrefix, string workBookName, ITabularPage sheet, string projectRootNamespace)
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter());

            var projectRelativePath = Path.Combine(workBookName, sheet.Name + ".cs");
            var outputPath = Path.Combine(outputFolder, projectRelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            using (var outputFile = new StreamWriter(outputPath))
            {
                try
                {
                    outputFile.Write(sheetConverter.GenerateCSharpTestCode(usings, assertionClassPrefix, sheet, projectRootNamespace, workBookName));
                }
                catch (Exception ex)
                {
                    outputFile.Write(string.Format("Error creating c# from Excel: {0}", ex.Message));
                }
            }
            return projectRelativePath;
        }
    }
}
