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
        const string excelTestsFolder = "ExcelTests";
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
            string specificationFolder,
            string specificationProject,
            string projectRootNamespace,
            IEnumerable<string> usings,
            IEnumerable<string> assembliesUnderTest,
            string assertionClassPrefix,
            ITabularLibrary excel)
        {
            var assemblyTypes = GetTypesUnderTest(assembliesUnderTest);

            // We could carry on here instead of returning early, but then the 
            // generated code would not be as expected, so probably best like this
            if (logger.HasErrors)
                return;

            var excelTestFilenames = ListValidSpecificationSpreadsheets();

            var projectFilePath = Path.Combine(specificationFolder, specificationProject);
            var project = OpenProjectFile(projectFilePath);

            if (logger.HasErrors)
                return;

            new TestProjectCreatorPure(logger)
                .Create(
                    specificationFolder,
                    specificationProject,
                    project,
                    excelTestFilenames,
                    projectRootNamespace,
                    usings,
                    assemblyTypes,
                    assertionClassPrefix,
                    excel
                );
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

        IEnumerable<string> ListValidSpecificationSpreadsheets()
        {
            var combinedList = new List<string>();
            combinedList.AddRange(Directory.GetFiles(excelTestsFolder, "*.xlsx"));
            combinedList.AddRange(Directory.GetFiles(excelTestsFolder, "*.xlsm"));
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
            XDocument projectFile;
            using (var projectStreamReader = new StreamReader(projectPath))
            {
                projectFile = XDocument.Load(projectStreamReader.BaseStream);
            }
            return projectFile;
        }
    }
}
