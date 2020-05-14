using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerTestsExcel.ExcelToCode
{
    public class ExcelToCode : ExcelToCodeBase
    {
        public ExcelToCode(ICodeNameToExcelNameConverter converter)
            : base(new ExcelToCodeState(converter))
        { }

        // This function also returns some data in the Errors, Warnings and 
        // similar properties. It would be better to wrap all this in to 
        // a new type, and return that type here. Or to take an ILogger
        // and use this to communicate the errors and warnings.
        public string GenerateCSharpTestCode(
            IEnumerable<string> usings,
            ITabularPage worksheet,
            string projectRootNamespace,
            string workBookName)
        {
            try
            {
                return TryGenerateCSharpTestCode(
                    usings,
                    worksheet,
                    projectRootNamespace,
                    workBookName);
            }
            catch (ExcelToCodeException exception)
            {
                log.AddError(exception.Message);
                return $"// {exception.Message}";
            }
            catch (Exception exception)
            {
                string message = $"Unable to convert worksheet due to an unexpected internal error:\n{exception.ToString()}";
                log.AddError(message);
                return $"// {message}";
            }
        }

        string TryGenerateCSharpTestCode(
            IEnumerable<string> usings,
            ITabularPage worksheet,
            string projectRootNamespace,
            string workBookName)
        {
            Initialise(worksheet);

            var sutName = ReadSutName();
            var description = ReadDescription();

            DoHeaders(
                sutName,
                usings, 
                description, 
                projectRootNamespace, 
                workBookName);

            DoGiven(sutName);
            excel.MoveDown();
            CheckExactlyOneBlankLineBetweenGivenAndWhen();

            DoWhen(sutName);
            excel.MoveDown();
            // Looks like there should be a CheckExactlyOneBlankLineBetweenWhenAndThen() function

            DoThen(sutName);

            DoFooters();

            return code.GeneratedCode;
        }

        string ReadSutName()
        {
            using (excel.SavePosition())
            {
                excel.MoveDownToToken(converter.Given);

                return excel.PeekRight();
            }
        }

        string ReadDescription()
        {
            using (excel.SavePosition())
            {
                excel.MoveDownToToken(converter.Specification);

                return excel.PeekRight();
            }
        }

        void Initialise(ITabularPage worksheet) =>
            excelToCodeState.Initialise(worksheet);

        void DoHeaders(
            string sutName,
            IEnumerable<string> usings, 
            string description, 
            string projectRootNamespace, 
            string workBookName)
        {
            code.Add("using System;");
            code.Add("using System.Collections.Generic;");
            code.Add("using System.Linq;");
            code.Add("using System.Text;");
            code.Add("using NUnit.Framework;");
            code.Add("using CustomerTestsExcel;");
            code.Add("using CustomerTestsExcel.Assertions;");
            code.Add("using CustomerTestsExcel.SpecificationSpecificClassGeneration;");
            code.Add("using System.Linq.Expressions;");
            code.Add($"using {projectRootNamespace};");
            code.Add($"using {projectRootNamespace}.GeneratedSpecificationSpecific;");
            code.BlankLine();
            foreach (var usingNamespace in usings)
                code.Add($"using {usingNamespace};");
            code.BlankLine();
            code.Add($"namespace {projectRootNamespace}.{converter.ExcelFileNameToCodeNamespacePart(workBookName)}");
            code.Add("{");
            code.Add("[TestFixture]");
            code.Add($"public class {converter.ExcelSpecificationNameToCodeSpecificationClassName(excel.Worksheet.Name)} : SpecificationBase<{CSharpSUTSpecificationSpecificClassName(sutName)}>, ISpecification<{CSharpSUTSpecificationSpecificClassName(sutName)}>");
            code.Add("{");
            code.Add("public override string Description()");
            code.Add("{");
            code.Add($"return \"{description}\";");
            code.Add("}");
        }

        void DoGiven(string sutName) =>
            excelToCodeState.Given.DoGiven(sutName);

        void CheckExactlyOneBlankLineBetweenGivenAndWhen()
        {
            uint endOfGiven = excel.Row;
            uint startOfWhen;

            using (excel.SavePosition())
            {
                excel.MoveDownToToken(converter.When);
                startOfWhen = excel.Row;
            }

            if (startOfWhen - endOfGiven <= 1)
                log.AddIssuePreventingRoundTrip($"There is no blank line between the end of the Given section (Row {endOfGiven}) and the start of the When section (Row {startOfWhen}) in the Excel test, worksheet '{excel.Worksheet.Name}'");
            else if (startOfWhen - endOfGiven > 2)
                log.AddIssuePreventingRoundTrip($"There should be exactly one blank line, but there are {startOfWhen - endOfGiven - 1}, between the end of the Given section (Row {endOfGiven}) and the start of the When section (Row {startOfWhen}) in the Excel test, worksheet '{excel.Worksheet.Name}'");
        }

        void DoWhen(string sutName) =>
            excelToCodeState.When.DoWhen(sutName);

        void DoThen(string sutName) => 
            excelToCodeState.Then.DoThen(sutName);

        void DoFooters()
        {
            code.Add("};");
            code.Add("}");
            // This is so that when writing back out to excel, the prefix can be removed. So the prefix exists in the code, but not in excel, and is round trippable
            code.BlankLine();
            code.Add($"protected override string AssertionClassPrefixAddedByGenerator => \"{converter.AssertionClassPrefixAddedByGenerator}\";");
            OutputErrors();
            OutputWarnings();
            OutputRoundTripIssues();
            code.Add("}");
            code.Add("}");
        }

        void OutputErrors()
        {
            if (log.Errors.Any())
            {
                code.BlankLine();
                log.Errors.ToList().ForEach(error => code.Add($"// {error}"));
            }
        }

        void OutputWarnings()
        {
            if (log.Warnings.Any())
            {
                code.BlankLine();
                log.Warnings.ToList().ForEach(warning => code.Add($"// {warning}"));
                log.Warnings.ToList().ForEach(warning => code.Add($"// {warning}"));
            }
        }

        void OutputRoundTripIssues()
        {
            if (!log.IssuesPreventingRoundTrip.Any())
                return;

            code.BlankLine();
            code.Add($"protected override bool RoundTrippable() => false;");
            code.BlankLine();
            code.Add("protected override IEnumerable<string> IssuesPreventingRoundTrip() => new List<string>");
            code.Add("{");
            code.Add(
                string.Join(
                    "," + Environment.NewLine,
                    log.IssuesPreventingRoundTrip.Select(issue => "\"" + issue + "\"")
                )
            );
            code.Add("};");
        }

    }
}
