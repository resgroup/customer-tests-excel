using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace RES.Specification
{
    public static class TestProjectCreator
    {
        private static XNamespace xNamespace = "http://schemas.microsoft.com/developer/msbuild/2003";

        public static void Create(string specificationFolder, string projectRooNamespace, IExcelApplication excel)
        {
            var specificationProjectNamespace = projectRooNamespace + ".Specification";
            var projectFileName = specificationProjectNamespace + ".csproj";
            var projectFilePath = Path.Combine(specificationFolder, projectFileName);

            var project = OpenProjectFile(projectFilePath);
            var compileItemGroupNode = GetItemGroupForCompileNodes(project);
            var excelItemGroupNode = GetItemGroupForExcelNodes(project);

            string excelFolder = Path.Combine(specificationFolder, "ExcelTests");
            foreach (var excelFileName in ListValidSpecificationSpreadsheets(excelFolder))
            {
                excelItemGroupNode.Add(MakeFileElement("None",Path.Combine("ExcelTests", Path.GetFileName(excelFileName))));
                OutputWorkbook(specificationFolder, projectRooNamespace, excel, compileItemGroupNode, excelFileName);
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
                compileItemGroupNode.RemoveAll();
            }
            else
            {

                compileItemGroupNode = new XElement(xNamespace + "ItemGroup");
                projectFile.Root.Add(compileItemGroupNode);
            }
            return compileItemGroupNode;
        }

        private static XElement GetNodeForCompileNodes(XDocument projectFile)
        {
            var compileItemGroupNode = projectFile.Descendants(xNamespace + "Compile").First().Parent;
            compileItemGroupNode.RemoveAll();
            compileItemGroupNode.Add(MakeFileElement("Compile", @"Properties\AssemblyInfo.cs"));
            return compileItemGroupNode;
        }

        private static XElement MakeFileElement(string noteName, string relativeFilePath)
        {
            return new XElement(xNamespace + noteName, new XAttribute("Include", relativeFilePath));
        }

        private static void OutputWorkbook(string specificationFolder, string projectRooNamespace, IExcelApplication excel, XElement compileItemGroupNode, string excelFileName)
        {
            using (var workbookFile = GetExcelFileStream(excelFileName))
            {
                using (var workbook = excel.OpenWorkBook(workbookFile))
                {
                    var workBookName = Path.GetFileNameWithoutExtension(excelFileName);
                    for (int i = 0; i < workbook.NumberOfWorkSheets; i++)
                        if (IsTestSheet(workbook.GetWorkSheet(i)))
                        {
                            compileItemGroupNode.Add(MakeFileElement("Compile", OutputWorkSheet(specificationFolder, workBookName, workbook.GetWorkSheet(i), projectRooNamespace)));
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

        private static bool IsTestSheet(IExcelWorksheet excelSheet)
        {
            return excelSheet.GetCell(1, 1).Value != null ? (excelSheet.GetCell(1, 1).Value.ToString() == "Specification") : false;
        }

        private static string OutputWorkSheet(string outputFolder, string workBookName, IExcelWorksheet sheet, string projectRootNamespace)
        {
            var sheetConverter = new ExcelToCode.ExcelToCode(new CodeNameToExcelNameConverter());
            var sheetName = sheet.Name;

            var projectRelativePath = Path.Combine(workBookName, sheet.Name + ".cs");
            var outputPath = Path.Combine(outputFolder, projectRelativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            using (var outputFile = new StreamWriter(outputPath))
            {
                try
                {
                    outputFile.Write(sheetConverter.GenerateCSharpTestCode(sheet, projectRootNamespace, workBookName));
                }
                catch (Exception ex)
                {
                    outputFile.Write(string.Format("Error creating c# from Excel: {0}", ex.Message));
                }
            }
            return projectRelativePath;
        }

        // TODO : add all the required usings for the project under test and associated projects
        private static string CreateUsings(string projectRooNamespace)
        {
            return  "using System;" + Environment.NewLine +
                    "using System.Collections.Generic;" + Environment.NewLine +
                    "using System.Linq;" + Environment.NewLine +
                    "using System.Text;" + Environment.NewLine +
                    "using NUnit.Framework;" + Environment.NewLine +
                    "using RES.Specification;" + Environment.NewLine +
                    "using System.Linq.Expressions;" + Environment.NewLine +
                    "using " + projectRooNamespace + ".Specification.Setup;" + Environment.NewLine
                    ;
        }
    }
}
