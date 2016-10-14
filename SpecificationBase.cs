using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Linq.Expressions;
using System.IO;
using System.Reflection;

namespace RES.Specification
{
    [TestFixture]
    public abstract class SpecificationBase<T> : ISpecification<T> 
        where T : IReportsSpecificationSetup
    {
        protected T _sut;

        // these control what writers are used.
        // we could have an nunit plugin installed on the build server to make sure that the html output is always generated
        // I'm not sure about the best way to decide when to output back to excel. Maybe the programmer could add an attribute
        // to the test which we could check for. Or set the variable below
        protected bool _debugOutput = true;
        protected bool _htmlOutput = true;
        protected bool _excelOutput = false; // make this true to write out to excel. this should always be false when checked in, otherwise it will cause a lot of extra work on the build. It should be set to true manually by programmers when they have made some changes to a test in code and want to write these changes back out to the associated excel file.

        public abstract string Description();
        public abstract string TrunkRelativePath();
        public abstract T Given();
        public abstract string When(T sut);
        public abstract IEnumerable<IAssertion<T>> Assertions();

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            //RES.User.Stubs.StubFactoryInitializer.InitializeAll();
        }

        [Test]
        public void RunTests()
        {
            var runner = new RunSpecification<T>(GetWriter());

            bool passed = runner.Run(this);

            if (!passed) Assert.Fail(runner.Message);
        }

        protected static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private string GetTrunkPath()
        {
            return Path.Combine(new DirectoryInfo(AssemblyDirectory).Parent.Parent.FullName, TrunkRelativePath());
        }

        private ITestOutputWriter GetWriter()
        {
            // executing directory is expected to be svn\builtsdlls\debug, you need to make sure that the build path for your test projects are set to build here.
            var trunkPath = GetTrunkPath();
            var writers = new List<ITestOutputWriter>();
            if (_debugOutput) writers.Add(new StringTestOutputWriter(new HumanFriendlyFormatter(), new DebugTextLineWriter()));
            if (_htmlOutput) writers.Add(new HTMLTestOutputWriter(new HumanFriendlyFormatter()));
            if (_excelOutput) writers.Add(new ExcelTestOutputWriter(new ExcelTabularFormatLibrary(), new CodeNameToExcelNameConverter(), Path.Combine(trunkPath, @"Specification\ExcelTests")));

            ITestOutputWriter writer;
            if (writers.Count() == 0)
            {
                writer = new StringTestOutputWriter(new HumanFriendlyFormatter(), new DebugTextLineWriter()); // change this for a null object output writer
            }
            else if (writers.Count() == 1)
            {
                writer = writers.First();
            }
            else
            {
                writer = new CombinedTestOutputWriter(writers);
            }

            return writer;
        }

    }
}
