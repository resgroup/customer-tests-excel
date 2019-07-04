using System;
using System.Collections.Generic;
using CustomerTestsExcel.CodeOutputWriters;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class ListPropertiesOutputWriter : TestBase
    {
        [Test]
        public void ListPropertiesOutputWritersOutputifListOfSyntax()
        {
            const string LIST_PROPERTY_NAME = "groups";
            const string LIST_PROPERTY_TYPE = "AnovaGroup";
            const string PROPERTY1_NAME = "School";
            const string PROPERTY2_NAME = "AverageClassSize";

            var listPropertyItems = new List<IReportsSpecificationSetup>
            {
                new TestReportsSpecificationSetup()
                    .AddValueProperty(PROPERTY1_NAME, "Langley")
                    .AddValueProperty(PROPERTY2_NAME, 30),
                new TestReportsSpecificationSetup()
                    .AddValueProperty(PROPERTY1_NAME, "Ninestiles")
                    .AddValueProperty(PROPERTY2_NAME, 20),
            };

            var givenListProperty = new ReportSpecificationSetupList(
                LIST_PROPERTY_NAME,
                LIST_PROPERTY_TYPE,
                listPropertyItems);

            var setup = new TestReportsSpecificationSetup().AddListProperty(givenListProperty);

            var runner = CreateRunner();

            runner.Run(new TestSpecification(setup));

            // This doesn't test everything is in the right order and things like that,
            // but it is more flexible in allowing whitespace changes, so probably an
            // ok compromise.
            StringAssert.Contains($"{LIST_PROPERTY_NAME} list of {LIST_PROPERTY_TYPE}", runner.Message);
            StringAssert.Contains("With Item", runner.Message);
            StringAssert.Contains($"{PROPERTY1_NAME} \"Langley\"", runner.Message);
            StringAssert.Contains($"{PROPERTY2_NAME} 30", runner.Message);
            StringAssert.Contains("With Item", runner.Message);
            StringAssert.Contains($"{PROPERTY1_NAME} \"Ninestiles\"", runner.Message);
            StringAssert.Contains($"{PROPERTY2_NAME} 20", runner.Message);
        }

        private static RunSpecification<TestReportsSpecificationSetup> CreateRunner()
        {
            var outputWriter = new StringBuilderTextLineWriter();

            var writer = new StringTestOutputWriter(new HumanFriendlyFormatter(), outputWriter);

            return new RunSpecification<TestReportsSpecificationSetup>(writer);
        }
    }
}
