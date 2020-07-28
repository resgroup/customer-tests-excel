using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using static System.Environment;

namespace CustomerTestsExcel.ExcelToCode
{
    public class FileSystemGenerateCSharpFromExcel
    {
        readonly string excelTestsFolderName;
        readonly IEnumerable<string> usings;
        readonly IEnumerable<string> assembliesUnderTest;
        readonly string assertionClassPrefix;
        readonly ITabularLibrary excel;
        //readonly string projectFilePath;
        readonly ILogger logger;
        private readonly string projectRootFolder;
        readonly string projectRootNamespace;

        public FileSystemGenerateCSharpFromExcel(
            ILogger logger,
            string projectRootFolder,
            //string specificationProject,
            string projectRootNamespace,
            string excelTestsFolderName,
            IEnumerable<string> usings,
            IEnumerable<string> assembliesUnderTest,
            string assertionClassPrefix,
            ITabularLibrary excel)
        {
            this.logger = logger;
            this.projectRootFolder = projectRootFolder;
            this.projectRootNamespace = projectRootNamespace;
            this.excelTestsFolderName = excelTestsFolderName;
            this.usings = usings;
            this.assembliesUnderTest = assembliesUnderTest;
            this.assertionClassPrefix = assertionClassPrefix;
            this.excel = excel;
            //projectFilePath = Path.Combine(projectRootFolder, specificationProject);
        }

        public void Create()
        {
            var assemblyTypes = GetTypesUnderTest();

            // We could carry on here instead of returning early, but then the 
            // generated code would not be as expected, so probably best like this
            if (logger.HasErrors)
                return;

            //var existingCsproj = OpenProjectFile();

            //if (logger.HasErrors)
            //    return;

            var excelFilenames = ListValidSpecificationSpreadsheets();

            GeneratedCsharpProject inMemoryGeneratedFiles;

            using (var tidyUp = OpenExcelFilesAndAutoClose(excelFilenames))
            {
                if (logger.HasErrors)
                    return;

                inMemoryGeneratedFiles = GenerateInMemory(
                    assemblyTypes, 
                    //existingCsproj, 
                    tidyUp.RememberedThing.Select(e => e.ExcelWorkbook)
                );
            }

            //SaveProjectFile(inMemoryGeneratedFiles.CsprojFile);

            SaveFiles(inMemoryGeneratedFiles.Files);
        }

        List<Type> GetTypesUnderTest()
        {
            return
                assembliesUnderTest.Aggregate(
                    new List<Type>(),
                    (assemblyTypes, assemblyFilename) => assemblyTypes.Concat(GetTypesFromAssembly(assemblyFilename)).ToList());
        }

        IEnumerable<Type> GetTypesFromAssembly(string assemblyFilename)
        {
            string assemblyPath = Path.GetDirectoryName(assemblyFilename);
            ResolveEventHandler resolver = (object sender, ResolveEventArgs args) => CurrentDomain_AssemblyResolve(assemblyPath, args);

            AppDomain.CurrentDomain.AssemblyResolve += resolver;
            try
            {

                try
                {
                    return GetLoadableTypes(Assembly.LoadFile(assemblyFilename));
                }
                catch (ReflectionTypeLoadException exception)
                {
                    logger.LogAssemblyError(assemblyFilename, ReflectionTypeLoadExceptionDetails(exception), exception);
                    return new List<Type>();
                }
                catch (Exception exception)
                {
                    logger.LogAssemblyError(assemblyFilename, "", exception);
                    return new List<Type>();
                }
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= resolver;
            }
        }

        static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        Assembly CurrentDomain_AssemblyResolve(string assemblyPath, ResolveEventArgs args)
        {
            // Ignore missing resources
            if (args.Name.Contains(".resources"))
                return null;

            // check for assemblies already loaded
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
                return assembly;

            // Try to load by filename - split out the filename of the full assembly name
            // and append the base path of the original assembly (ie. look in the same dir)
            string filename = args.Name.Split(',')[0] + ".dll".ToLower();

            string asmFile = Path.Combine(@".\", assemblyPath, filename);

            try
            {
                return System.Reflection.Assembly.LoadFrom(asmFile);
            }
            catch (ReflectionTypeLoadException exception)
            {
                logger.LogAssemblyError(filename, ReflectionTypeLoadExceptionDetails(exception), exception);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        static string ReflectionTypeLoadExceptionDetails(ReflectionTypeLoadException exception)
        {
            var problems = new List<string>();
            foreach (var subException in exception.LoaderExceptions)
            {
                var problem = subException.Message;
                if (subException is FileNotFoundException fileNotFoundException)
                {
                    if (!string.IsNullOrEmpty(fileNotFoundException.FusionLog))
                    {
                        problem += $"Fusion Log:{NewLine}{fileNotFoundException.FusionLog}";
                    }
                }
                problems.Add(problem);
            }

            var uniqueProblems = problems.Distinct();
            if (uniqueProblems.Any())
                return $"Details from Loader exceptions:{NewLine}{string.Join(NewLine, uniqueProblems)}";
            else
                return "";
        }

        //XDocument OpenProjectFile()
        //{
        //    try
        //    {
        //        return TryOpenProjectFile(projectFilePath);
        //    }
        //    catch (Exception exception)
        //    {
        //        logger.LogCsprojLoadError(projectFilePath, exception);
        //        return new XDocument();
        //    }
        //}

        //XDocument TryOpenProjectFile(string projectPath)
        //{
        //    using (var projectStreamReader = new StreamReader(projectPath))
        //        return XDocument.Load(projectStreamReader.BaseStream);
        //}

        IEnumerable<string> ListValidSpecificationSpreadsheets()
        {
            var excelTestsPath = Path.Combine(projectRootFolder, excelTestsFolderName);
            var combinedList = new List<string>();
            combinedList.AddRange(Directory.GetFiles(excelTestsPath, "*.xlsx"));
            combinedList.AddRange(Directory.GetFiles(excelTestsPath, "*.xlsm"));
            combinedList = combinedList.Where(f => !f.Contains("~$")).ToList(); // these are temporary files created by excel when the main file is open.
            return combinedList;
        }

        TidyUpWithRemember<IEnumerable<ExcelFileIo>> OpenExcelFilesAndAutoClose(IEnumerable<string> excelFilenames)
        {
            return 
                new TidyUpWithRemember<IEnumerable<ExcelFileIo>>(
                    () => OpenExcelFiles(excelFilenames),
                    CloseExcelFiles
                );
        }

        IEnumerable<ExcelFileIo> OpenExcelFiles(IEnumerable<string> excelWorkbookFilename) =>
            excelWorkbookFilename
            .Select(OpenExcelFile);

        ExcelFileIo OpenExcelFile(string excelWorkbookFilename)
        {
            try
            {
                return TryOpenExcelFile(excelWorkbookFilename);
            }
            catch (Exception exception)
            {
                logger.LogExcelFileLoadError(excelWorkbookFilename, exception);
                return new ExcelFileIo();
            }
        }

        ExcelFileIo TryOpenExcelFile(string excelWorkbookFilename)
        {
            var stream = GetExcelFileStream(excelWorkbookFilename);

            return
                new ExcelFileIo
                {
                    Stream = stream,
                    ExcelWorkbook = OpenWorkbook(stream, excelWorkbookFilename, excel)
                };
        }

        ITabularBook OpenWorkbook(Stream stream, string filename, ITabularLibrary excel)
        {
            var workbook = excel.OpenBook(stream);
            workbook.Filename = filename;
            return workbook;
        }

        static void CloseExcelFiles(IEnumerable<ExcelFileIo> excelFileIos) =>
            excelFileIos
            .Select(excelFileIo => excelFileIo.Stream)
            .ToList()
            .ForEach(stream => stream.Dispose());


        Stream GetExcelFileStream(string excelFilename, string temporaryFolder = null)
        {
            var templateStream = new MemoryStream();

            // Excel rather stupidly locks files (even for reading) when it has them open.
            // To avoid errors in this case, we copy the file to a temporary location and
            // open from there.
            if (string.IsNullOrEmpty(temporaryFolder))
                temporaryFolder = Path.GetTempPath();

            var tempFileName = Path.Combine(temporaryFolder, Guid.NewGuid().ToString("N") + ".tmp");

            File.Copy(excelFilename, tempFileName, true);
            try
            {
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

        GeneratedCsharpProject GenerateInMemory(
            List<Type> assemblyTypes, 
            //XDocument existingCsproj, 
            IEnumerable<ITabularBook> excelWorkbooks)
        {
            return
                new InMemoryGenerateCSharpFromExcel(
                    logger,
                    //existingCsproj,
                    excelWorkbooks,
                    projectRootNamespace,
                    excelTestsFolderName,
                    usings,
                    assemblyTypes,
                    assertionClassPrefix)
                .Generate();
        }


        //void SaveProjectFile(XDocument projectFile)
        //{
        //    try
        //    {
        //        TrySaveProjectFile(projectFilePath, projectFile);
        //    }
        //    catch (Exception exception)
        //    {
        //        logger.LogCsprojSaveError(projectFilePath, exception);
        //    }
        //}

        //void TrySaveProjectFile(string projectPath, XDocument projectFile)
        //{
        //    using (var projectStreamWriter = new StreamWriter(projectPath))
        //        projectFile.Save(projectStreamWriter.BaseStream);
        //}

        void SaveFiles(List<CsharpProjectFileToSave> files) =>
            files.ForEach(file => SaveFile(projectRootFolder, file));

        void SaveFile(string projectRootFolder, CsharpProjectFileToSave file)
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

        void TrySaveFile(string projectRootFolder, CsharpProjectFileToSave file)
        {
            var filename = Path.Combine(projectRootFolder, file.PathRelativeToProjectRoot);

            Directory.CreateDirectory(Path.GetDirectoryName(filename));

            if (File.Exists("Override" + filename))
            {
                if (File.Exists(filename))
                    File.Delete(filename);

                File.WriteAllText(
                    filename + ".txt",
                    file.Content);
            }
            else
            {
                File.WriteAllText(
                    filename,
                    file.Content);
            }
        }

    }
}
