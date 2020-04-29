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
        string excelTestsFolderName;
        //readonly GivenClassRecorder givenClassRecorder;
        //readonly ExcelCsharpClassMatcher excelCsharpClassMatcher;
        //readonly ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher;
        readonly ILogger logger;
        //bool success;

        public TestProjectCreator(
            ILogger logger)
        {
            //givenClassRecorder = new GivenClassRecorder();
            //excelCsharpPropertyMatcher = new ExcelCsharpPropertyMatcher();
            //excelCsharpClassMatcher = new ExcelCsharpClassMatcher(excelCsharpPropertyMatcher);

            this.logger = logger;
        }

        public void Create(
            string projectRootFolder,
            string specificationProject,
            string projectRootNamespace,
            string excelTestsFolderName,
            IEnumerable<string> usings,
            IEnumerable<string> assembliesUnderTest,
            string assertionClassPrefix,
            ITabularLibrary excel)
        {
            this.excelTestsFolderName = excelTestsFolderName;
            var assemblyTypes = GetTypesUnderTest(assembliesUnderTest);

            // We could carry on here instead of returning early, but then the 
            // generated code would not be as expected, so probably best like this
            if (logger.HasErrors)
                return;

            var excelTestFilenames = ListValidSpecificationSpreadsheets(projectRootFolder);

            var projectFilePath = Path.Combine(projectRootFolder, specificationProject);
            var existingCsproj = OpenProjectFile(projectFilePath);

            if (logger.HasErrors)
                return;

            var generatedProject = new TestProjectCreatorPure(logger)
                .Create(
                    projectRootFolder,
                    existingCsproj,
                    excelTestFilenames,
                    projectRootNamespace,
                    excelTestsFolderName,
                    usings,
                    assemblyTypes,
                    assertionClassPrefix,
                    excel
                );

            SaveProjectFile(projectFilePath, generatedProject.CsprojFile);

            SaveFiles(projectRootFolder, generatedProject.Files);
        }

        void SaveFiles(string projectRootFolder, List<FileToSave> files)
        {

        }

        void SaveFile(string projectRootFolder, FileToSave file)
        {
            try
            {
                TrySaveFile(projectRootFolder, file);
            }
            catch (Exception exception)
            {
                logger.LogFileSaveError(projectRootFolder, exception);
            }
        }

        void TrySaveFile(string projectRootFolder, FileToSave file)
        {
            File.WriteAllText(
                Path.Combine(projectRootFolder, file.PathRelativeToProjectRoot),
                file.Content);
        }

        List<Type> GetTypesUnderTest(IEnumerable<string> assembliesUnderTest)
        {
            return
                assembliesUnderTest.Aggregate(
                    new List<Type>(),
                    (assemblyTypes, assemblyFilename) => assemblyTypes.Concat(GetTypesFromAssembly(assemblyFilename)).ToList());
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

        IEnumerable<string> ListValidSpecificationSpreadsheets(string specificationFolder)
        {
            var excelTestsPath = Path.Combine(specificationFolder, excelTestsFolderName);
            var combinedList = new List<string>();
            combinedList.AddRange(Directory.GetFiles(excelTestsPath, "*.xlsx"));
            combinedList.AddRange(Directory.GetFiles(excelTestsPath, "*.xlsm"));
            combinedList = combinedList.Where(f => !f.Contains("~$")).ToList(); // these are temporary files created by excel when the main file is open.
            return combinedList;
        }

        XDocument OpenProjectFile(string projectPath)
        {
            try
            {
                return TryOpenProjectFile(projectPath);
            }
            catch (Exception exception)
            {
                logger.LogCsprojLoadError(projectPath, exception);
                return new XDocument();
            }
        }

        XDocument TryOpenProjectFile(string projectPath)
        {
            using (var projectStreamReader = new StreamReader(projectPath))
                return XDocument.Load(projectStreamReader.BaseStream);
        }

        void SaveProjectFile(string projectPath, XDocument projectFile)
        {
            try
            {
                TrySaveProjectFile(projectPath, projectFile);
            }
            catch (Exception exception)
            {
                logger.LogCsprojSaveError(projectPath, exception);
            }
        }

        void TrySaveProjectFile(string projectPath, XDocument projectFile)
        {
            using (var projectStreamWriter = new StreamWriter(projectPath))
                projectFile.Save(projectStreamWriter.BaseStream);
        }

    }
}
