using System;
using System.Collections.Generic;
using CustomerTestsExcel.CodeOutputWriters;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    public class ListPropertiesOutputWriter : TestBase
    {
        const string LIST_PROPERTY_NAME = "groups";
        const string LIST_PROPERTY_TYPE = "AnovaGroup";
        const string PROPERTY1_NAME = "School";
        const string PROPERTY1_VALUE1 = "Langley";
        const string PROPERTY1_VALUE2 = "Ninestiles";
        const string PROPERTY2_NAME = "AverageClassSize";
        const int PROPERTY2_VALUE1 = 30;
        const int PROPERTY2_VALUE2 = 20;
        TestReportsSpecificationSetup setupWithListProperty;

        [SetUp]
        public void SetUp()
        {
            var listPropertyItems = new List<IReportsSpecificationSetup>
            {
                new TestReportsSpecificationSetup()
                    .AddValueProperty(PROPERTY1_NAME, PROPERTY1_VALUE1)
                    .AddValueProperty(PROPERTY2_NAME, PROPERTY2_VALUE1),
                new TestReportsSpecificationSetup()
                    .AddValueProperty(PROPERTY1_NAME, PROPERTY1_VALUE2)
                    .AddValueProperty(PROPERTY2_NAME, PROPERTY2_VALUE2),
            };

            var givenListProperty = new ReportSpecificationSetupList(
                LIST_PROPERTY_NAME,
                LIST_PROPERTY_TYPE,
                listPropertyItems);

            setupWithListProperty = new TestReportsSpecificationSetup().AddListProperty(givenListProperty);
        }

        [Test]
        public void RunSpecificationWithHtmlTestOutputWriterReturnsCorrectOutputForListProperties()
        {
            var htmlTestOutputWriter = new HTMLTestOutputWriter(new TestHumanFriendlyFormatter());

            var runSpecification = new RunSpecification<TestReportsSpecificationSetup>(htmlTestOutputWriter);

            runSpecification.Run(new TestSpecification(setupWithListProperty));

            // It would be better to do a more html like compare here. For example asserting that various elements
            // exist and have certain attributes etc. This is fine for now though, I might even remove the html
            // output at some point as it doesn't seem to be used much.
            StringAssert.Contains($"<div class='givenListProperty'>", htmlTestOutputWriter.Html);
            StringAssert.Contains($@"<span class='propertyName'>{LIST_PROPERTY_NAME}</span>", htmlTestOutputWriter.Html);
            StringAssert.Contains($@"<span class='propertyType code'>{LIST_PROPERTY_TYPE}</span>", htmlTestOutputWriter.Html);
            StringAssert.Contains(@"<div class='withItem'>", htmlTestOutputWriter.Html);
            StringAssert.Contains(@"<div>With Item</div>", htmlTestOutputWriter.Html);
            StringAssert.Contains($@"<span class='propertyName'>{PROPERTY1_NAME}</span> <span class='propertyValue code'>{PROPERTY1_VALUE1}</span>", htmlTestOutputWriter.Html);
            StringAssert.Contains($@"<span class='propertyName'>{PROPERTY2_NAME}</span> <span class='propertyValue code'>{PROPERTY2_VALUE1}</span>", htmlTestOutputWriter.Html);
            StringAssert.Contains($@"<span class='propertyName'>{PROPERTY1_NAME}</span> <span class='propertyValue code'>{PROPERTY1_VALUE2}</span>", htmlTestOutputWriter.Html);
            StringAssert.Contains($@"<span class='propertyName'>{PROPERTY2_NAME}</span> <span class='propertyValue code'>{PROPERTY2_VALUE2}</span>", htmlTestOutputWriter.Html);
        }

        [Test]
        public void RunSpecificationWithStringTestOutputWriterReturnsCorrectOutputForListProperties()
        {
            var runSpecification = new RunSpecification<TestReportsSpecificationSetup>();

            runSpecification.Run(new TestSpecification(setupWithListProperty));

            // This doesn't test everything is in the right order and things like that,
            // but it is more flexible in allowing whitespace changes, so probably an
            // ok compromise.
            StringAssert.Contains($"{LIST_PROPERTY_NAME} list of {LIST_PROPERTY_TYPE}", runSpecification.Message);
            StringAssert.Contains("With Item", runSpecification.Message);
            StringAssert.Contains($"{PROPERTY1_NAME} \"{PROPERTY1_VALUE1}\"", runSpecification.Message);
            StringAssert.Contains($"{PROPERTY2_NAME} {PROPERTY2_VALUE1}", runSpecification.Message);
            StringAssert.Contains("With Item", runSpecification.Message);
            StringAssert.Contains($"{PROPERTY1_NAME} \"{PROPERTY1_VALUE2}\"", runSpecification.Message);
            StringAssert.Contains($"{PROPERTY2_NAME} {PROPERTY2_VALUE2}", runSpecification.Message);
        }

    }
}
