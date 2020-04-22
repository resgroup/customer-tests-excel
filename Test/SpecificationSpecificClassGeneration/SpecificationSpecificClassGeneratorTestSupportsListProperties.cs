using CustomerTestsExcel.SpecificationSpecificClassGeneration;
using NUnit.Framework;
using System.Collections.Generic;

namespace CustomerTestsExcel.Test.SpecificationSpecificClassGeneration
{
    public class SpecificationSpecificClassGeneratorTest : TestBase
    {
        interface ITarget
        {
            IEnumerable<ITarget> IEnumerableProperty { get; }
            List<ITarget> ListProperty { get; }
            IReadOnlyList<ITarget> IReadOnlyListProperty { get; }
            ICollection<ITarget> ICollectionProperty { get; }
        }

        [Test]
        public void SupportsIEnumerableProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("IEnumerableProperty", "Target")
            );

            var actual = new SpecificationSpecificClassGenerator(
                new ExcelCsharpPropertyMatcher(),
                    excelGivenClass
                ).CsharpCode(
                    "SampleTests",
                    new List<string>(),
                    typeof(ITarget)
                );

            var expectedListDeclaration = @"readonly List<SpecificationSpecificTarget> iEnumerablePropertys = new List<SpecificationSpecificTarget>();";

            var expectedMockSetup = @"target.Setup(m => m.IEnumerableProperty).Returns(iEnumerablePropertys.Select(l => l.Target));";

            var expectedListOfSetters =
            @"internal SpecificationSpecificTarget IEnumerableProperty_of(SpecificationSpecificTarget iEnumerableProperty)
        {
            AddClassProperty(new ReportSpecificationSetupClass(GetCurrentMethod(), iEnumerableProperty));

            this.iEnumerablePropertys.Add(iEnumerableProperty);

            return this;
        }";

            var expectedTableOfSetters =
            @"internal SpecificationSpecificTarget IEnumerableProperty_table_of(ReportSpecificationSetupClassUsingTable<SpecificationSpecificTarget> iEnumerablePropertys)
        {
            iEnumerablePropertys.PropertyName = GetCurrentMethod().Name;

            AddClassTableProperty(iEnumerablePropertys);

            foreach (var row in iEnumerablePropertys.Rows)
                this.iEnumerablePropertys.Add(row.Properties);

            return this;
        }";

            StringAssert.Contains(expectedListDeclaration, actual);
            StringAssert.Contains(expectedMockSetup, actual);
            StringAssert.Contains(expectedListOfSetters, actual);
            StringAssert.Contains(expectedTableOfSetters, actual);
            StringAssert.DoesNotContain(
                "AddValueProperty(GetCurrentMethod(), iEnumerableProperty)",
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
                new ExcelCsharpPropertyMatcher(),
                    excelGivenClass
                ).CsharpCode(
                    "SampleTests",
                    new List<string>(),
                    typeof(ITarget)
                );

            var expectedMockSetup = @"target.Setup(m => m.ListProperty).Returns(listPropertys.Select(l => l.Target));";

            // Everything apart from the mock setup is the same as the IEnumerable properties, so not replicating here
            StringAssert.Contains(expectedMockSetup, actual);
        }

        [Test]
        public void SupportsIReadOnlyListProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("IReadOnlyListProperty", "Target")
            );

            var actual = new SpecificationSpecificClassGenerator(
                new ExcelCsharpPropertyMatcher(),
                    excelGivenClass
                ).CsharpCode(
                    "SampleTests",
                    new List<string>(),
                    typeof(ITarget)
                );

            var expectedMockSetup = @"target.Setup(m => m.IReadOnlyListProperty).Returns(iReadOnlyListPropertys.Select(l => l.Target));";

            // Everything apart from the mock setup is the same as the IEnumerable properties, so not replicating here
            StringAssert.Contains(expectedMockSetup, actual);
        }

        [Test]
        public void SupportsICollectionProperties()
        {
            var excelGivenClass = ExcelGivenClass(
                "Target",
                new GivenClassComplexListProperty("ICollectionProperty", "Target")
            );

            var actual = new SpecificationSpecificClassGenerator(
                new ExcelCsharpPropertyMatcher(),
                    excelGivenClass
                ).CsharpCode(
                    "SampleTests",
                    new List<string>(),
                    typeof(ITarget)
                );

            var expectedMockSetup = @"target.Setup(m => m.ICollectionProperty).Returns(iCollectionPropertys.Select(l => l.Target));";

            // Everything apart from the mock setup is the same as the IEnumerable properties, so not replicating here
            StringAssert.Contains(expectedMockSetup, actual);
        }
    }
}
