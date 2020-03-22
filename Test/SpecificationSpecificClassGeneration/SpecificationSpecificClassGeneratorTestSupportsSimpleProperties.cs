using CustomerTestsExcel.ExcelToCode;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CustomerTestsExcel.Test.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificClassGeneratorTestSupportsSimpleProperties : TestBase
    {
        interface ITarget
        {
            int IntegerProperty { get; }
            float FloatProperty { get; set; }
            string StringProperty { get; set; }
            DateTime DateTimeProperty { get; set; }
        }

        [Test]
        public void SupportsSimpleProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassSimpleProperty("IntegerProperty", ExcelPropertyType.Number),
                new GivenClassSimpleProperty("FloatProperty", ExcelPropertyType.Number),
                new GivenClassSimpleProperty("StringProperty", ExcelPropertyType.String),
                new GivenClassSimpleProperty("DateTimeProperty", ExcelPropertyType.DateTime)
            );

            var actual = new SpecificationSpecificClassGenerator(
                new ExcelCsharpPropertyMatcher()
                ).cSharpCode(
                    "SampleTests",
                    new List<string> { "SampleSystemUnderTest.VermeulenNearWakeLength" },
                    typeof(ITarget),
                    excelGivenClass
                );

            var expected =
@"using System;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.MethodBase;
using Moq;
using CustomerTestsExcel;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using SampleSystemUnderTest.VermeulenNearWakeLength;

namespace SampleTests.GeneratedSpecificationSpecific
{
    public class SpecificationSpecificTarget : ReportsSpecificationSetup
    {
        readonly Mock<ITarget> target;

        public ITarget Target =>
            target.Object;

        public SpecificationSpecificTarget()
        {
            target = new Mock<ITarget>();
        }

        internal SpecificationSpecificTarget IntegerProperty_of(Int32 integerProperty)
        {
            valueProperties.Add(GetCurrentMethod(), integerProperty);

            target.Setup(m => m.IntegerProperty).Returns(integerProperty);

            return this;
        }

        internal SpecificationSpecificTarget FloatProperty_of(Single floatProperty)
        {
            valueProperties.Add(GetCurrentMethod(), floatProperty);

            target.Setup(m => m.FloatProperty).Returns(floatProperty);

            return this;
        }

        internal SpecificationSpecificTarget StringProperty_of(String stringProperty)
        {
            valueProperties.Add(GetCurrentMethod(), stringProperty);

            target.Setup(m => m.StringProperty).Returns(stringProperty);

            return this;
        }

        internal SpecificationSpecificTarget DateTimeProperty_of(DateTime dateTimeProperty)
        {
            valueProperties.Add(GetCurrentMethod(), dateTimeProperty);

            target.Setup(m => m.DateTimeProperty).Returns(dateTimeProperty);

            return this;
        }

    }
}
";

            IncrementallyAssertEqual(expected, actual);
        }

        void IncrementallyAssertEqual(string expected, string actual)
        {
            Assert.AreEqual(
                RemoveWhitespaceAndNoiseAndLowerCase(expected),
                RemoveWhitespaceAndNoiseAndLowerCase(actual),
                "Expected and actual code don't match, even when all converted to lowercase and all whitespace and noise are stripped out");

            Assert.AreEqual(
                RemoveWhitespaceAndNoise(expected),
                RemoveWhitespaceAndNoise(actual),
                "Expected and actual code don't match due to a lower case / upper case problem");

            Assert.AreEqual(
                RemoveWhitespaceAndLowerCase(expected),
                RemoveWhitespaceAndLowerCase(actual),
                $"Expected and actual code don't match due to a problem with noise characters ('{noiseCharacters}')");

            Assert.AreEqual(
                RemoveNoiseAndLowerCase(expected),
                RemoveNoiseAndLowerCase(actual),
                $"Expected and actual code don't match due to a whitespace problem");

            Assert.AreEqual(
                RemoveWhitespace(expected),
                RemoveWhitespace(actual),
                $"Expected and actual code don't match due to a problem with casing or noise characters ('{noiseCharacters}')");

            Assert.AreEqual(
                RemoveNoise(expected),
                RemoveNoise(actual),
                "Expected and actual code don't match due to a problem with casing or whitespace");

            // This test was failing on the (windows) build server, despite passing here, due
            // to line ending differences. The build server maybe has a different git checkout
            // setting or something. Either way, I don't really care about them, so making
            // sure they can't fail the test
            Assert.AreEqual(
                StandardiseLineEndings(expected),
                StandardiseLineEndings(actual),
                "Expected and actual code don't match, and it isn't due to noise, casing or whitespace characters");
        }

        // don't care about multiple line endings, or unix / windows line ending differences
        string StandardiseLineEndings(string value) =>
            value
            .Replace("\r\n", "\n")
            .Replace("\n\n", "\n")
            .Replace("\n\n", "\n")
            .Replace("\n\n", "\n");

        string RemoveWhitespaceAndNoiseAndLowerCase(string value) =>
            RemoveWhitespaceAndNoise(value.ToLowerInvariant());

        string RemoveNoiseAndLowerCase(string value) =>
            RemoveNoise(StandardiseLineEndings(value.ToLowerInvariant()));

        string RemoveWhitespaceAndLowerCase(string value) =>
            RemoveWhitespace(value.ToLowerInvariant());

        string RemoveWhitespaceAndNoise(string value) =>
            RemoveWhitespace(RemoveNoise(value));

        string RemoveWhitespace(string value) =>
            Regex.Replace(value, @"\s", "");

        string noiseCharacters =>
            @"(){};,=>.<:";

        string RemoveNoise(string value)
        {
            string removableChars = Regex.Escape(noiseCharacters);
            string pattern = "[" + removableChars + "]";
            return StandardiseLineEndings(Regex.Replace(value, pattern, ""));
        }
    }
}
