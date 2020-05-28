using System.Collections.Generic;
using CustomerTestsExcel.ExcelToCode;
using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using NUnit.Framework;

namespace CustomerTestsExcel.Test.ExcelToCodeVisitor
{
    [TestFixture]
    public class ExcelCsharpClassMatcherTest : TestBase
    {
        interface ITarget
        {
            int IntegerProperty { get; }
            float FloatProperty { get; set; }
            string StringProperty { get; set; }
            // Functions can make sense if you are setting up data by using a class directly,
            // but do not make much sense if you are using a mock to set up an interface 
            // (which is the preferred method now)
            // so these shouldn't be matched.
            // a function without parameters that returns a value is basically the same 
            // as a getter, so we should support this, but don't at the moment
            void StringFunction(string parameter);
            void FunctionWithoutParameter();
            // end
            void FunctionWithTwoParameters(int parameter1, int parameter2);
            double FunctionWithReturnValue(string parameter);

            IEnumerable<float> IEnumerableFloatProperty { get; }
            IEnumerable<ITarget> IEnumerableProperty { get; }
            List<ITarget> ListProperty { get; }
            IReadOnlyList<ITarget> IReadOnlyListProperty { get; }
            ICollection<ITarget> ICollectionProperty { get; }
            ITarget ComplexProperty { get; }
        }

        [Test]
        public void MatchesSimpleProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassSimpleProperty("IntegerProperty", ExcelPropertyType.Number),
                new GivenClassSimpleProperty("FloatProperty", ExcelPropertyType.Number),
                new GivenClassSimpleProperty("StringProperty", ExcelPropertyType.String)
            );

            var match = new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass);

            Assert.True(match.Matches);
            Assert.AreEqual(match.PercentMatchingProperties, 1);
        }

        [Test]
        public void DoesntMatchFunctionsWithOneParameter()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassSimpleProperty("StringFunction", ExcelPropertyType.String)
            );

            var match = new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass);

            Assert.True(match.Matches);
            Assert.AreEqual(match.PercentMatchingProperties, 0);
        }

        [Test]
        public void DoesntMatchesFunctionsWithMultipleParameters()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassSimpleProperty("FunctionWithTwoParameters", ExcelPropertyType.Number)
            );

            var match = new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass);

            Assert.True(match.Matches);
            Assert.AreEqual(match.PercentMatchingProperties, 0);
        }

        [Test]
        public void DoesntMatchFunctionsWithNoParameters()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassSimpleProperty("FunctionWithoutParameter", ExcelPropertyType.Null)
            );

            var match = new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass);

            Assert.True(match.Matches);
            Assert.AreEqual(match.PercentMatchingProperties, 0);
        }

        [Test]
        public void DoesntMatchFunctionsWithReturnValues()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassSimpleProperty("FunctionWithReturnValue", ExcelPropertyType.String)
            );

            var match = new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass);

            Assert.True(match.Matches);
            Assert.AreEqual(match.PercentMatchingProperties, 0);
        }

        [Test]
        public void MatchesIEnumerableProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("IEnumerableProperty", "Target")
            );

            var match = new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass);

            Assert.True(match.Matches);
            Assert.AreEqual(match.PercentMatchingProperties, 1);
        }

        [Test]
        public void MatchesListProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("ListProperty", "Target")
            );

            var match = new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass);

            Assert.True(match.Matches);
            Assert.AreEqual(match.PercentMatchingProperties, 1);
        }

        [Test]
        public void MatchesIReadOnlyListProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("IReadOnlyListProperty", "Target")
            );

            var match = new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass);

            Assert.True(match.Matches);
            Assert.AreEqual(match.PercentMatchingProperties, 1);
        }

        [Test]
        public void MatchesICollectionProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("ICollectionProperty", "Target")
            );

            var match = new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass);

            Assert.True(match.Matches);
            Assert.AreEqual(match.PercentMatchingProperties, 1);
        }

        [Test]
        public void MatchesComplexProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexProperty("ComplexProperty", "Target")
            );

            var match = new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass);

            Assert.True(match.Matches);
            Assert.AreEqual(match.PercentMatchingProperties, 1);
        }

        [Test]
        public void MatchesIEnumerableSpecialCaseFloatProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("IEnumerableFloatProperty", "Float")
            );

            var match = new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass);

            Assert.True(match.Matches);
            Assert.AreEqual(match.PercentMatchingProperties, 1);
        }

        [Test]
        public void CalculatePercentMatchingProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("IEnumerableFloatProperty", "Float"),
                new GivenClassComplexListProperty("NonMatchingProperty", "Float")
            );

            var match = new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass);

            Assert.True(match.Matches);
            Assert.AreEqual(match.PercentMatchingProperties, 0.5);
        }
    }
}
