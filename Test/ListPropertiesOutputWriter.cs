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

            var listPropertyItem1 = new TestReportsSpecificationSetup();
            listPropertyItem1.AddValueProperty("School", "Langley");
            listPropertyItem1.AddValueProperty("AverageClassSize", 30);

            var listPropertyItem2 = new TestReportsSpecificationSetup();
            listPropertyItem2.AddValueProperty("School", "Ninestiles");
            listPropertyItem2.AddValueProperty("AverageClassSize", 20);

            var listPropertyItems = new List<IReportsSpecificationSetup>
            {
                listPropertyItem1,
                listPropertyItem2
            };

            var givenListProperty = new ReportSpecificationSetupList(
                "groups",
                "AnovaGroup",
                listPropertyItems);

            var givenObject = new TestReportsSpecificationSetup();
            givenObject.AddListProperty(givenListProperty);

            var specification = new TestSpecification(givenObject);

            var outputWriter = new StringBuilderTextLineWriter();
            var writer = new StringTestOutputWriter(new HumanFriendlyFormatter(), outputWriter);

            var runner = new RunSpecification<TestReportsSpecificationSetup>(writer);

            runner.Run(specification);

            var output = outputWriter.StringBuilder.ToString();

            StringAssert.Contains("groups list of AnovaGroup", output);
        }
    }
}
