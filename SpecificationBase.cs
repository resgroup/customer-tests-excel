using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Linq.Expressions;
using System.IO;
using System.Reflection;
using CustomerTestsExcel.CodeOutputWriters;
using CustomerTestsExcel.Assertions;

namespace CustomerTestsExcel
{
    [TestFixture]
    public abstract class SpecificationBase<T> : ISpecification<T>
        where T : IReportsSpecificationSetup
    {
        protected T _sut;
        protected abstract string AssertionClassPrefixAddedByGenerator { get; }
        protected virtual bool RoundTrippable() => true;
        protected virtual IEnumerable<string> IssuesPreventingRoundTrip() => new List<string>();

        // these control what writers are used.
        protected bool _debugOutput = true;
        protected bool _htmlOutput = false;
        protected bool _excelOutput = false; // This can be set to true manually by programmers (on a per test basis) when they have made some changes to a test in code and want to write these changes back out to the associated excel file.

        public abstract string Description();
        public abstract T Given();
        public abstract string When(T sut);
        public abstract IEnumerable<IAssertion<T>> Assertions();

        [Test]
        public virtual void RunTests()
        {
            var runner = new RunSpecification<T>(GetWriter());

            bool passed = runner.Run(this);

            if (!passed) Assert.Fail(runner.Message);
        }

        protected string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        ITestOutputWriter GetWriter()
        {
            var writers = new List<ITestOutputWriter>();

            if (_debugOutput)
                writers.Add(
                    new StringTestOutputWriter(
                        new HumanFriendlyFormatter(),
                        new DebugTextLineWriter()));

            if (_htmlOutput)
                writers.Add(
                    new HTMLTestOutputWriter(
                        new HumanFriendlyFormatter()));

            if (ExcelOutput)
            {
                if (RoundTrippable())
                    writers.Add(
                        new ExcelTestOutputWriter(
                            new ExcelTabularLibrary(),
                            new CodeNameToExcelNameConverter(AssertionClassPrefixAddedByGenerator),
                            Environment.GetEnvironmentVariable("CUSTOMER_TESTS_RELATIVE_PATH_TO_EXCELTESTS") ?? @"..\..\ExcelTests"));
                else
                    writers.Add(
                        new ExcelUnRoundTrippableTestOutputWriter(
                            new ExcelTabularLibrary(),
                            new CodeNameToExcelNameConverter(AssertionClassPrefixAddedByGenerator),
                            Environment.GetEnvironmentVariable("CUSTOMER_TESTS_RELATIVE_PATH_TO_EXCELTESTS") ?? @"..\..\ExcelTests",
                            IssuesPreventingRoundTrip()));
            }

            if (writers.Count > 1)
            {
                return new CombinedTestOutputWriter(writers);
            }
            else if (writers.Count == 1)
            {
                return writers.First();
            }
            else
            {
                return new StringTestOutputWriter(new HumanFriendlyFormatter(), new DebugTextLineWriter()); // change this for a null object output writer
            }
        }

        bool ExcelOutput =>
            _excelOutput ||
            Environment.GetEnvironmentVariable("CUSTOMER_TESTS_EXCEL_WRITE_TO_EXCEL")?.ToLowerInvariant() == "true";

    }
}
