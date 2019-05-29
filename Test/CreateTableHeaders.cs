using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CustomerTestsExcel;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    class CreateTableHeaders
    {
        [Test]
        public void SimpleProperties()
        {
            var simpleProperties = new TestReportsSpecificationSetup();
            simpleProperties.AddValueProperty("Cedd");
            simpleProperties.AddValueProperty("Chad");

            var expected = CreateHeaders(CreateHeader("Cedd"), CreateHeader("Chad"));

            var calculated = new CustomerTestsExcel.CreateTableHeaders(simpleProperties).Calculate();

            AssertEqualHeaders(expected, calculated);
        }

        private void AssertEqualHeaders(IEnumerable<ITableHeader> expected, IEnumerable<ITableHeader> calculated)
        {
            Assert.AreEqual(expected.Count(), calculated.Count());

            var expectedEnumerator = expected.GetEnumerator();
            var calculatedEnumerator = calculated.GetEnumerator();

            while (expectedEnumerator.MoveNext())
            {
                calculatedEnumerator.MoveNext();

                Assert.IsTrue(expectedEnumerator.Current.Equals(calculatedEnumerator.Current));
            }
        }

        private IEnumerable<ITableHeader> CreateHeaders(params ITableHeader[] tableHeaders)
        {
            return tableHeaders;
        }

        public ITableHeader CreateHeader(string propertyName)
        {
            return new PropertyTableHeader(propertyName);
        }
    }
}
