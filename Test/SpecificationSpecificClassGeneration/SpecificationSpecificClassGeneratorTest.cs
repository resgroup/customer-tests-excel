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
    public class SpecificationSpecificClassGeneratorTest : TestBase
    {
        interface ITarget
        {
            int IntegerProperty { get; }
            float FloatProperty { get; set; }
            string StringProperty { get; set; }
            IEnumerable<ITarget> IEnumerableProperty { get; }
            List<ITarget> ListProperty { get; }
            IReadOnlyList<ITarget> IReadOnlyListProperty { get; }
            ICollection<ITarget> ICollectionProperty { get; }
            ITarget ComplexProperty { get; }
            // can't support functions on interfaces, but could if setting up an object directly (although I think this is probably a niche case
            void StringFunction(string parameter);
            void FunctionWithoutParameter();
            void FunctionWithTwoParameters(int parameter1, int parameter2);
            double FunctionWithReturnValue(string parameter);
        }

        // want a test for a complex property. That is a property that is itself an interface or similar.


        [Test]
        public void SupportsSimpleProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassSimpleProperty("IntegerProperty", ExcelPropertyType.Number),
                new GivenClassSimpleProperty("FloatProperty", ExcelPropertyType.Number),
                new GivenClassSimpleProperty("StringProperty", ExcelPropertyType.String)
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
@"using static System.Reflection.MethodBase;
using Moq;
using CustomerTestsExcel;
using SampleSystemUnderTest.VermeulenNearWakeLength;

namespace SampleTests
{
    internal class SpecificationSpecificTarget : ReportsSpecificationSetup
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

    }
}
";

            IncrementallyAssertEqual(expected, actual);
        }

        [Test]
        public void SupportsIEnumerableProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("IEnumerableProperty", "Target")
            );

            var actual = new SpecificationSpecificClassGenerator(
                new ExcelCsharpPropertyMatcher()
                ).cSharpCode(
                    "SampleTests",
                    new List<string> { "SampleSystemUnderTest.VermeulenNearWakeLength" },
                    typeof(ITarget),
                    excelGivenClass
                );

            var expectedListDeclaration = @"readonly List<SpecificationSpecificTarget> iEnumerablePropertys = new List<SpecificationSpecificTarget>();";

            var expectedMockSetup = @"target.Setup(m => m.IEnumerableProperty).Returns(iEnumerablePropertys.Select(l => l.value));";

            var expectedListOfSetters =
            @"internal SpecificationSpecificTarget IEnumerableProperty_of(SpecificationSpecificTarget iEnumerableProperty)
        {
            classProperties.Add(new ReportSpecificationSetupClass(GetCurrentMethod(), iEnumerableProperty));

            this.iEnumerablePropertys.Add(iEnumerableProperty);

            return this;
        }";

            var expectedTableOfSetters =
            @"internal SpecificationSpecificTarget IEnumerableProperty_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificTarget> iEnumerablePropertys)
        {
            iEnumerablePropertys.PropertyName = GetCurrentMethod().Name;

            classTableProperties.Add(iEnumerablePropertys);

            foreach (var row in iEnumerablePropertys.Rows)
                this.iEnumerablePropertys.Add(row.Properties);

            return this;
        }";

            StringAssert.Contains(expectedListDeclaration, actual);
            StringAssert.Contains(expectedMockSetup, actual);
            StringAssert.Contains(expectedListOfSetters, actual);
            StringAssert.Contains(expectedTableOfSetters, actual);
            StringAssert.DoesNotContain(
                "valueProperties.Add(GetCurrentMethod(), iEnumerableProperty)",
                actual,
                "'Object' Code is being generated for a 'List' property");
        }

        [Test]
        public void SupportsListProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("ListProperty", "Target")
            );

            var actual = new SpecificationSpecificClassGenerator(
                new ExcelCsharpPropertyMatcher()
                ).cSharpCode(
                    "SampleTests",
                    new List<string> { "SampleSystemUnderTest.VermeulenNearWakeLength" },
                    typeof(ITarget),
                    excelGivenClass
                );

            var expectedMockSetup = @"target.Setup(m => m.ListProperty).Returns(listPropertys.Select(l => l.value).ToList());";

            // Everything apart from the mock setup is the same as the IEnumerable properties, so not replicating here
            StringAssert.Contains(expectedMockSetup, actual);
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

            // This test was passing on the (windows) build server, despite passing here, due
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
