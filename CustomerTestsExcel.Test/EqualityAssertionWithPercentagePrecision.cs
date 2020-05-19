using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CustomerTestsExcel.Assertions;
using NUnit.Framework;

namespace CustomerTestsExcel.Test
{
    [TestFixture]
    class EqualityAssertionWithPercentagePrecision
    {
        private void Check(bool shouldPass, double expected, double actual, double precision, string message)
        {
            // we just return actual regardless of the generic type of the assertion, so just use an int and then basically ignore it
            var sut = new EqualityAssertionWithPercentagePrecision<int>((i) => actual, expected, precision);

            Assert.AreEqual(shouldPass, sut.Passed(0), message);
        }

        [Test]
        public void Precision()
        {
            // rounding errors required the use of the number of decimal places in the precision
            Check(shouldPass: true, expected: 1, actual: 1.01, precision: 0.010001, message: "1% precision of 1 should include 1.01");
            Check(shouldPass: true, expected: 1, actual: 0.99, precision: 0.010001, message: "1% precision of 1 should include 0.99");
            Check(shouldPass: false, expected: 0.99, actual: 1, precision: 0.010001, message: "1% precision of 0.99 should not include 1");
            Check(shouldPass: true, expected: -1, actual: -1.01, precision: 0.010001, message: "1% precision of -1 should include -1.01");
            Check(shouldPass: true, expected: -600, actual: 600, precision: 2.000001, message: "200% precision of -600 should include 600");
            Check(shouldPass: true, expected: 1275, actual: -2550, precision: 3.000001, message: "300% precision of 1275 should include -2550");
        }

    }
}
