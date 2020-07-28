using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;

namespace CustomerTestsExcel.ExcelToCode
{
    public class InMemoryGenerateCSharpFromExcel
    {
        readonly IEnumerable<string> usings;
        readonly IEnumerable<Type> typesUnderTest;
        readonly string assertionClassPrefix;
        readonly GivenClassRecorder givenClassRecorder;
        readonly ExcelCsharpClassMatcher excelCsharpClassMatcher;
        readonly ExcelCsharpPropertyMatcher excelCsharpPropertyMatcher;
        readonly ILogger logger;
        readonly IEnumerable<ITabularBook> workbooks;
        readonly string projectRootNamespace;
        readonly List<CsharpProjectFileToSave> generatedFiles;

        public InMemoryGenerateCSharpFromExcel(
            ILogger logger,
            IEnumerable<ITabularBook> workbooks,
            string projectRootNamespace,
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
            this.usings = usings;
            this.typesUnderTest = typesUnderTest;
            this.assertionClassPrefix = assertionClassPrefix;

            generatedFiles = new List<CsharpProjectFileToSave>();
        }

        // rename GeneratedCsharpProject to generatedfiles or just have a list or sometning
        public List<CsharpProjectFileToSave> Generate()
        {
            generatedFiles.Clear();

            GenerateTestClasses();

            GenerateSpecificationSpecificSetupClasses();

            GenerateSpecificationSpecificPlaceholder();

            return generatedFiles;
        }

        void GenerateTestClasses()
        {
            foreach (var workbook in workbooks)
                OutputWorkbook(workbook);
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

                    AddFile(cSharpCode, projectRelativePath);
                }
            }

        }

        bool IsTestSheet(ITabularPage excelSheet) =>
            excelSheet.GetCell(1, 1).Value != null
            && (excelSheet.GetCell(1, 1).Value.ToString() == "Specification");

        string OutputWorkSheet(string workBookName, ITabularPage sheet)
        {
            var sheetConverter = new ExcelToCode(new CodeNameToExcelNameConverter(assertionClassPrefix));
            sheetConverter.AddVisitor(givenClassRecorder);

            var generatedTest = sheetConverter.GenerateCSharpTestCode(
                usings,
                sheet,
                projectRootNamespace,
                workBookName);

            generatedTest.Errors.ToList().ForEach(error => logger.LogWorkbookError(workBookName, sheet.Name, error));

            generatedTest.Warnings.ToList().ForEach(warning => logger.LogWarning(workBookName, sheet.Name, warning));

            generatedTest.IssuesPreventingRoundTrip.ToList().ForEach(issue => logger.LogIssuePreventingRoundTrip(workBookName, sheet.Name, issue));

            return generatedTest.Code;
        }

        void GenerateSpecificationSpecificPlaceholder()
        {
            AddFile(
                SpecificationSpecificPlaceholderGenerator.GenerateSpecificationSpecificPlaceholder(projectRootNamespace),
                Path.Combine("Setup", "Placeholder.cs"));
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
                // don't generate classes that already exist within the framework
                if (ClassNameMatcher.IsFramworkSuppliedClass(excelGivenClass.Name))
                    return;

                GenerateLeafClass(excelGivenClass);
            }
        }

        void GenerateLeafClass(GivenClass excelGivenClass)
        {
            var matchingType =
                typesUnderTest
                .Where(t => excelCsharpClassMatcher.Matches(t, excelGivenClass).Matches)
                .OrderByDescending(t => excelCsharpClassMatcher.Matches(t, excelGivenClass).PercentMatchingProperties)
                .FirstOrDefault();

            if (matchingType != null)
                GeneratedSpecificationSpecificMatchedClass(excelGivenClass, matchingType);
            else
                GeneratedSpecificationSpecificClassWithoutMatchingTypeInSut(excelGivenClass);
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
            var projectRelativePath = Path.Combine("Setup", excelGivenClass.Name);

            AddFile(code, $"{projectRelativePath}.cs");
        }

        void AddFile(string cSharpCode, string projectRelativePath)
        {
            generatedFiles.Add(
                new CsharpProjectFileToSave
                {
                    Content = cSharpCode,
                    PathRelativeToProjectRoot = projectRelativePath
                }
            );
        }
    }
}