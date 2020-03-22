using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            Assert.True(
                new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass));
        }

        [Test]
        public void DoesntMatchFunctionsWithOneParameter()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassSimpleProperty("StringFunction", ExcelPropertyType.String)
            );

            Assert.False(
                new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass));
        }

        [Test]
        public void DoesntMatchesFunctionsWithMultipleParameters()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassSimpleProperty("FunctionWithTwoParameters", ExcelPropertyType.Number)
            );

            Assert.False(
                new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass));
        }

        [Test]
        public void DoesntMatchFunctionsWithNoParameters()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassSimpleProperty("FunctionWithoutParameter", ExcelPropertyType.Null)
            );

            Assert.False(
                new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass));
        }

        [Test]
        public void DoesntMatchFunctionsWithReturnValues()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassSimpleProperty("FunctionWithReturnValue", ExcelPropertyType.String)
            );

            Assert.False(
                new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass));
        }

        [Test]
        public void MatchesIEnumerableProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("IEnumerableProperty", "Target")
            );

            Assert.True(
                new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass));
        }

        [Test]
        public void MatchesListProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("ListProperty", "Target")
            );

            Assert.True(
                new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass));
        }

        [Test]
        public void MatchesIReadOnlyListProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("IReadOnlyListProperty", "Target")
            );

            Assert.True(
                new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass));
        }

        [Test]
        public void MatchesICollectionProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("ICollectionProperty", "Target")
            );

            Assert.True(
                new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass));
        }

        [Test]
        public void MatchesComplexProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexProperty("ComplexProperty", "Target")
            );

            Assert.True(
                new ExcelCsharpClassMatcher(new ExcelCsharpPropertyMatcher()).Matches(
                    typeof(ITarget),
                    excelGivenClass));
        }

    }
}
